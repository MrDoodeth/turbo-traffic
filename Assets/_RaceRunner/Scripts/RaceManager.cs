using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private CarsConfig carsConfig;
    [Space] 
    
    [Header("Ratios")]
    [SerializeField] private float scoreSpeedRatio = 0.05f;
    [SerializeField] private float scoreOncomingRatio = 2f;
    
    [Header("Panels")] 
    [SerializeField] private Image pausePanel;
    
    [Header("Buttons")] 
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    public const float SCORE_START_SPEED = 26.3f;
    public static Action<int> OnOvertaking;
    
    public static float currentScore;
    private float timeAtHighSpeed;
    private float timeAtOncomingLane;
    private int overtakingCount;
    public static int[] gemsAmount;

    private void Awake()
    {
        CarController.Instance = Instantiate(carsConfig.cars[YandexGame.savesData.currentCarIndex].car, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        //Score Calculating
        if (CarController.Instance.currentSpeed >= SCORE_START_SPEED)
        {
            timeAtHighSpeed += Time.deltaTime;
            var ratio = scoreSpeedRatio;
            if (RoadSpawner.isOncoming)
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
        pauseButton.onClick.AddListener(OnPausePress);
        resumeButton.onClick.AddListener(OnResumePress);
    }
    private void OnDisable()
    {
        OnOvertaking -= OnScoreUpdate;
        pauseButton.onClick.RemoveListener(OnPausePress);
        resumeButton.onClick.RemoveListener(OnResumePress);
    }
    
    private void OnScoreUpdate(int scoreAmount)
    {
        currentScore += scoreAmount;
        overtakingCount++;
    }
    private void OnPausePress()
    {
        Time.timeScale = 0f;
        pausePanel.gameObject.SetActive(true);
    }

    private void OnResumePress()
    {
        pausePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}