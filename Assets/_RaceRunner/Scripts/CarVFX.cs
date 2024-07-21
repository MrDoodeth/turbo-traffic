using UnityEngine;

public class CarVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem borderVFX;
    [SerializeField] private ParticleSystem crashVFX;
    [SerializeField] private Transform rightBorderPoint;
    [SerializeField] private Transform leftBorderPoint;
    private Transform currentPoint;

    private void OnEnable()
    {
        RaceManager.OnPlayerFinished += Crash;
    }
    
    private void OnDisable()
    {
        RaceManager.OnPlayerFinished -= Crash;
    }

    private void Update()
    {
        borderVFX.gameObject.SetActive(CarController.isOnBorder);
        if (CarController.isOnBorder)
        {
            currentPoint = transform.position.x > 0f ? rightBorderPoint : leftBorderPoint;
            borderVFX.transform.position = currentPoint.position;
            borderVFX.Play();
        }
        else
        {
            borderVFX.Pause();
        }
    }

    private void Crash()
    {
        crashVFX.Play();
    }
}
