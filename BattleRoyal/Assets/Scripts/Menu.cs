using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class Menu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [Header("Screens")]
    public GameObject main;
    public GameObject createRoom;
    public GameObject lobby;
    public GameObject lobbyBrowser;
    [Header("Main Screen")]
    public Button createRoomButton;
    public Button findRoomButton;
    [Header("Create Room Screen")]
    public TMP_InputField roomName;

    [Header("Lobby")]
    public TextMeshProUGUI playerListText;
    public TextMeshProUGUI roomInfoText;
    public Button startGameButton;
    [Header("Lobby Browser")]
    public RectTransform roomListContent;
    public GameObject roomButtonPrefab;

    private List<GameObject> roomButtons = new List<GameObject>();
    private List<RoomInfo> roomList = new List<RoomInfo>();

    void Start()
    {
        createRoomButton.interactable = false;
        findRoomButton.interactable = false;
        Cursor.lockState = CursorLockMode.None;
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }
    void SetScreen(GameObject screen)
    {
        main.SetActive(false);
        createRoom.SetActive(false);
        lobby.SetActive(false);
        lobbyBrowser.SetActive(false);

        screen.SetActive(true);
        if(screen == lobbyBrowser)
        {
            UpdateLobbyBrowserUI();
        }
    }
    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        findRoomButton.interactable = true;
    }
    public void Back()
    {
        SetScreen(main);
    }
    //main screen functions
    public void OnPlayerNameValueChanged(TMP_InputField nameInput)
    {
        PhotonNetwork.NickName = nameInput.text;
    }
    public void OnCreateRoomButton()
    {
        SetScreen(createRoom);
    }
    public void OnFindRoomButton()
    {
        SetScreen(lobbyBrowser);
    }
    //create room screen functions
    public void OnCreateButton()
    {
        NetworkManager.instance.CreateRoom(roomName.text);
        SetScreen(lobby);
    }
    
    //lobby screen functions
    public void OnJoinRoom()
    {
        SetScreen(lobby);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }
    [PunRPC]
    void UpdateLobbyUI()
    {
        startGameButton.interactable = PhotonNetwork.IsMasterClient;
        playerListText.text = "";
        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n";
        }
        roomInfoText.text = "<b>Room Name</b>\n" + PhotonNetwork.CurrentRoom.Name;
    }
    public override void OnPlayerLeftRoom(Player player)
    {
        UpdateLobbyUI();
    }
    public void OnStartGameButton()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All,"Game");
    }
    public void OnLeaveRoomButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(main);
    }
    //Lobby browser screen functions
    public void UpdateLobbyBrowserUI()
    {
        foreach(GameObject button in roomButtons)
        {
            button.SetActive(false);
        }
        for(int x=0; x<roomList.Count; x++)
        {
            GameObject button = x>=roomButtons.Count ? CreateRoomButton() : roomButtons[x];
            button.SetActive(true);
            button.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text = roomList[x].Name;
            button.transform.Find("PlayerCountText").GetComponent<TextMeshProUGUI>().text = roomList[x].PlayerCount + "/" + roomList[x].MaxPlayers;
        }
    }
    GameObject CreateRoomButton()
    {
        GameObject buttonObj = Instantiate(roomButtonPrefab, roomListContent.transform);
        roomButtons.Add(buttonObj);
        return buttonObj;
    }
    public void OnJoinRoomButton(string roomName)
    {
        NetworkManager.instance.JoinRoom(roomName);
    }
    public void OnRefreshButton()
    {
        UpdateLobbyBrowserUI();
    }
    public void onRoomListUpdate(List<RoomInfo> allRooms)
    {
        roomList = allRooms;
    }
}