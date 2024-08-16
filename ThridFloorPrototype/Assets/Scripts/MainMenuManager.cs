using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.IO;
using System;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private VerticalScrollMap mapImg;

    [SerializeField] private Button startGameBtn;

    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI enegyText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [SerializeField] private GameObject panelMap;
    [SerializeField] private GameObject panelSelectStage;
    [SerializeField] private GameObject playerObjShow;

    [SerializeField] private GameObject stagePrefab;
    [SerializeField] private Transform stageTransform;

    [SerializeField] private GameObject loadingObject;
    private void Awake()
    {
        StartScene();
        StartCoroutine(DisplayBannerWithDelay());
    }
    private void Start()
    {
        startGameBtn.onClick.AddListener(StartGameSelectMap);
    }

    private void StartScene()
    {
        loadingObject.SetActive(true);
        StartCoroutine(InitializeScene());
    }

    private IEnumerator InitializeScene()
    {
        yield return StartCoroutine(LoginManager.Instance.GetUserData());

        loadingObject.SetActive(false);

        UpdateStage();
        UpdateCoinText();
        UpdateDiamondText();
    }

    private void OnDestroy()
    {
        startGameBtn.onClick.RemoveAllListeners();
    }

    IEnumerator DisplayBannerWithDelay()
    {
        yield return new WaitForSeconds(1f);
       // AdsManager.Instance.BannerAds.ShowBannerAd();

    }
    public void StartGameSelectMap()
    {
        mapImg.ResetPosition();

        playerObjShow.SetActive(false);
        SoundManager.Instance.ClickSound();
        panelMap.SetActive(true);
    }

    public void CloseSelectMap()
    {
        playerObjShow.SetActive(true);
        SoundManager.Instance.ClickSound();
        panelMap.SetActive(false);
    }

    public void SelectRegion(int number)
    {
        panelMap.SetActive(false);
        panelSelectStage.SetActive(true);

        DataCenter.Instance.LoadStagesForRegion(number, stagePrefab, stageTransform, loadingObject);
    }

    public void CloseStagePanel()
    {
        panelMap.SetActive(true);
        panelSelectStage.SetActive(false);

        foreach (Transform Child in stageTransform)
        {
            Destroy(Child.gameObject);
        }
    }

    private void UpdateStage()
    {
        stageText.text = $"Stage {DataCenter.Instance.GetPlayerData().region}-{DataCenter.Instance.GetPlayerData().stage}";
    }
    public void UpdateCoinText()
    {
        int coins = DataCenter.Instance.GetPlayerData().Coin;
        coinText.text = FormatCurrency(coins);
    }
    public void UpdateDiamondText()
    {
        int diamond = DataCenter.Instance.GetPlayerData().Diamond;
        diamondText.text = FormatCurrency(diamond);
    }

    private string FormatCurrency(int currency)
    {
        if (currency >= 1000000)
        {
            return (currency / 1000000.0).ToString("F1") + "M";
        }
        else if (currency >= 1000)
        {
            return (currency / 1000.0).ToString("F1") + "K";
        }
        else
        {
            return currency.ToString();
        }
    }


}
