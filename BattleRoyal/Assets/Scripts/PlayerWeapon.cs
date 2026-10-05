using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerWeapon : MonoBehaviour
{
    [Header("Stats")]
    public int damage;
    public int curAmmo;
    public int maxAmmo;
    public float bulletSpeed;
    public float fireRate;

    private float lastShotTime;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPos;

    private PlayerController player;
    void Awake(){
        player=GetComponent<PlayerController>();
    }
    public void TryShoot(){
        Debug.Log("Try Shot");
        if (curAmmo<= 0 || Time.time-lastShotTime<fireRate){return;}
        Debug.Log("shooting");
        curAmmo--;
        lastShotTime=Time.time;
        GameUI.instance.UpdateAmmoText();
        player.photonView.RPC("SpawnBullet",RpcTarget.All,bulletSpawnPos.transform.position,Camera.main.transform.forward);
    }
}
