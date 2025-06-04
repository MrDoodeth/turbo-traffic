using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private CarsConfig carsConfig;
    [SerializeField] private GemSpawner gemSpawner;
    [Space] 
    
    [Header("Ratios")]
    [SerializeField] private float scoreSpeedRatio = 0.05f;
    [SerializeField] private float scoreOncomingRatio = 2f;
    [Space] 
    
    [Header("Money ratios")] 
    [SerializeField] private float distanceMoneyRatio;
    [SerializeField] private int overtakingMoneyRatio;
    [SerializeField] private float highSpeedMoneyRatio;
    [SerializeField] private float oncomingMoneyRatio;
    
    [Header("Panels")] 
    [SerializeField] private FinishPanel finishPanel;
    [SerializeField] private Transform mobileHUD;
    [SerializeField] private Transform continuePanel;
    [SerializeField] private SettingPanel settingPanel;

    public const float SCORE_START_SPEED = 26.3f;
    private const string LEADERBOARD_NAME = "Score";
    
    public static Action<int> OnOvertaking;
    public static Action OnPlayerFinished;

    public static bool isRespawned = false;
    public static float currentScore;
    public static int[] gemsAmount = new int[4];
    private static float timeAtHighSpeed;
    private static float timeAtOncomingLane;
    private static int overtakingCount;
    
    private int resultMoney;

    private void Awake()
    {
        QualitySettings.SetQualityLevel(YG2.saves.qualityIndex);
        //Player spawn
        CarController.Instance = Instantiate(carsConfig.cars[YG2.saves.currentCarIndex].car, Vector3.zero, Quaternion.identity);
        //Change color
        CarController.Instance.ChangeMaterial(carsConfig.cars[YG2.saves.currentCarIndex].materials[YG2.saves.currentColorIndex]);
    }
    
    private void Update()
    {
        //Score Calculating
        if (CarController.currentSpeed >= SCORE_START_SPEED)
        {
            timeAtHighSpeed += Time.deltaTime;
            var ratio = scoreSpeedRatio;
            if (CarController.isOncoming)
            {
                ratio *= scoreOncomingRatio;
                timeAtOncomingLane += Time.deltaTime;
            }
            currentScore += CarController.currentSpeed * ratio * Time.deltaTime;
        }
    }

    private void Start()
    {
        settingPanel.UpdateSound();
        settingPanel.UpdateMusic();
    }
    private void OnEnable()
    {
        OnOvertaking += OnScoreUpdate;
        OnPlayerFinished += Finish;
        YG2.onRewardAdv += OnX2Ad;
        YG2.onRewardAdv += OnContinueAd;
        
        if (!isRespawned)
        {
            currentScore = 0;
            gemsAmount = new int[4];
            timeAtHighSpeed = 0f;
            timeAtOncomingLane = 0f;
            overtakingCount = 0;
        }
    }
    private void OnDisable()
    {
        OnOvertaking -= OnScoreUpdate;
        OnPlayerFinished -= Finish;
        YG2.onRewardAdv -= OnX2Ad;
        YG2.onRewardAdv -= OnContinueAd;
    }
    
    private void OnScoreUpdate(int scoreAmount)
    {
        currentScore += scoreAmount;
        overtakingCount++;
        AudioManager.Instance.PlayOvertakingSound();
    }

    private static void TrySaveNewScore()
    {
        var score = Mathf.RoundToInt(currentScore);
        if (YG2.saves.highScore < score)
        {
            YG2.saves.highScore = score;
            YG2.SetLeaderboard(LEADERBOARD_NAME, score);
            YG2.SaveProgress();
        }
    }
    private void OnX2Ad(string index)
    {
        if (index == TypeOfAd.x2)
        {
            YG2.saves.moneyAmount += resultMoney;
            finishPanel.targetResultValueMoney = resultMoney * 2;
            YG2.SaveProgress();
        }
    }
    private void OnContinueAd(string index)
    {
        if (index == TypeOfAd.continuePlay)
        {
            isRespawned = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        }
    }
    
    private async void Finish()
    {
        AudioManager.Instance.PlayCrashSound(); 
        if (!isRespawned)
        {
            await UniTask.Delay(1000);
            continuePanel.gameObject.SetActive(true);
        }
        else
        {
            ShowResult(1000);
        }
    }

    public async void ShowResult(int delay)
    {
        var distanceMoney = Mathf.RoundToInt(CarController.Instance.GetDistance() * distanceMoneyRatio);
            var overtakingMoney = overtakingCount * overtakingMoneyRatio;
            var highSpeedMoney = Mathf.RoundToInt(timeAtHighSpeed * highSpeedMoneyRatio);
            var oncomingMoney = Mathf.RoundToInt(timeAtOncomingLane * oncomingMoneyRatio);
            int gemsMoney = 0;
            if (gemsAmount != null)
            {
                for (int i = 0; i < gemsAmount.Length; i++)
                {
                    gemsMoney += gemsAmount[i] * gemSpawner.gemsPrefabs[i].price;
                }
            }
            resultMoney = distanceMoney + overtakingMoney + highSpeedMoney + oncomingMoney + gemsMoney;
        
            YG2.saves.moneyAmount += resultMoney;
            TrySaveNewScore();
            YG2.SaveProgress();
        
            finishPanel.targetScoreValue = currentScore;
            finishPanel.targetDistanceValue = CarController.Instance.GetDistance();
            finishPanel.targetOvertakingValue = overtakingCount;
            finishPanel.targetHighSpeedValue = timeAtHighSpeed;
            finishPanel.targetOncomingValue = timeAtOncomingLane;
        
            if (gemsAmount != null)
            {
                finishPanel.targetGem1Value = gemsAmount[0];
                finishPanel.targetGem2Value = gemsAmount[1];
                finishPanel.targetGem3Value = gemsAmount[2];
                finishPanel.targetGem4Value = gemsAmount[3]; 
            }
        
            finishPanel.gem1Price = gemSpawner.gemsPrefabs[0].price;
            finishPanel.gem2Price = gemSpawner.gemsPrefabs[1].price;
            finishPanel.gem3Price = gemSpawner.gemsPrefabs[2].price;
            finishPanel.gem4Price = gemSpawner.gemsPrefabs[3].price;

            finishPanel.targetDistanceValueMoney = distanceMoney;
            finishPanel.targetOvertakingValueMoney = overtakingMoney;
            finishPanel.targetHighSpeedValueMoney = highSpeedMoney;
            finishPanel.targetOncomingValueMoney = oncomingMoney;
            finishPanel.targetGemsValueMoney = gemsMoney;
            finishPanel.targetResultValueMoney = resultMoney;
        
            await UniTask.Delay(delay);
            finishPanel.gameObject.SetActive(true);
    }
}