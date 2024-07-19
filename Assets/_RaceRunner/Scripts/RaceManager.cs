using System;
using System.Threading.Tasks;
using UnityEngine;
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

    public const float SCORE_START_SPEED = 26.3f;
    private const string LEADERBOARD_NAME = "Score";
    public static Action<int> OnOvertaking;
    public static Action OnPlayerFinished;
    
    public static float currentScore;
    private float timeAtHighSpeed;
    private float timeAtOncomingLane;
    private int overtakingCount;
    public static int[] gemsAmount = new int[4];
    private int resultMoney;

    private void Awake()
    {
        CarController.Instance = Instantiate(carsConfig.cars[YandexGame.savesData.currentCarIndex].car, Vector3.zero, Quaternion.identity);
        mobileHUD.gameObject.SetActive(YandexGame.EnvironmentData.isMobile);
    }

    private void Start()
    {
        gemsAmount = new int[4];
        currentScore = 0;
    }

    private void Update()
    {
        if (!CarController.Instance.isControllable)
            return;
        
        //Score Calculating
        if (CarController.Instance.currentSpeed >= SCORE_START_SPEED)
        {
            timeAtHighSpeed += Time.deltaTime;
            var ratio = scoreSpeedRatio;
            if (CarController.isOncoming)
            {
                ratio *= scoreOncomingRatio;
                timeAtOncomingLane += Time.deltaTime;
            }
            currentScore += CarController.Instance.currentSpeed * ratio * Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        OnOvertaking += OnScoreUpdate;
        OnPlayerFinished += Finish;
    }
    private void OnDisable()
    {
        OnOvertaking -= OnScoreUpdate;
        OnPlayerFinished -= Finish;
    }
    
    private void OnScoreUpdate(int scoreAmount)
    {
        currentScore += scoreAmount;
        overtakingCount++;
    }

    private static void TrySaveNewScore()
    {
        var score = Mathf.RoundToInt(currentScore);
        if (YandexGame.savesData.highScore < score)
        {
            YandexGame.savesData.highScore = score;
            YandexGame.NewLeaderboardScores(LEADERBOARD_NAME, score);
            YandexGame.SaveProgress();
        }
            
    }

    private async void Finish()
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
        
        YandexGame.savesData.moneyAmount += resultMoney;
        TrySaveNewScore();
        YandexGame.SaveProgress();
        
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
        
        await Task.Delay(1000);
        finishPanel.gameObject.SetActive(true);
    }
}