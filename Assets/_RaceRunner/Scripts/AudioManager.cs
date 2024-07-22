using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")] 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource engineSource;
    [Space]
    
    [Header("Engine")]
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float changeStep;
    [Space]
    
    [Header("Clips")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip engineSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip moneySound;
    [SerializeField] private AudioClip purchaseSound;
    [SerializeField] private AudioClip buySound;
    public static AudioManager Instance;

    private void Start()
    {
        Instance = this;
    }

    private void Awake()
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();

        if (engineSource != null)
        {
            engineSource.clip = engineSound;
            engineSource.loop = true;
            engineSource.Play();
        }
    }
    private void Update()
    {
        if (engineSource != null)
        {
            if (!CarController.isControllable)
                engineSource.Pause();

            var targetPitch = 1f;
            var inputY = CarController.Instance.controller.Player.Move.ReadValue<Vector2>().y;
            
            if (inputY > 0f)
                targetPitch = maxPitch;
            else if (inputY < 0f)
                targetPitch = minPitch;
            else
                targetPitch = 1f;

            engineSource.pitch = Mathf.Lerp(engineSource.pitch, targetPitch, changeStep * Time.deltaTime);
        }
    }

    public void PlayClickSound() => soundSource.PlayOneShot(clickSound);
    public void PlayMoneySound() => soundSource.PlayOneShot(moneySound);
    public void PlayPurchaseSound() => soundSource.PlayOneShot(purchaseSound);
    public void PlayBuySound() => soundSource.PlayOneShot(buySound);
}