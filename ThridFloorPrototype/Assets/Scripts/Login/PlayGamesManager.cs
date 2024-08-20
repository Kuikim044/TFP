
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Networking;
using System;

public class Login
{
    public string Platform;
    public string UserID;
    public string Name;
}
public class PlayGamesManager : MonoBehaviour
{
    public TextMeshProUGUI DetailsText;
    void Start()
    {
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }
    internal void ProcessAuthentication(SignInStatus status)
    {
       if (status == SignInStatus.Success)
       {

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, (serverAccessCode) =>
            {
                DetailsText.text = $"Success : {name} \n Code : {serverAccessCode}";
                if (!string.IsNullOrEmpty(serverAccessCode))
                {
                    StartCoroutine(SendLoginRequest(name, serverAccessCode));
                }
                else
                {
                    Debug.LogError("Server Access Code is null or empty.");
                }
            });
        }
      else
        {
            DetailsText.text = "Sign in Failed!!";
        }
    }
    private IEnumerator SendLoginRequest(string playerName, string accessCode)
    {
        string url = "http://116.204.183.220:3000/login";

        RequestDataLogin requestData = new()
        {
            Platform = "Android",
            Code = accessCode,
            Name = playerName
        };

        string jsonData = JsonUtility.ToJson(requestData);

        yield return ApiManager.Instance.PostRequest(url, jsonData, (response) =>
        {
            if (!string.IsNullOrEmpty(response))
            {
                Debug.Log("API Response: " + response);
            }
            else
            {
                Debug.LogError("Failed to send data to API.");
            }
        });
    }
}
[Serializable]
public class RequestDataLogin
{
    public string Platform;
    public string Code;
    public string Name;
}