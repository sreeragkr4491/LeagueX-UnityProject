using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ChatScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject chatPanel;
    public Text textChat;
    public Color notificationColor;
    public Color clearNotificationColor;
    public Text notificationCountText;
    int notificationCount = 0;
    public Button chatButton;
    public Button sendButton;
    public InputField messageInput;
    public bool isChatOpen = false;
    public bool IsChatOpen
    {
        get { return isChatOpen; }
        set
        {
            isChatOpen = value;
            if (isChatOpen)
            {
                chatPanel.transform.position = new Vector3(862f, 360f, 0f);
                ClearNotificationMessage();
            }
            else
            {
                chatPanel.transform.position = new Vector3(1200f, 360f, 0f);
            }
        }
    }

    void Start()
    {
        IsChatOpen = false;
        sendButton.onClick.AddListener(() => SendMessage());
        chatButton.onClick.AddListener(()=> OpenClose());
        notificationCountText.text = "";
        notificationCount = 0;
        clearNotificationColor = chatButton.GetComponent<Image>().color;
    }

    public void OpenClose()
    {
        IsChatOpen = !IsChatOpen;
    }

    public void SendMessage()
    {
        // post the message to all players
        if (!string.IsNullOrEmpty(messageInput.text))
        {
            photonView.RPC("ChatMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + messageInput.text);
            messageInput.text = "";
        }
;
    }

    [PunRPC]
    void ChatMessage(string message)
    {
        textChat.text += "\n" + message;
        if (!IsChatOpen)
        {
            NotificationMessage();
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    void NotificationMessage()
    {
        notificationCount++;
        notificationCountText.text = notificationCount.ToString();
        chatButton.GetComponent<Image>().color = notificationColor;
    }
    void ClearNotificationMessage()
    {
        chatButton.GetComponent<Image>().color = clearNotificationColor;
        notificationCount = 0;
        notificationCountText.text = "";
    }
}
