using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ShopPanel : MonoBehaviour
{

    [SerializeField] private Button rateButton;
    [SerializeField] private TMP_Text rateMoneyLabel;
    [SerializeField] private int rateMoneyValue;
    [Space]
    
    [SerializeField] private Button adButton;
    [SerializeField] private TMP_Text adMoneyLabel;
    [SerializeField] private int adMoneyValue;
    [Space]
    
    [SerializeField] private Button buyYanButton;
    [SerializeField] private TMP_Text buyYanMoneyLabel;
    [SerializeField] private int buyYanMoneyValue;

    private void OnEnable()
    {
        rateMoneyLabel.text = rateMoneyValue.ToString("N0");
        adMoneyLabel.text = adMoneyValue.ToString("N0");
        buyYanMoneyLabel.text = buyYanMoneyValue.ToString("N0");
        
        rateButton.onClick.AddListener(() => YandexGame.ReviewShow(true));
        adButton.onClick.AddListener(() => YandexGame.RewVideoShow((int)TypeOfAd.Money));
        buyYanButton.onClick.AddListener(() => YandexGame.BuyPayments("money"));
        YandexGame.ReviewSentEvent += OnLeftReview;
        YandexGame.RewardVideoEvent += OnShowAd;
        YandexGame.PurchaseSuccessEvent += OnSuccessPurchase;
    }

    private void OnDisable()
    {
        rateButton.onClick.RemoveListener(() => YandexGame.ReviewShow(true));
        adButton.onClick.RemoveListener(() => YandexGame.RewVideoShow((int)TypeOfAd.Money));
        buyYanButton.onClick.RemoveListener(() => YandexGame.BuyPayments("money"));
        YandexGame.ReviewSentEvent -= OnLeftReview;
        YandexGame.RewardVideoEvent -= OnShowAd;
        YandexGame.PurchaseSuccessEvent -= OnSuccessPurchase;
    }

    private void OnLeftReview(bool action)
    {
        if (action)
        {
            YandexGame.savesData.moneyAmount += rateMoneyValue;
            GarageManager.OnUIUpdated?.Invoke();
            YandexGame.SaveProgress();
        }
    }
    
    private void OnShowAd(int index)
    {
        if (index == (int)TypeOfAd.Money)
        {
            YandexGame.savesData.moneyAmount += adMoneyValue;
            GarageManager.OnUIUpdated?.Invoke();
            YandexGame.SaveProgress();
        }
    }
    private void OnSuccessPurchase(string id)
    {
        if (id == "money")
        {
            YandexGame.savesData.moneyAmount += buyYanMoneyValue;   
            GarageManager.OnUIUpdated?.Invoke();
            YandexGame.SaveProgress();
        }
    }
}
