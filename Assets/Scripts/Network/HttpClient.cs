using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient
{
    public static string postResponse;
    public static string getResponse;

    public static async Task<T> Get<T>(string endpoint)
    {
        using (var getRequest = CreateRequest(endpoint))
        {
            getRequest.SendWebRequest();
            while (!getRequest.isDone) await Task.Delay(10);
            if (getRequest.downloadHandler.text.StartsWith("{"))
            {
                getResponse = getRequest.downloadHandler.text;
                return JsonConvert.DeserializeObject<T>(getRequest.downloadHandler.text);
            }
            else
            {
                getResponse = getRequest.downloadHandler.text;
            }
        }
        return default(T);
    }

    public static async Task<T> Post<T>(string endpoint, object payload)
    {
        using (var postRequest = CreateRequest(endpoint, RequestType.POST, payload)) {
            postRequest.downloadHandler = new DownloadHandlerBuffer();
            postRequest.SendWebRequest();
            

            while (!postRequest.isDone) await Task.Delay(10);
            if (postRequest.downloadHandler.text.StartsWith("{"))
            {
                postResponse = postRequest.downloadHandler.text;
                return JsonConvert.DeserializeObject<T>(postRequest.downloadHandler.text);
            }
            else
            {
                postResponse = postRequest.downloadHandler.text;
            }
        }

        
        return default(T);
        
    }
    private static UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        Debug.Log(JsonUtility.ToJson(data));
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            // Disable Nagle's algorithm
            if (request.uploadHandler != null)
            {
                if (request.uploadHandler.GetType().GetField("m_Connection", BindingFlags.NonPublic | BindingFlags.Instance) != null)
                {
                    var connField = request.uploadHandler.GetType().GetField("m_Connection", BindingFlags.NonPublic | BindingFlags.Instance);
                    var connObj = connField.GetValue(request.uploadHandler);
                    if (connObj != null)
                    {
                        var socketProp = connObj.GetType().GetProperty("Socket");
                        var socket = (Socket)socketProp.GetValue(connObj);
                        socket.NoDelay = true;
                    }
                }
            }
        } 

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private static void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }
    public enum RequestType
    {
        GET = 0,
        POST = 1,
        PUT = 2
    }
}
