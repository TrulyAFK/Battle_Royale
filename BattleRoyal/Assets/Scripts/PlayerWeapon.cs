using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerWeapon : MonoBehaviour
{
    [Header("Stats")]
    public int daamage;
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
        if(curAmmo<= 0 || Time.time-lastShotTime<fireRate){return;}
        curAmmo--;
        lastShotTime=Time.time;
        //UI update

        player.photonView.RPC("SpawnBullet",RpcTarget.All,bulletSpawnPos.transform.position,Camera.main.transform.forward);
    }
}
