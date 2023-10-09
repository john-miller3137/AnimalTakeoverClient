
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

public class AuthenticationResponse
{
    [Preserve]
    [JsonConstructor]
    public AuthenticationResponse(string token)
    {
        _token = token;
    }
    private string _token;

    public string Token
    {
        get { return _token; }
        set { _token = value; }
    }


}