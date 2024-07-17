using System;
using UnityEngine;
using TMPro;

public class GameUIInfo : MonoBehaviour
{
    [Header("Ratios")]
    [SerializeField] private float distanceRatio = 1.2f;
    
    [Header("Labels")] 
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text scoreVFX;
    [SerializeField] private TMP_Text distanceLabel;
    [SerializeField] private TMP_Text speedLabel;
    public static Action OnDistanceUpdated;
    public static Action OnSpeedUpdated;
    private void OnEnable()
    {
        RaceManager.OnOvertaking += OnScoreVFXUpdate;
        OnDistanceUpdated += OnDistanceUpdate;
        OnSpeedUpdated += OnSpeedUpdate;
    }
    private void OnDisable()
    {
        RaceManager.OnOvertaking -= OnScoreVFXUpdate;
        OnDistanceUpdated -= OnDistanceUpdate;
        OnSpeedUpdated -= OnSpeedUpdate;
    }
    private void OnScoreVFXUpdate(int scoreAmount)
    {
        scoreVFX.text = $"+{scoreAmount}";
        var animations = scoreVFX.GetComponent<Animation>();
        animations.Stop();
        animations.Play();
    }
    private void OnDistanceUpdate()
    {
        distanceLabel.text = (CarController.Instance.transform.position.z * distanceRatio * 0.001).ToString("F");
    }
    private void OnSpeedUpdate()
    {
        speedLabel.text = CarController.Instance.GetSpeed().ToString("N0");
    }
    private void Update()
    {
        scoreLabel.text = RaceManager.currentScore.ToString("#");
    }
}
