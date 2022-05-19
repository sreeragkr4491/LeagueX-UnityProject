using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public GameObject player;
    public bool enableControll = false;
    public Transform[] spawnPoints;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
    }

    void Update()
    {

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {

    }
    public string myPlayerName = "";
    public string otherPlayerName = "";
    public override void OnJoinedRoom()
    {
        if (Player.localPlayer == null)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            Vector3 spawnPosition = spawnPoints[playerCount - 1].position;
            GameObject playerGameobject = PhotonNetwork.Instantiate(player.name, spawnPosition, Quaternion.identity);
            if (playerCount == 2)
            {
                playerGameobject.GetComponent<Player>().SetAsOtherPlayer();
                Invoke("DisplayNames", 0.5f);
            }
        }
    }

    public void Replay()
    {
        // it should reset players position n health
        Player.localPlayer.GetComponent<Player>().Replay();
    }

    int noOfPlayerReady = 0;
    public void AreBothPlayerReady()
    {
        noOfPlayerReady++;
        if (noOfPlayerReady == 1)
        {
            UIManager.instance.StatusText("waiting for other player");
        }
        if (noOfPlayerReady == 2)
        {
            enableControll = true;
            UIManager.instance.StatusText("");
            noOfPlayerReady = 0;
        }
    }

    void DisplayNames()
    {
        photonView.RPC("RPCDisplayNames", RpcTarget.All, null);
    }

    [PunRPC]
    void RPCDisplayNames()
    {
        myPlayerName = Player.localPlayer.GetComponent<Player>().playerName;
        otherPlayerName = GetOtherPlayerName(myPlayerName);
        UIManager.instance.DisplayPlayerNames();
        enableControll = true;
    }


    public string GetOtherPlayerName(string _outPlayer)
    {
        if (PhotonNetwork.PlayerList[0].NickName == _outPlayer)
        {
            return PhotonNetwork.PlayerList[1].NickName;
        }
        else
        {
            return PhotonNetwork.PlayerList[0].NickName;
        }
    }
}
