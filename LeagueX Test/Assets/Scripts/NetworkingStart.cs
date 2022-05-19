using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class NetworkingStart : MonoBehaviourPunCallbacks, IConnectionCallbacks
{
    public static NetworkingStart instance;
    //public GameObject loginPanel;
    //public GameObject playerPanel;
    public GameObject connectionPanel;
    private string roomName = "";
    public string playerName = "";
    private string playerNameKey = "playerNamekey";
    private string roomNameKey = "roomNamekey";
    public InputField inputFieldPlayerName;
    public InputField inputFieldRoomName;
    public Text statusText;
    string lobbyName = "isca";
    bool isConnectedToMaster = false;
    void Awake()
    {
        instance = this;
        Application.runInBackground = true;    //TO run in background when  its minimized
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        statusText.text = ("Connecting.....");
        playerName = PlayerPrefs.GetString(playerNameKey);
        roomName = PlayerPrefs.GetString(roomNameKey);
        inputFieldPlayerName.text = playerName;
        inputFieldRoomName.text = roomName;
    }
    void Update()
    {

    }
    public override void OnConnectedToMaster()
    {
        statusText.text = ("Connected to Master");
        base.OnConnectedToMaster();
        isConnectedToMaster = true;
        //loginPanel.GetComponent<Button>().interactable = true;
        //playerPanel.GetComponent<Button>().interactable = true;
        connectionPanel.GetComponent<CanvasGroup>().interactable = true;
    }
    public void CreateRoom()
    {
        if (isConnectedToMaster)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                statusText.text = ("enter room name");
                return;
            }
            statusText.text = ("creating room with name " + roomName);
            PhotonNetwork.NickName = playerName;
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = 4;
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", 1 } };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "C0" };
            TypedLobby typedLobby = new TypedLobby(lobbyName, LobbyType.SqlLobby);
            PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
        }
    }
    public override void OnCreatedRoom()
    {
        statusText.text = ("room is created with name " + roomName);
    }
    public void InfRoomName(string _roomName)
    {
        roomName = _roomName;
        PlayerPrefs.SetString(roomNameKey, roomName);
    }
    public void InfPlayerName(string _playerName)
    {
        playerName = _playerName;
        PlayerPrefs.SetString(playerNameKey, playerName);
    }

    public void SearchRooms()
    {
        TypedLobby typedLobby = new TypedLobby(lobbyName, LobbyType.SqlLobby);
        string sqlLobbyFilter = "C0";
        bool list = PhotonNetwork.GetCustomRoomList(typedLobby, sqlLobbyFilter);
        statusText.text = ("searching for rooms");
    }

    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        RoomsContent.instance.CreateRoomListDetails(_roomList);
        statusText.text = ("rooms found " + _roomList.Count);

    }
    public override void OnJoinedRoom()
    {
        connectionPanel.SetActive(false);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        connectionPanel.SetActive(true);
    }

}
