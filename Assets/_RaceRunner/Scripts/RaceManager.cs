using System;
using TMPro;
using UnityEngine;
using YG;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private CarsConfig carsConfig;
    [Space] 
    
    [SerializeField] private float distanceRatio = 0.45f;
    [SerializeField] private float speedRatio = 1f;
    [Space]
    
    [Header("Labels")] 
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text distanceLabel;
    [SerializeField] private TMP_Text speedLabel;
    public static Action<int> OnScoreUpdated;
    public static Action OnDistanceUpdated;
    public static Action OnSpeedUpdated;
    private int currentScore;

    private void Awake()
    {
        CarController.Instance = Instantiate(carsConfig.cars[YandexGame.savesData.currentCarIndex].car, Vector3.zero, Quaternion.identity);
    }

    private void OnEnable()
    {
        OnScoreUpdated += OnScoreUpdate;
        OnDistanceUpdated += OnDistanceUpdate;
        OnSpeedUpdated += OnSpeedUpdate;
    }
    private void OnDisable()
    {
        OnScoreUpdated -= OnScoreUpdate;
        OnDistanceUpdated -= OnDistanceUpdate;
        OnSpeedUpdated -= OnSpeedUpdate;
    }
    private void OnScoreUpdate(int scoreAmount)
    {
        currentScore += scoreAmount;
        scoreLabel.text = currentScore.ToString();
    }
    private void OnDistanceUpdate()
    {
        distanceLabel.text = (CarController.Instance.transform.position.z * distanceRatio * 0.001).ToString("F");
    }
    private void OnSpeedUpdate()
    {
        speedLabel.text = (CarController.Instance.currentSpeed * speedRatio).ToString("N0");
    }
}
