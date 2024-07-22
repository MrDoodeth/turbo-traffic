using UnityEngine;

public class CarVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem borderVFX;
    [SerializeField] private ParticleSystem crashVFX;
    [SerializeField] private Transform rightBorderPoint;
    [SerializeField] private Transform leftBorderPoint;
    private Transform currentPoint;
    private float elapsedTime;
    private const float SOUND_INTERVAL = 0.2f;

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
        if (!CarController.isControllable)
        {
            borderVFX.gameObject.SetActive(false);
            borderVFX.Pause();
            return;
        }

        elapsedTime += Time.deltaTime;
        
        borderVFX.gameObject.SetActive(CarController.isOnBorder);
        if (CarController.isOnBorder)
        {
            currentPoint = transform.position.x > 0f ? rightBorderPoint : leftBorderPoint;
            borderVFX.transform.position = currentPoint.position;
            borderVFX.Play();

            if (elapsedTime >= SOUND_INTERVAL)
            {
                AudioManager.Instance.PlayScreechSound();
                elapsedTime = 0;
            }
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
