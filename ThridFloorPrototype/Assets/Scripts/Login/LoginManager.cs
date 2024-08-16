using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Progress
{
    public int Region;
    public int Stage;
}

[Serializable]
public class InventoryItems
{
    public string ID;
    public string Name;
}

[Serializable]
public class Quest
{
    public string ID;
    public string Name;
    public string Description;
    public int Progress;
    public int Quantity;
    public bool RewardReceived;
}

[Serializable]
public class Reward
{
    public string ID;
    public string ItemType;
    public string ItemID;
    public int Quantity;
    public bool RewardReceived;
    public long Date; 
}

[Serializable]
public class Player
{
    public string ID;
    public int UserID;
    public string Name;
    public int Gold;
    public int Gem;
    public List<InventoryItems> Inventory;
    public List<Quest> QuestList;
    public List<Reward> RewardList;
    public long QuestGeneratedTime;
    public long WeeklyRewardGeneratedTime;
    public Progress Progress = new Progress();
    public bool AdRemoved;
}

[Serializable]
public class ResponseData
{
    public Player Player;
    public string message;

    public ResponseData()
    {
        Player = new Player();
    }
}

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;

    [SerializeField] private ApiManager apiManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        StartCoroutine(Login());
    }
    IEnumerator Login()
    {
        if (DataCenter.Instance == null)
        {
            Debug.LogError("DataCenter.Instance is null in Login.");
            yield break;
        }

        string url = "http://116.204.183.220:3000/login";

        yield return apiManager.PostRequest(url, "", (response) =>
        {
            if (response != null)
            {
                //StartCoroutine(GetUserData());
                StartCoroutine(DataCenter.Instance.GetShopData());
                Debug.Log("Login Success: " + response);
            }
            else
            {
                Debug.LogError("Failed to receive response from login.");
            }
        });
    }

    public IEnumerator GetUserData()
    {
        string url = "http://116.204.183.220:3000/get/playerdata";

        yield return apiManager.GetRequest(url, (response) =>
        {
            if (response != null)
            {
                Debug.Log("Get User Data Success: " + response);

                try
                {
                    Debug.Log("Raw JSON Response: " + response);

                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(response);

                    if (responseData != null && responseData.Player != null)
                    {
                        Debug.Log("ResponseData.Player is not null.");
                        UpdateDataCenter(responseData.Player);
                    }
                    else
                    {
                        Debug.LogError("ResponseData or Player is null after deserialization.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception during JSON deserialization: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Failed to receive response from GetUserData.");
            }
        });
    }

    private void UpdateDataCenter(Player player)
    {
        PlayerData playerData = DataCenter.Instance.GetPlayerData();

        if (playerData != null)
        {
            Debug.Log("Updating DataCenter with player data:");
            Debug.Log("Region: " + player.Progress.Region);
            Debug.Log("Stage: " + player.Progress.Stage);
            Debug.Log("Gold: " + player.Gold);
            Debug.Log("Gem: " + player.Gem);

            playerData.ID = player.ID;
            playerData.UserID = player.UserID;
            playerData.region = player.Progress.Region;
            playerData.stage = player.Progress.Stage;
            playerData.Coin = player.Gold;
            playerData.Diamond = player.Gem;
            playerData.Inventory = player.Inventory;

            Debug.Log("DataCenter updated successfully.");
        }
        else
        {
            Debug.LogError("PlayerData is not set in DataCenter.");
        }
    }
}
