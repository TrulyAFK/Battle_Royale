using UnityEngine;
using System.Collections;
using Photon.Realtime;
using Photon.Pun;
public class PlayerController : MonoBehaviourPun
{
    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;
    [Header("Components")]
    public Rigidbody rig;
    public int id;
    public Player photonPlayer;
    [Header("Other")]
    private int curAttackerId;
    public int curHp;
    public int maxHp;
    public int kills;
    public bool dead;
    private bool flashingDamage;
    public MeshRenderer mr;
    public PlayerWeapon weapon;
    private void Update()
    {
        if(!photonView.IsMine||dead)
            return;
        Move();
        if (Input.GetKey(KeyCode.Space))
        {
            TryJump();
        }
        if(Input.GetMouseButtonDown(0)){weapon.TryShoot();}
    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = (transform.forward*z+transform.right*x)*moveSpeed;
        dir.y = rig.linearVelocity.y;
        rig.linearVelocity = dir;
    }
    void TryJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, 1.5f))
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    [PunRPC]
    public void Initialize(Player player){
        id=player.ActorNumber;
        photonPlayer = player;
        GameManager.instance.players[id-1]=this;
        if(!photonView.IsMine){
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
            rig.isKinematic=true;
        }
    }
    [PunRPC]
    public void TakeDamage(int attackerID,int damage){
        if(dead)
            return;
        curHp-=damage;
        curAttackerId=attackerID;
        photonView.RPC("DamageFlash",RpcTarget.Others);
        if(curHp<=0)
            photonView.RPC("Die",RpcTarget.All);
    }
    [PunRPC]
    void DamageFlash(){
        if(flashingDamage)
            return;
        StartCoroutine(DamageFlashCoRoutine());
        IEnumerator DamageFlashCoRoutine(){
            flashingDamage=true;
            Color defaultColor = mr.material.color;
            mr.material.color=Color.white;
            yield return new WaitForSeconds(0.05f);
            mr.material.color=defaultColor;
            flashingDamage=false;
        }
    }
    [PunRPC]
    void Die(){
        curHp = 0;
        dead = true;
        GameManager.instance.alaivePlayers--;
        if (PhotonNetwork.IsMasterClient) { GameManager.instance.CheckWinCondition(); }
        if (curAttackerId != 0)
        {
            GameManager.instance.GetPlayer(curAttackerId).photonView.RPC("AddKill", RpcTarget.All);
            GetComponentInChildren<CameraController>().SetAsSpectator();
            rig.isKinematic = true;
            transform.position = new Vector3(0, -50, 0);
        }
    }

    [PunRPC]//PlayerWeapon RPC call
    void SpawnBullet(Vector3 pos,Vector3 dir){
        GameObject bulletObj = Instantiate(weapon.bulletPrefab,pos,Quaternion.identity);
        bulletObj.transform.forward=dir;
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Initialize(weapon.daamage,weapon.range,id,photonView.IsMine);
        bullet.rb.linearVelocity=dir*bullet.GetSpeed();
    }
    [PunRPC]
    public void AddKill()
    {
        kills++;
    }
}
