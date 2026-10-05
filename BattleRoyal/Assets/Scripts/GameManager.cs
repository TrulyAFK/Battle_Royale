using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
public class GameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string playerPrefabLocation;
    public PlayerController[] players;
    public Transform[] spawnPoints;
    public int alaivePlayers;
    public float postGameTime;

    private int playersInGame;
    
    public static GameManager instance;
    void Awake(){
        instance=this;
    }

    void Start(){
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        alaivePlayers = players.Length;
        photonView.RPC("ImInGame",RpcTarget.AllBuffered);
        CheckWinCondition();
    }

    public PlayerController GetPlayer(int id){
        foreach (PlayerController player in players)
        {
            if(player!=null&&player.id==id){
                return player;
            }
        }
        return null;
        //return players.First(x=>x.id==id);
    }
    public PlayerController GetPlayer(GameObject playerObj){
        foreach (PlayerController player in players)
        {
            if(player!=null&&player.gameObject==playerObj){
                return player;
            }
        }
        return null;
        //return players.First(x=>x.gameObject==player);
    }

    [PunRPC]
    void ImInGame(){
        playersInGame++;
        if(PhotonNetwork.IsMasterClient&&playersInGame==PhotonNetwork.PlayerList.Length){
            photonView.RPC("SpawnPlayer",RpcTarget.All);
        }
    }

    [PunRPC]
    void SpawnPlayer(){
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation,spawnPoints[Random.Range(0,spawnPoints.Length)].position,Quaternion.identity);
        playerObj.GetComponent<PlayerController>().photonView.RPC("Initialize",RpcTarget.All,PhotonNetwork.LocalPlayer);
    }
    public void CheckWinCondition()
    {
        if (alaivePlayers == 1) {photonView.RPC("WinGame",RpcTarget.All, players.First(x => !x.dead).id);}
    }
    [PunRPC]
    void WinGame(int winnerId)
    {
        GameUI.instance.SetWinText(GetPlayer(winnerId).photonPlayer.NickName);
        Invoke("GoBackToMenu", postGameTime);
    }
    void GoBackToMenu()
    {
        NetworkManager.instance.ChangeScene("Menu");
    }
}
