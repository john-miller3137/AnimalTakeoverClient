using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Network;
using UnityEngine;
using SharedLibrary.Requests;
using Newtonsoft.Json;
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
    
    private string domainUrl = "https://localhost:7200";
    private string domainUrl2 = "https://24.6.211.107:7201";
    private string requestUrl;
    private string _username;
    private string _password;
    private static string _token;
    private bool localhost = true;
    private NetworkManager networkManager;
    [SerializeField] private GameObject inputLogic, localhostButtonText, loginUI, findGameButton;
    [SerializeField] private GameObject loginDisplay, optionsBackground;
    public static string Token
    {
        get { return _token; }
        set { _token = value; }
    }
    [SerializeField] private string ip = "24.6.211.107";
    [SerializeField] private ushort port = 7201;
    [SerializeField] private GameObject menuSong;
    private AudioSource menuSource;

    // Start is called before the first frame update
    void Start()
    {
        menuSource = menuSong.GetComponent<AudioSource>();
        if(inputLogic != null)
        {
            networkManager = inputLogic.GetComponent<NetworkManager>();
        }
        requestUrl = domainUrl;
        findGameButton.SetActive(false);
        if(!SceneManager.GetSceneByName("GameScene1").isLoaded)
        {
            SceneManager.LoadSceneAsync("GameScene1", LoadSceneMode.Additive);
        }
        menuSource.Play();
    }

    public void StopMenuSource()
    {
        menuSource.Stop();
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
        if (_username == null || _password == null) return;
        AuthenticationRequest ar = new AuthenticationRequest();
        ar.Username = _username;
        ar.Password = _password;

        
        await HttpClient.Post<AuthenticationRequest>(requestUrl + "/authentication/login", ar);
        
        if (HttpClient.postResponse.StartsWith("{"))
        {
            string inittoken = JsonConvert.DeserializeObject<AuthenticationResponse>(HttpClient.postResponse).Token;
            Token = inittoken.GetUntilOrEmpty();
            Debug.Log(Token);
            if(Token.Length == 16)
            {
                HideUI();
                ShowPlayOptions();
                findGameButton.SetActive(true);
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
        if (_username == null || _password == null) return;
        AuthenticationRequest ar = new AuthenticationRequest();
        ar.Username = _username;
        ar.Password = _password;
        
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
            networkManager.Port = 7778;
            requestUrl = domainUrl2;
            localhostButtonText.GetComponent<Text>().text = "IP";
        } else
        {
            localhost = true;
            networkManager.Ip = "127.0.0.1";
            networkManager.Port = 7777;
            requestUrl = domainUrl;
            localhostButtonText.GetComponent<Text>().text = "Localhost";
        }
    }
    public void EndGame()
    {
        loginDisplay.SetActive(true);
        NetworkManager.Singleton.MainClient.Disconnect();
        //SceneManager.UnloadSceneAsync("LoginRegisterScene");
    }
}
