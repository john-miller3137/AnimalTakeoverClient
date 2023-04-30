using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using Newtonsoft.Json;

public class InputLogic : MonoBehaviour
{
    private string domainApi = "https://localhost:7200";
    private string _username;
    private string _password;
    private static string _token;

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
        AuthenticationRequest ar = new AuthenticationRequest();
        ar.Username = _username;
        ar.Password = _password;

        string requestUrl = ip + ":" + port;
        await HttpClient.Post<AuthenticationRequest>(domainApi + "/authentication/login", ar);
        
        if (HttpClient.postResponse.StartsWith("{"))
        {
            string inittoken = JsonConvert.DeserializeObject<AuthenticationResponse>(HttpClient.postResponse).Token;
            Token = inittoken.GetUntilOrEmpty();
            Debug.Log(Token);
            if (JsonConvert.DeserializeObject<AuthenticationResponse>(HttpClient.postResponse).Token == "Success")
            {
                Debug.Log("Yay");
            }
        }
    }

    public async void OnRegister()
    {
        AuthenticationRequest ar = new AuthenticationRequest();
        ar.Username = _username;
        ar.Password = _password;
        string requestUrl = ip + ":" + port;
        await HttpClient.Post<AuthenticationRequest>(domainApi + "/authentication/register", ar);
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
}
