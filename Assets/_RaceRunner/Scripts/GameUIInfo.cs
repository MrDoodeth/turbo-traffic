using System;
using UnityEngine;
using TMPro;

public class GameUIInfo : MonoBehaviour
{
    [Header("Labels")] 
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text scoreVFX;
    [SerializeField] private TMP_Text distanceLabel;
    [SerializeField] private TMP_Text speedLabel;
    [Space] 
    
    [SerializeField] private TMP_Text gem1Label;
    [SerializeField] private TMP_Text gem2Label;
    [SerializeField] private TMP_Text gem3Label;
    [SerializeField] private TMP_Text gem4Label;
    public static Action OnDistanceUpdated;
    public static Action OnSpeedUpdated;
    private void OnEnable()
    {
        RaceManager.OnOvertaking += OnScoreVFXUpdate;
        GemSpawner.OnGemPickedUp += UpdateGemsUI;
        OnDistanceUpdated += OnDistanceUpdate;
        OnSpeedUpdated += OnSpeedUpdate;
    }
    private void OnDisable()
    {
        RaceManager.OnOvertaking -= OnScoreVFXUpdate;
        GemSpawner.OnGemPickedUp -= UpdateGemsUI;
        OnDistanceUpdated -= OnDistanceUpdate;
        OnSpeedUpdated -= OnSpeedUpdate;
    }
    private void UpdateGemsUI(int[] gemsAmount)
    {
        gem1Label.text = gemsAmount[0].ToString();
        gem2Label.text = gemsAmount[1].ToString();
        gem3Label.text = gemsAmount[2].ToString();
        gem4Label.text = gemsAmount[3].ToString();
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
        distanceLabel.text = CarController.Instance.GetDistance().ToString("F");
    }
    private void OnSpeedUpdate()
    {
        speedLabel.text = CarController.Instance.GetSpeed().ToString();
    }
    private void Update()
    {
        if (!CarController.Instance.isControllable)
            return;
        
        scoreLabel.text = ((int)(RaceManager.currentScore)).ToString();
    }
}
