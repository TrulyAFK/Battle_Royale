using UnityEngine;
using Photon.Pun;
public enum PickupType
{
    Health, Ammo
}
public class Pickup : MonoBehaviourPun
{
    public bool canRespawn=false;
    public float time=10;
    public float delayTime=10;


    public PickupType type;
    public int value;
    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        Debug.Log("Entered");
        if (other.CompareTag("Player"))
        {
            PlayerController player = GameManager.instance.GetPlayer(other.gameObject);
            if (type == PickupType.Health)
            {
                player.photonView.RPC("Heal", player.photonPlayer, value);
            } else if(type == PickupType.Ammo)
            {
                player.photonView.RPC("GiveAmmo", player.photonPlayer, player.weapon,value);
            }
            if(!canRespawn){
                photonView.RPC("DestroyPickup", RpcTarget.AllBuffered);
            } else {
                photonView.RPC("Respawn",RpcTarget.AllBuffered);
            }
            
        }
    }
    [PunRPC]
    public void DestroyPickup()
    {   
        Destroy(gameObject);
    }
    [PunRPC]
    public  async void Respawn(){
        this.gameObject.SetActive(false);
        Debug.Log("No Ammo, starting respawn");
        await Awaitable.WaitForSecondsAsync(15);
        time=delayTime;
        this.gameObject.SetActive(true);
        Debug.Log("Ammo respawned");
    }
}



