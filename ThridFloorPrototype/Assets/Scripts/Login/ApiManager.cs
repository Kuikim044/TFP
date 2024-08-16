using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    public static ApiManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }

    public IEnumerator PostRequest(string url, string jsonData, Action<string> callback)
    {
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        www.uploadHandler = new UploadHandlerRaw(postData);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
            callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
            callback?.Invoke(www.downloadHandler.text);
        }
    }

    public IEnumerator GetRequest(string url, Action<string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
            callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
            callback?.Invoke(www.downloadHandler.text);
        }
    }

    public IEnumerator PutRequest(string url, string jsonData, Action<string> callback)
    {
        byte[] putData = Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest www = new UnityWebRequest(url, "PUT");
        www.uploadHandler = new UploadHandlerRaw(putData);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
            callback?.Invoke(null);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
            callback?.Invoke(www.downloadHandler.text);
        }
    }
}
