using UnityEngine;
using YG;

public class ContinuePanel : MonoBehaviour
{
    [SerializeField] private RaceManager raceManager;
    public void CloseAdOffer()
    {
        raceManager.ShowResult(0);
        this.gameObject.SetActive(false);
    }
    public void ShowAd() => YG2.RewardedAdvShow(TypeOfAd.continuePlay);
}
