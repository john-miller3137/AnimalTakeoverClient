using UnityEngine;
using System.Collections;
using Riptide.Utils;
using Riptide;
using Riptide.Transports.Udp;
using System;
using Network;
using SharedLibrary;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private string ip;
    [SerializeField] private ushort port;
    public static Client MainClient { get; set; }
    private static NetworkManager _singleton;
    
    
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null) { _singleton = value; }
            else if(_singleton!=null)
            {
                Debug.Log("singleton networkmanager destroyed");
                Destroy(value);
            }
        }
    }
    // Use this for initialization
    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, true);
        MainClient = new Client(new UdpClient(SocketMode.IPv4Only));
    }
    public void ConnectClient()
    {
        MainClient.Connect($"{ip}:{port}");

    }
    public void DoSomething()
    {
        MainClient.Send(Message.Create(MessageSendMode.Unreliable, (ushort)MessageResponseCodes.SendToken).AddString(MessageHandlers.Key).AddString(InputLogic.Token));
    }
    public void FixedUpdate()
    {
        MainClient.Update();
    }
    public void OnApplicationQuit()
    {
        MainClient.Disconnect();
    }

    
}
