
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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


           DetailsText.text = $"Success{name} \n UserID{id}";

       }
      else
        {
            DetailsText.text = "Sign in Failed!!";
        }
    }
}
