using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class Bullet : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

            //PhotonNetwork.Destroy(other.gameObject.GetPhotonView());
            //PhotonNetwork.Destroy(gameObject.GetPhotonView());
        }
    }
}
