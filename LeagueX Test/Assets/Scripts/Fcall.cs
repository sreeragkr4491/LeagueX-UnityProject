using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;

public class Fcall : MonoBehaviourPunCallbacks
{
    public Text text;
    int i = 0;
    public void ClickedMe()
    {
        i++;
        photonView.RPC("RPCFunct", RpcTarget.All, i);
    }

    [PunRPC]
    void RPCFunct(int _i)
    {

        text.text = "hi " + _i.ToString();
    }
}
