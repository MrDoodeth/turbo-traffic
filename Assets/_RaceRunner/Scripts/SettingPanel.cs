using UnityEngine;
using UnityEngine.UI;
using YG;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private ToggleGroup qualityToggleGroup;
    [SerializeField] private ToggleGroup languageToggleGroup;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    private Toggle[] qualityToggles;
    private Toggle[] languageToggles;

    private void Awake()
    {
        qualityToggles = qualityToggleGroup.GetComponentsInChildren<Toggle>();
        languageToggles = languageToggleGroup.GetComponentsInChildren<Toggle>();
    }

    private void OnEnable()
    {
        qualityToggles[YandexGame.savesData.qualityIndex].isOn = true;
        
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
        QualitySettings.SetQualityLevel(index);
        YandexGame.savesData.qualityIndex = index;
        YandexGame.SaveProgress();
        AudioManager.Instance.PlayClickSound();
    }

    public void SetLanguage(string local)
    {
        YandexGame.SwitchLanguage(local);
        YandexGame.SaveProgress();
        AudioManager.Instance.PlayClickSound();
    }
}
