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
using Game;
using Scripts.LoginRegister;

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
       // NetworkManager.Singleton.MainClient.Disconnected += Disconnected();
    }
    public async void StartMatchmaking()
    {
        InputLogic.Instance.shopButton.SetActive(false);
        InputLogic.Instance.cancelButton.SetActive(true);
        InputLogic.Instance.findGameButton.SetActive(false);
        InputLogic.Instance.findGameTwosButton.SetActive(false);
        InputLogic.Instance.animalInfoButton.SetActive(false);
        InputLogic.Instance.findGameTwosAiButton.SetActive(false);
        MainMenuController.Instance.searchingText.SetActive(true);
        MainMenuController.Instance.searchingForGame = true;
    }
    public void StartMatchmakingDefault()
    {
        StartMatchmaking();
        MessageHandlers.StartMatchmakingDefault();
    }
    public void StartMatchmakingTwos()
    {
        StartMatchmaking();
        MessageHandlers.StartMatchmakingTwos();
    }
    
    public void StartMatchmakingTwosAI()
    {
        StartMatchmaking();
        MessageHandlers.StartMatchmakingTwosAI();
    }
    private EventHandler<DisconnectedEventArgs> Disconnected()
    {
        if (GameLogic.Instance.gameStarted)
        {
            InputLogic.Instance.EndGame();
            GameLogic.Instance.EndGame();
        }
        else if (InputLogic.Instance.loggedIn)
        {
            MainMenuController.Instance.CancelButton();
        }
        return null;
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

    public async Task WaitForConnection()
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
