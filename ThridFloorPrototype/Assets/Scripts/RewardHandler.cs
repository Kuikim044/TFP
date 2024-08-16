using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardHandler : MonoBehaviour
{

    [SerializeField] private Button btnReciveGem;
   // [SerializeField] private Button btnReciveGold;

    private enum RewardType { None, Gem, Gold }
    private RewardType currentRewardType = RewardType.None;

    void Start()
    {
        RewardedAdsButton.OnRewardedAdCompleted += OnRewardedAdCompleted;

        btnReciveGem.onClick.AddListener(() => ShowAdForReward(RewardType.Gem));
      //  btnReciveGold.onClick.AddListener(() => ShowAdForReward(RewardType.Gold));
    }

    private void OnDestroy()
    {
        RewardedAdsButton.OnRewardedAdCompleted -= OnRewardedAdCompleted;

        btnReciveGem.onClick.RemoveAllListeners();
      //  btnReciveGold.onClick.RemoveAllListeners();
    }

    private void ShowAdForReward(RewardType rewardType)
    {
        currentRewardType = rewardType;
        AdsManager.Instance.RewardedAdsButton.ShowAd();
    }

    private void OnRewardedAdCompleted()
    {
        switch (currentRewardType)
        {
            case RewardType.Gem:
                GiveDiamonds();
                break;
            case RewardType.Gold:
                GiveGold();
                break;
        }

        currentRewardType = RewardType.None;
    }

    private void GiveDiamonds()
    {
        Debug.Log("Player received diamonds from rewarded ad.");
    }

    private void GiveGold()
    {
        Debug.Log("Player received gold from rewarded ad.");
    }
}
