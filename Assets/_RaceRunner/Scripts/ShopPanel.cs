using TMPro;
using UnityEngine;
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

        YG2.onReviewSent += OnLeftReview;
        YG2.onRewardAdv += OnShowAd;
        YG2.onPurchaseSuccess += OnSuccessPurchase;
    }

    public void ShowReview() => YG2.ReviewShow();
    public void ShowAd() => YG2.RewardedAdvShow(TypeOfAd.money);
    public void GetPayment() => YG2.BuyPayments(TypeOfPurchase.money);
    private void OnDisable()
    {
        YG2.onReviewSent -= OnLeftReview;
        YG2.onRewardAdv -= OnShowAd;
        YG2.onPurchaseSuccess -= OnSuccessPurchase;
    }

    private void OnLeftReview(bool action)
    {
        if (action)
        {
            YG2.saves.moneyAmount += rateMoneyValue;
            GarageManager.OnUIUpdated?.Invoke();
            YG2.SaveProgress();
            AudioManager.Instance.PlayMoneySound();
        }
    }
    
    private void OnShowAd(string index)
    {
        if (index == TypeOfAd.money)
        {
            YG2.saves.moneyAmount += adMoneyValue;
            GarageManager.OnUIUpdated?.Invoke();
            YG2.SaveProgress();
            AudioManager.Instance.PlayMoneySound();
        }
    }
    private void OnSuccessPurchase(string id)
    {
        if (id == TypeOfPurchase.money)
        {
            YG2.saves.moneyAmount += buyYanMoneyValue; 
            GarageManager.OnUIUpdated?.Invoke();
            YG2.SaveProgress();
            AudioManager.Instance.PlayPurchaseSound();
        }
    }

    public void UpdatePurchase()
    {
        buyYanLabel.text = YG2.purchases[0].priceValue;
        currencyImageLoadYg.Load(YG2.purchases[0].currencyImageURL);
        iconImageLoadYg.Load(YG2.purchases[0].imageURI);
    }
}