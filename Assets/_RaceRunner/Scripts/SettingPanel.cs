using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private ToggleGroup qualityToggleGroup;
    [SerializeField] private ToggleGroup languageToggleGroup;
    [Space] 
    
    [SerializeField] private AudioMixer audioMixer;
    [Space] 
    
    [SerializeField] private Button soundButton;
    [SerializeField] private Sprite onSound;
    [SerializeField] private Sprite offSound;
    [Space] 
    
    [SerializeField] private Button musicButton;
    [SerializeField] private Sprite onMusic;
    [SerializeField] private Sprite offMusic;
    private Toggle[] qualityToggles;
    private Toggle[] languageToggles;
    private Image soundImage;
    private Image musicImage;

    private void Awake()
    {
        qualityToggles = qualityToggleGroup.GetComponentsInChildren<Toggle>();
        languageToggles = languageToggleGroup.GetComponentsInChildren<Toggle>();
    }

    private void OnEnable()
    {
        qualityToggles[YandexGame.savesData.qualityIndex].isOn = true;
        
        soundImage = soundButton.GetComponent<Image>();
        musicImage = musicButton.GetComponent<Image>();
        
        soundImage.sprite = YandexGame.savesData.isSoundActive ? onSound : offSound;
        musicImage.sprite = YandexGame.savesData.isMusicActive ? onMusic : offMusic;

        UpdateLanguage();
    }
    
    private void UpdateLanguage()
    {
        switch (YandexGame.savesData.language)
        {
            case "ru":
                languageToggles[0].isOn = true;
                break;
            case "en":
                languageToggles[1].isOn = true;
                break;
            case "tr":
                languageToggles[2].isOn = true;
                break;
        }
    }
    public void SetQuality(int index)
    {
        if (qualityToggles[index].isOn)
        {
            QualitySettings.SetQualityLevel(index);
            YandexGame.savesData.qualityIndex = index;
            YandexGame.SaveProgress();
            AudioManager.Instance.PlayClickSound();
        }
    }

    public void SetLanguage(string local)
    {
        var index = 0;
        switch (local)
        {
            case "ru":
                index = 0;
                break;
            case "en":
                index = 1;
                break;
            case "tr":
                index = 2;
                break;
        }

        if (languageToggles[index].isOn)
        {
            YandexGame.SwitchLanguage(local);
            YandexGame.SaveProgress();
            AudioManager.Instance.PlayClickSound();
        }
    }

    public void UpdateSound()
    {
        soundImage = soundButton.GetComponent<Image>();
        
        if (YandexGame.savesData.isSoundActive)
        {
            soundImage.sprite = onSound;
            audioMixer.SetFloat("soundVolume", 0f);
        }
        else
        {
            soundImage.sprite = offSound;
            audioMixer.SetFloat("soundVolume", -80f);
        }
    }

    public void UpdateMusic()
    {
        musicImage = musicButton.GetComponent<Image>();
        
        if (YandexGame.savesData.isMusicActive)
        {
            musicImage.sprite = onMusic;
            audioMixer.SetFloat("musicVolume", 0f);
        }
        else
        {
            musicImage.sprite = offMusic;
            audioMixer.SetFloat("musicVolume", -80f);
        }
    }

    private void Update()
    {
        UpdateLanguage();
    }

    public void SwitchSound()
    {
        YandexGame.savesData.isSoundActive = !YandexGame.savesData.isSoundActive;
        YandexGame.SaveProgress();
        UpdateSound();
    }
    
    public void SwitchMusic()
    {
        YandexGame.savesData.isMusicActive = !YandexGame.savesData.isMusicActive;
        YandexGame.SaveProgress();
        UpdateMusic();
    }
}