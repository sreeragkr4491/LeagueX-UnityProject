using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;

public class RoomContent : MonoBehaviour
{
    public static RoomContent instance;
    public GameObject roomBtnPrefab;
    public List<RoomInfo> roomList;
    public List<GameObject> allBtns;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoomListDetails(List<RoomInfo> _roomList)
    {
        foreach (var item in allBtns)
        {
            Destroy(item);
        }


        roomList = _roomList;
        foreach (RoomInfo roomInfo in roomList)
        {
            GameObject newBtn = Instantiate(roomBtnPrefab, transform);
            newBtn.GetComponent<Button>().onClick.AddListener(() => JoinRoom(roomInfo.Name));
            newBtn.GetComponentInChildren<Text>().text = roomInfo.Name + ":" + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
            allBtns.Add(newBtn);
        }
    }
    void JoinRoom(string _roomName)
    {
        PhotonNetwork.NickName = StartNetwork.instance.playerName;
        PhotonNetwork.JoinRoom(_roomName);
    }
}
