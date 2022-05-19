using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject localPlayer;
    public string playerName;
    public Gradient healthGradient;
    public Image healthImage;
    public float maxHealth = 100;
    public TextMesh playerNameText;
    public float mouseX, mouseY;
    public Transform camTrans;
    public float speed = 2f; 
    public float sensitivity = 5f;
    public Vector3 startingPosition;
    bool gameOver = false;
    public float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (gameOver)
            {
                return;
            }

            currentHealth = value;
            healthImage.fillAmount = currentHealth / 100;
            healthImage.color = healthGradient.Evaluate(currentHealth / 100);

            if (currentHealth <= 0f)
            {
                currentHealth = 0;
                // gameOver
                GameOver();
                playAgain = false;
                gameOver = true;
            }
        }
    }
    private void Awake()
    {
        if (photonView.IsMine)
        {
            localPlayer = gameObject;
            CurrentHealth = maxHealth;
        }
        shootDirection = Vector3.right;
    }

    void Start()
    {
        startingPosition = transform.position;
        playerName = photonView.Owner.NickName;
        playerNameText.text = photonView.Owner.NickName;
    }

    Vector3 move;
    void Update()
    {
        
        //if (photonView.IsMine && GameManager.instance.enableControll)
        if (photonView.IsMine)
        {
            Movement();
            if (Input.GetMouseButtonUp(0))
            {
                Shoot();
            }
        }
        void Movement()
        {
            mouseX += Input.GetAxis("Mouse X") * sensitivity;
            mouseY += Input.GetAxis("Mouse Y") * sensitivity;
            mouseY = Mathf.Clamp(mouseY, -25f, 35f);
            transform.rotation = Quaternion.Euler(0f, mouseX, transform.rotation.eulerAngles.z);
            camTrans.rotation = Quaternion.Euler(-mouseY, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            transform.position += (transform.forward) * vertical * speed * Time.deltaTime;
            transform.position += transform.right * horizontal * speed * Time.deltaTime / 2f;

        }
    }


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // playerNameText.text = newPlayer.NickName;
    }

    Vector3 shootDirection;
 
    public void SetAsOtherPlayer()
    {
        bulletSpwanPoint.localPosition = -bulletSpwanPoint.localPosition;
        shootDirection = -shootDirection;
    }

    public Transform bulletSpwanPoint;
    public GameObject bulletPrefab;


    void Shoot()
    {
        GameObject newBullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletSpwanPoint.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(camTrans.forward * 20f,ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            // return;
        }

        if (other.tag == "Bullet")
        {
            CurrentHealth -= 25f;
            Destroy(other.gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurrentHealth);
        }
        else
        {
            CurrentHealth = (float)stream.ReceiveNext();
        }
    }


    public const byte win_lose_EventCode = 1;
    public void Replay()
    {
        if (photonView.IsMine)
        {
            transform.position = startingPosition;
            photonView.RPC("RPCResetHealth", RpcTarget.All,null);
        }
    }

    public bool playAgain = false;
    [PunRPC]
    void RPCResetHealth()
    {
        playAgain = true;
        gameOver = false;
        CurrentHealth = maxHealth;
        GameManager.instance.AreBothPlayerReady();
    }


    bool winORLose = false;
    void GameOver()
    {
        if (photonView.IsMine)
        {
            
            winORLose = true;
            object[] data = new object[] { winORLose };
            PhotonNetwork.RaiseEvent(win_lose_EventCode, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
            UIManager.instance.GameOverPanel(!winORLose);
            print("GameOver");
        }
    }

    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == win_lose_EventCode)
        {
            if (photonView.IsMine)
            {
                GameManager.instance.enableControll = false;
                object[] data = (object[])obj.CustomData;
                winORLose = (bool)data[0];
                print("NetworkingClient_EventReceived " + photonView.IsMine);
                UIManager.instance.GameOverPanel(winORLose);
            }
        }
    }
}