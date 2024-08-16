using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    public InterstitialAds InterstitialAds;
    public BannerAds BannerAds;
    public RewardedAdsButton RewardedAdsButton;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        BannerAds.LoadBanner();
        InterstitialAds.LoadInterstitialAd();
        RewardedAdsButton.LoadAd();
    }
    public void ShowInterstitialAd(System.Action onAdClosed)
    {
        InterstitialAds.ShowInterstitialAd(onAdClosed);
    }
}

