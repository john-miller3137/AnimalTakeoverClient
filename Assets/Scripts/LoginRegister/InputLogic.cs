using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using Newtonsoft.Json;
using UnityEngine.UI;

public class InputLogic : MonoBehaviour
{
    private string domainUrl = "https://localhost:7200";
    private string domainUrl2 = "https://73.252.141.178:7200";
    private string requestUrl;
    private string _username;
    private string _password;
    private static string _token;
    private bool localhost = true;
    private NetworkManager networkManager;
    [SerializeField] private GameObject inputLogic, localhostButtonText, loginUI;

    public static string Token
    {
        get { return _token; }
        set { _token = value; }
    }
    [SerializeField] private string ip = "73.252.141.178";
    [SerializeField] private ushort port = 7200;

    // Start is called before the first frame update
    void Start()
    {
        if(inputLogic != null)
        {
            networkManager = inputLogic.GetComponent<NetworkManager>();
        }
        requestUrl = domainUrl;
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
            }
            if (JsonConvert.DeserializeObject<AuthenticationResponse>(HttpClient.postResponse).Token == "Success")
            {
                Debug.Log("Yay");
            }
        }
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
            networkManager.Ip = "73.252.141.178";
            networkManager.Port = 7777;
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
}
