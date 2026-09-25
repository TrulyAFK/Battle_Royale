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
    public void ChanageScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers=(byte)maxPlayers;
        PhotonNetwork.CreateRoom(roomName, options);
    }
    public void JoinRoom(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
