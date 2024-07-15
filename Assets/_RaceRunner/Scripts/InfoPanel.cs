using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [Header("Acceleration")] 
    [SerializeField] private Image accelerationFill;
    [SerializeField] private TMP_Text accelerationLabel;
    [Space]
    
    [Header("Controllability")] 
    [SerializeField] private Image controllabilityFill;
    [SerializeField] private TMP_Text controllabilityLabel;
    [Space]
    
    [Header("Braking")] 
    [SerializeField] private Image brakingFill;
    [SerializeField] private TMP_Text brakingLabel;
    [Space] 
    
    [SerializeField] private float fillSpeed;
    [SerializeField] private float ratioFill;

    [HideInInspector] public float targetAcceleration;
    [HideInInspector] public float targetControllability;
    [HideInInspector] public float targetBraking;

    private void Update()
    {
        accelerationFill.fillAmount = Mathf.Lerp(accelerationFill.fillAmount, targetAcceleration,
            Time.deltaTime * fillSpeed);
        controllabilityFill.fillAmount = Mathf.Lerp(controllabilityFill.fillAmount, targetControllability,
            Time.deltaTime * fillSpeed);
        brakingFill.fillAmount = Mathf.Lerp(brakingFill.fillAmount, targetBraking,
            Time.deltaTime * fillSpeed);
    }

    public void SetInfo(float acceleration, float controllability, float braking)
    {
        accelerationLabel.text = acceleration.ToString("N0");
        controllabilityLabel.text = controllability.ToString("N0");
        brakingLabel.text = (braking/2).ToString("N0");

        targetAcceleration = acceleration * ratioFill;
        targetControllability = controllability * ratioFill;
        targetBraking = (braking/2) * ratioFill;
    }
}
