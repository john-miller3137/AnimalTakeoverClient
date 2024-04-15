using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Network;
using UnityEngine;
using SharedLibrary.Requests;
using Newtonsoft.Json;
using Scripts.LoginRegister;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputLogic : MonoBehaviour
{
    private static InputLogic instance;
    public static InputLogic Instance
    
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputLogic>();
                if (instance == null)
                {
                    GameObject go = new GameObject("InputLogic");
                    instance = go.AddComponent<InputLogic>();
                }
            }
            return instance;
        }
    }
    
    private string domainUrl = "https://0.0.0.0:7200";
    private string domainUrl2 = "https://www.animal-craze.com:7200";
    private string requestUrl;
    private string _username;
    private string _password;
    private static string _token;
    private bool localhost = true;
    private NetworkManager networkManager;
    [SerializeField] private GameObject inputLogic, localhostButtonText, loginUI;
    public GameObject loginDisplay, cancelButton, findGameButton, menuScene, pleaseWait, shopButton, animalInfoButton, findGameTwosButton, findGameTwosAiButton;
    public bool loggedIn;
    public static string Token
    {
        get { return _token; }
        set { _token = value; }
    }
    [SerializeField] private string ip = "24.6.211.107";
    [SerializeField] private ushort port = 7200;
    [SerializeField] private GameObject menuSong;
    private AudioSource menuSource;
    public bool isTutorial, tutComplete;
    private bool pleaseWaitBool;
    public AsyncOperation loadGameScene, tutScene;

    // Start is called before the first frame update
    private void Awake()
    {
        isTutorial = !PlayerPrefs.HasKey("tutorial");
    }
    

    void Start()
    {
        if(!SceneManager.GetSceneByName("GameScene1").isLoaded)
        {
            loadGameScene = SceneManager.LoadSceneAsync("GameScene1", LoadSceneMode.Single);
        }

        loggedIn = false;
        pleaseWaitBool = false;
        pleaseWait.SetActive(false);
        
        if(inputLogic != null)
        {
            networkManager = inputLogic.GetComponent<NetworkManager>();
        }
        requestUrl = domainUrl;
        findGameButton.SetActive(false);
        findGameTwosButton.SetActive(false);
        findGameTwosAiButton.SetActive(false);
        animalInfoButton.SetActive(false);
        cancelButton.SetActive(false);
        
        ServicePointManager.ServerCertificateValidationCallback += MyCertificateValidationCallback;
        
        ChangeHost();
    }

    bool MyCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // Always return true to accept all certificates (not recommended for production)
        return true;
    }

    // Remember to detach the callback when it's no longer needed
    void OnDestroy()
    {
        ServicePointManager.ServerCertificateValidationCallback -= MyCertificateValidationCallback;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUsername(string s)
    {
        _username= s;
    }

    public void UpdatePassword(string s)
    {
        _password = s;
    }

    
    public async void OnLogin()
    {
        if (pleaseWaitBool) return;
        if (_username == null || _password == null) return;
        AuthenticationRequest ar = new AuthenticationRequest();
        ar.Username = _username;
        ar.Password = _password;

        StartCoroutine(PleaseWait());
        await HttpClient.Post<AuthenticationRequest>(requestUrl + "/authentication/login", ar);
        
        if (HttpClient.postResponse.StartsWith("{"))
        {
            string inittoken = JsonConvert.DeserializeObject<AuthenticationResponse>(HttpClient.postResponse).Token;
            Token = inittoken.GetUntilOrEmpty();
            Debug.Log(Token);
            if(Token.Length == 16)
            {
                OnLoginSuccess();
            }
            if (JsonConvert.DeserializeObject<AuthenticationResponse>(HttpClient.postResponse).Token == "Success")
            {
                Debug.Log("Yay");
            }
        }
    }

    public void ShowPlayOptions()
    {
        
    }
    public async void OnRegister()
    {
        if (pleaseWaitBool) return;
        if (_username == null || _password == null) return;
        AuthenticationRequest ar = new AuthenticationRequest();
        ar.Username = _username;
        ar.Password = _password;
        StartCoroutine(PleaseWait());
        await HttpClient.Post<AuthenticationRequest>(requestUrl + "/authentication/register", ar);
        Debug.Log(HttpClient.postResponse);
        if (HttpClient.postResponse.StartsWith("{"))
        {
            if (JsonConvert.DeserializeObject<AuthenticationResponse>(HttpClient.postResponse).Token == "Success")
            {
                Debug.Log("Yay");
                OnLogin();
            }
        }
    }

    public void HideUI()
    {
        loginUI.SetActive(false);
    }
    public void ChangeHost()
    {
        if(localhost)
        {
            localhost = false;
            networkManager.Ip = "24.6.211.107";
            networkManager.Port = 7777;
            requestUrl = domainUrl2;
            if (localhostButtonText.activeSelf)
            {
                localhostButtonText.GetComponent<Text>().text = "IP";
            }
           
        } else
        {
            localhost = true;
            networkManager.Ip = "127.0.0.1";
            networkManager.Port = 7777;
            requestUrl = domainUrl;
            if (localhostButtonText.activeSelf)
            {
                localhostButtonText.GetComponent<Text>().text = "Localhost";
            }
        }
    }

    private async void OnLoginSuccess()
    {
        if (NetworkManager.Singleton.MainClient.IsConnected)
        {
            NetworkManager.Singleton.MainClient.Disconnect();
        } else if (NetworkManager.Singleton.MainClient.IsNotConnected)
        {
            InputLogic.Instance.loggedIn = true;
            NetworkManager.Singleton.MainClient.Connect($"{NetworkManager.Singleton.Ip}:{NetworkManager.Singleton.Port}");
        }
        await NetworkManager.Singleton.WaitForConnection();
        NetworkManager.Singleton.MainClient.Connection.CanTimeout = false;
        NetworkManager.Singleton.SendTokenAndKey();
    }
    private IEnumerator PleaseWait()
    {
        pleaseWait.SetActive(true);
        pleaseWaitBool = true;
        yield return new WaitForSeconds(5);
        pleaseWaitBool = false;
        pleaseWait.SetActive(false);
    }
    public void EndGame()
    {
        loginDisplay.SetActive(true);
        //SceneManager.UnloadSceneAsync("LoginRegisterScene");
        MainMenuController.Instance.CancelButton();
        GameLogic.Instance.myDeadAnimals.Clear();
        GameLogic.Instance.enemyDeadAnimals.Clear();
    }

    public void ShowOptions()
    {
        HideUI();
        ShowPlayOptions();
        findGameButton.SetActive(true);
        findGameTwosAiButton.SetActive(true);
        findGameTwosButton.SetActive(true);
        shopButton.SetActive(true);
        animalInfoButton.SetActive(true);
    }
    
    

}

