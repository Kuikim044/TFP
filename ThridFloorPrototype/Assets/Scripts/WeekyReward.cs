using Assets.HeroEditor4D.Common.Scripts.Collections;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Data;
using System;

[System.Serializable]
public class WeeklyRewardResponse
{
    public List<WeeklyRewardItem> WeeklyRewardList;
    public string message;
}

[System.Serializable]
public class WeeklyRewardItem
{
    public string ID;
    public string ItemType;
    public string ItemID;
    public string EquipmentPart;
    public string BodyPart;
    public int Quantity;
    public bool RewardReceived;
    public long Date;
}

public class WeekyReward : MonoBehaviour
{
    public IconCollection IconCollection;

    [SerializeField] private Image rewardDay1Img;
    [SerializeField] private Image rewardDay2Img;
    [SerializeField] private Image rewardDay3Img;
    [SerializeField] private Image rewardDay4Img;
    [SerializeField] private Image rewardDay5Img;
    [SerializeField] private Image rewardDay6Img;
    [SerializeField] private Image rewardDay7Img;

    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite gemSprite;

    [SerializeField] private GameObject rewardDay1Claim;
    [SerializeField] private GameObject rewardDay2Claim;
    [SerializeField] private GameObject rewardDay3Claim;
    [SerializeField] private GameObject rewardDay4Claim;
    [SerializeField] private GameObject rewardDay5Claim;
    [SerializeField] private GameObject rewardDay6Claim;
    [SerializeField] private GameObject rewardDay7Claim;

    [SerializeField] private GameObject rewardDay1Claimed;
    [SerializeField] private GameObject rewardDay2Claimed;
    [SerializeField] private GameObject rewardDay3Claimed;
    [SerializeField] private GameObject rewardDay4Claimed;
    [SerializeField] private GameObject rewardDay5Claimed;
    [SerializeField] private GameObject rewardDay6Claimed;
    [SerializeField] private GameObject rewardDay7Claimed;

    [SerializeField] Button OpenReward;
    [SerializeField] Button claimBtn;
    [SerializeField] GameObject claimedObj;
    [SerializeField] private GameObject panelWeeklyReward;
    [SerializeField] private GameObject LoaddingObj;

    private List<Image> rewardImages;
    private List<GameObject> rewardClaims;
    private List<GameObject> rewardClaimeds;
    private void Start()
    {
        rewardImages = new List<Image> { rewardDay1Img, rewardDay2Img, rewardDay3Img, rewardDay4Img, rewardDay5Img, rewardDay6Img, rewardDay7Img };
        rewardClaims = new List<GameObject> { rewardDay1Claim, rewardDay2Claim, rewardDay3Claim, rewardDay4Claim, rewardDay5Claim, rewardDay6Claim, rewardDay7Claim };
        rewardClaimeds = new List<GameObject> { rewardDay1Claimed, rewardDay2Claimed, rewardDay3Claimed, rewardDay4Claimed, rewardDay5Claimed, rewardDay6Claimed, rewardDay7Claimed };

        OpenReward.onClick.AddListener(OpenWeeklyRewardPage);
        claimBtn.onClick.AddListener(ClaimReward);
    }

    private void OnDestroy()
    {
        OpenReward.onClick.RemoveAllListeners();
        claimBtn.onClick.RemoveAllListeners();
    }

    private void OpenWeeklyRewardPage()
    {
        StartCoroutine(FetchWeeklyRewardData());
    }
    private IEnumerator FetchWeeklyRewardData()
    {
        LoaddingObj.SetActive(true);

        string url = "http://116.204.183.220:3000/get/weeklyreward";

        StartCoroutine(ApiManager.Instance.GetRequest(url, (response) =>
        {
            if (!string.IsNullOrEmpty(response))
            {
                LoaddingObj.SetActive(false);

                WeeklyRewardResponse weeklyRewardResponse = JsonConvert.DeserializeObject<WeeklyRewardResponse>(response);
                CheckRewards(weeklyRewardResponse);

                panelWeeklyReward.GetComponent<OpenObjAnimation>().OpenGameObject();

            }
            else
            {
                LoaddingObj.SetActive(false);
                Debug.LogError("Failed to fetch progress data.");
            }
        }));

        yield return null;
    }
    private void CheckRewards(WeeklyRewardResponse response)
    {
        var sortedRewards = response.WeeklyRewardList.OrderBy(item => item.Date).ToList();
        long currentDate = GetCurrentDateInSeconds();

        for (int i = 0; i < sortedRewards.Count; i++)
        {
            if (i >= 7) break;

            var rewardItem = sortedRewards[i];

            var rewardImage = rewardImages[i];
            var claimIndicator = rewardClaims[i];
            var claimedIndicator = rewardClaimeds[i];


            Sprite itemSprite = null;

            if (rewardItem.ItemType == "Gold")
            {
                itemSprite = goldSprite;
            }
            else if (rewardItem.ItemType == "Gem")
            {
                itemSprite = gemSprite;
            }
            else
            {
                var adjustedItemID = AdjustItemIDForEquipmentPart(rewardItem.EquipmentPart, rewardItem.ItemID);
                itemSprite = IconCollection.GetIcon(adjustedItemID);
            }

            rewardImage.sprite = itemSprite;

            var rewardDate = ConvertSecondsToDateTime(rewardItem.Date);
            bool isToday = (rewardDate.Date == DateTime.UtcNow.Date);

            Debug.Log($"Reward Date: {rewardDate:yyyy-MM-dd}: Item ID: {rewardItem.ItemID}, Is Today: {isToday},Reward Received: {rewardItem.RewardReceived}");

            if (isToday)
            {
                if (!rewardItem.RewardReceived)
                {
                    claimBtn.interactable = true;
                    claimedObj.SetActive(false);

                    claimIndicator.SetActive(true);
                    claimedIndicator.SetActive(false);
                }
                else
                {
                    claimBtn.interactable = false;
                    claimedObj.SetActive(true);

                    claimIndicator.SetActive(false);
                    claimedIndicator.SetActive(true);
                }
            }
            else
            {
                claimIndicator.SetActive(false);
                claimedIndicator.SetActive(rewardItem.RewardReceived);
            }

        }
    }
    private string AdjustItemIDForEquipmentPart(string equipmentPart, string itemID)
    {
        if (string.IsNullOrEmpty(itemID)) return itemID;

        return equipmentPart switch
        {
            "Helmet" => itemID.Replace(".Armor.", ".Helmet."),
            "Leggings" => itemID.Replace(".Armor.", ".Leggings."),
            _ => itemID,
        };
    }
    private long GetCurrentDateInSeconds()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
    private DateTime ConvertSecondsToDateTime(long seconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime.ToUniversalTime();
    }


    private void ClaimReward()
    {
        LoaddingObj.SetActive(true);

        string url = "http://116.204.183.220:3000/set/weeklyreward";

        StartCoroutine(ApiManager.Instance.PostRequest(url,"", (response) =>
        {
            LoaddingObj.SetActive(false);

            if (response != null)
            {
                StartCoroutine(FetchWeeklyRewardData()); // �� StartCoroutine ����Ѻ������¡ FetchWeeklyRewardData
                LoginManager.Instance.GetUserData();
                Debug.Log("Reward claimed successfully. Response: " + response);
            }
            else
            {
                LoaddingObj.SetActive(false);
                Debug.LogError("Failed to claim reward.");
            }
        }));
    }

}

