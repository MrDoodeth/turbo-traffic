using UnityEngine;
using UnityEngine.UI;
using YG;

public class ContinuePanel : MonoBehaviour
{
    [SerializeField] private RaceManager raceManager;
    [Space]
    
    [SerializeField] private Button adButton;
    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseAdOffer);
        adButton.onClick.AddListener(() => YandexGame.RewVideoShow((int)TypeOfAd.Continue));
    }
    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(CloseAdOffer);
        adButton.onClick.RemoveListener(() => YandexGame.RewVideoShow((int)TypeOfAd.Continue));
    }
    private void CloseAdOffer()
    {
        raceManager.ShowResult(0);
        this.gameObject.SetActive(false);
    }
}
