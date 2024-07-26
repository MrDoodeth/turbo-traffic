using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ShopPanel : MonoBehaviour
{
    
    [SerializeField] private TMP_Text rateMoneyLabel;
    [SerializeField] private int rateMoneyValue;
    [Space]
    
    [SerializeField] private TMP_Text adMoneyLabel;
    [SerializeField] private int adMoneyValue;
    [Space]
    
    [SerializeField] private TMP_Text buyYanMoneyLabel;
    [SerializeField] private TMP_Text buyYanLabel;
    [SerializeField] private ImageLoadYG currencyImageLoadYg;
    [SerializeField] private ImageLoadYG iconImageLoadYg;
    [SerializeField] private int buyYanMoneyValue;

    private void OnEnable()
    {
        rateMoneyLabel.text = rateMoneyValue.ToString("N0");
        adMoneyLabel.text = adMoneyValue.ToString("N0");
        buyYanMoneyLabel.text = buyYanMoneyValue.ToString("N0");
        
        YandexGame.ReviewSentEvent += OnLeftReview;
        YandexGame.RewardVideoEvent += OnShowAd;
        YandexGame.PurchaseSuccessEvent += OnSuccessPurchase;
    }

    public void ShowReview() => YandexGame.ReviewShow(true);
    public void ShowAd() => YandexGame.RewVideoShow((int)TypeOfAd.Money);
    public void GetPayment() => YandexGame.BuyPayments("money");
    private void OnDisable()
    {
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
            AudioManager.Instance.PlayMoneySound();
        }
    }
    
    private void OnShowAd(int index)
    {
        if (index == (int)TypeOfAd.Money)
        {
            YandexGame.savesData.moneyAmount += adMoneyValue;
            GarageManager.OnUIUpdated?.Invoke();
            YandexGame.SaveProgress();
            AudioManager.Instance.PlayMoneySound();
        }
    }
    private void OnSuccessPurchase(string id)
    {
        if (id == "money")
        {
            YandexGame.savesData.moneyAmount += buyYanMoneyValue;   
            GarageManager.OnUIUpdated?.Invoke();
            YandexGame.SaveProgress();
            AudioManager.Instance.PlayPurchaseSound();
        }
    }

    public void UpdatePurchase()
    {
        buyYanLabel.text = YandexGame.purchases[0].priceValue;
        currencyImageLoadYg.Load(YandexGame.purchases[0].currencyImageURL);
        iconImageLoadYg.Load(YandexGame.purchases[0].imageURI);
    }
}
