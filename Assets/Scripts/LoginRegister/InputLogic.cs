using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using Newtonsoft.Json;

public class InputLogic : MonoBehaviour
{
    private string domainApi = "https://localhost:7200";
    private string _username;
    private string _password;

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
        
        await HttpClient.Post<AuthenticationRequest>("https://localhost:7200/authentication/login", ar);
        Debug.Log(HttpClient.postResponse);
        if (HttpClient.postResponse.StartsWith("{"))
        {
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
        await HttpClient.Post<AuthenticationRequest>("https://localhost:7200/authentication/register", ar);
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
