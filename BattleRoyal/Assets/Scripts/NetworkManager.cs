using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public int maxPlayers = 10;
    public static NetworkManager instance;

    void Awake(){
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server.");
    }
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers=(byte)maxPlayers;
        PhotonNetwork.CreateRoom(roomName, options);
        Debug.Log(PhotonNetwork.IsMasterClient);
    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnDisconnected(DisconnectCause d){
        PhotonNetwork.LoadLevel("Menu");
    }
    public override void OnPlayerLeftRoom (Player otherPlayer){
        GameManager.instance.alaivePlayers--;
        GameUI.instance.UpdatePlayerInfoText();
        if(PhotonNetwork.IsMasterClient){
            GameManager.instance.CheckWinCondition();
        }
    }
}
