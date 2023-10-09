using UnityEngine;
using System.Collections;
using Riptide.Utils;
using Riptide;
using Riptide.Transports.Udp;
using System;
using Network;
using SharedLibrary;
using System.Threading;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private string ip;
    [SerializeField] private ushort port;
    public Client MainClient { get; set; }
    private static NetworkManager _singleton;
    
    public string Ip
    {
        get { return ip; }
        set { ip = value; }
    }
    public ushort Port
    {
        get { return port; }
        set { port = value; }
    }
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
    public async void StartMatchmaking()
    {
        if (MainClient.IsConnected)
        {
            MainClient.Disconnect();
        }
        if (MainClient.IsNotConnected)
        {
            MainClient.Connect($"{ip}:{port}");
        }
        await WaitForConnection();
        MainClient.Connection.CanTimeout = false;
        SendTokenAndKey();
        
    }
    public void SendTokenAndKey()
    {
        Debug.Log("token and key sending");
        MainClient.Send(Message.Create(MessageSendMode.Reliable, (ushort)MessageResponseCodes.SendToken).AddString(MessageHandlers.Key).AddString(InputLogic.Token));
    }
    public void FixedUpdate()
    {
        MainClient.Update();
    }
    public void OnApplicationQuit()
    {
        MainClient.Disconnect();
    }

    async Task WaitForConnection()
    {
        CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        while (!MainClient.IsConnected)
        {
            try
            {
                Debug.Log("trying to connect...");
                await Task.Delay(1000, cts.Token);
            }
            catch (TaskCanceledException)
            {
                throw new TimeoutException("Connection timed out.");
            }
        }
    }
}
