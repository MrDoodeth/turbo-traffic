using UnityEngine;
using UnityEngine.UI;
using YG;

public class LanguageChecker : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    private string[] locals = { "en", "ru", "tr" };
    private int currentIndex = 0;
    private void ChangeCurrentLocalization()
    {
        if (currentIndex >= locals.Length - 1)
            currentIndex = 0;
        else
            currentIndex++;
    }
    private void OnEnable()
    {
        OnSwitchLanguage(YandexGame.savesData.language);

        YandexGame.SwitchLangEvent += OnSwitchLanguage;
    }

    private void OnDisable()
    {
        YandexGame.SwitchLangEvent -= OnSwitchLanguage;
    }

    private void OnSwitchLanguage(string local)
    {
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        
        switch (local)
        {
            case "en":
                buttons[0].gameObject.SetActive(true);
                break;
            case "ru":
                buttons[1].gameObject.SetActive(true);
                break;
            case "tr":
                buttons[2].gameObject.SetActive(true);
                break;
        }
    }

    public void SwitchLanguage()
    {
        ChangeCurrentLocalization();
        YandexGame.SwitchLanguage(locals[currentIndex]);
        YandexGame.savesData.language = locals[currentIndex];
        YandexGame.SaveProgress();
        OnSwitchLanguage(YandexGame.savesData.language);
    }
}