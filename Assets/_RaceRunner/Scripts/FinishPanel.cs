using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class FinishPanel : MonoBehaviour
{
    [SerializeField] private Button garageButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button X2AdButton;
    [Space]
    
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text resultLabel;
    [Space]
    
    [SerializeField] private TMP_Text distanceLabel;
    [SerializeField] private TMP_Text overtakingLabel;
    [SerializeField] private TMP_Text highSpeedLabel;
    [SerializeField] private TMP_Text oncomingLabel;
    [Space]
    
    [SerializeField] private TMP_Text gem1Label;
    [SerializeField] private TMP_Text gem2Label;
    [SerializeField] private TMP_Text gem3Label;
    [SerializeField] private TMP_Text gem4Label;
    [Space]
    
    [SerializeField] private TMP_Text gem1PriceLabel;
    [SerializeField] private TMP_Text gem2PriceLabel;
    [SerializeField] private TMP_Text gem3PriceLabel;
    [SerializeField] private TMP_Text gem4PriceLabel;
    [Space]
    
    [SerializeField] private TMP_Text distanceMoneyLabel;
    [SerializeField] private TMP_Text overtakingMoneyLabel;
    [SerializeField] private TMP_Text highSpeedMoneyLabel;
    [SerializeField] private TMP_Text oncomingMoneyLabel;
    [SerializeField] private TMP_Text gemsMoneyLabel;
    [Space]
    
    [SerializeField] private float duration;
    [SerializeField] private float delay = 0.5f;
    
    //Values
    [HideInInspector] public float targetScoreValue;
    [HideInInspector] public float targetDistanceValue;
    [HideInInspector] public float targetOvertakingValue;
    [HideInInspector] public float targetHighSpeedValue;
    [HideInInspector] public float targetOncomingValue;
    private float scoreValue;
    private float distanceValue;
    private float overtakingValue;
    private float highSpeedValue;
    private float oncomingValue;
    
    //Money
    [HideInInspector] public float targetDistanceValueMoney;
    [HideInInspector] public float targetOvertakingValueMoney;
    [HideInInspector] public float targetHighSpeedValueMoney;
    [HideInInspector] public float targetOncomingValueMoney;
    [HideInInspector] public float targetGemsValueMoney;
    [HideInInspector] public float targetResultValueMoney;
    private float distanceValueMoney;
    private float overtakingValueMoney;
    private float highSpeedValueMoney;
    private float oncomingValueMoney;
    private float gemsValueMoney;
    private float resultValueMoney;
    
    //Gems
    [HideInInspector] public float targetGem1Value;
    [HideInInspector] public float targetGem2Value;
    [HideInInspector] public float targetGem3Value;
    [HideInInspector] public float targetGem4Value;
    
    [HideInInspector] public int gem1Price;
    [HideInInspector] public int gem2Price;
    [HideInInspector] public int gem3Price;
    [HideInInspector] public int gem4Price;
    private float gem1Value;
    private float gem2Value;
    private float gem3Value;
    private float gem4Value;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnX2Ad;
        
        garageButton.onClick.AddListener(() => SceneManager.LoadScene(1));
        replayButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        X2AdButton.onClick.AddListener(() => YandexGame.RewVideoShow((int)TypeOfAd.X2));
        
        gem1PriceLabel.text = gem1Price.ToString();
        gem2PriceLabel.text = gem2Price.ToString();
        gem3PriceLabel.text = gem3Price.ToString();
        gem4PriceLabel.text = gem4Price.ToString();
        
        DOTween.Sequence()
            .SetUpdate(UpdateType.Normal, true)
            .AppendInterval(delay)
            
            .Append(DOTween.To(() => scoreValue, x => scoreValue = x, targetScoreValue, duration))
            
            .Append(DOTween.To(() => distanceValue, x => distanceValue = x, targetDistanceValue, duration))
            .Insert(duration , DOTween.To(() => distanceValueMoney, x => distanceValueMoney = x, targetDistanceValueMoney, duration))
            
            .Append(DOTween.To(() => overtakingValue, x => overtakingValue = x, targetOvertakingValue, duration))
            .Insert(duration * 2 , DOTween.To(() => overtakingValueMoney, x => overtakingValueMoney = x, targetOvertakingValueMoney, duration))
            
            .Append(DOTween.To(() => highSpeedValue, x => highSpeedValue = x, targetHighSpeedValue, duration))
            .Insert(duration * 3 , DOTween.To(() => highSpeedValueMoney, x => highSpeedValueMoney = x, targetHighSpeedValueMoney, duration))
            
            .Append(DOTween.To(() => oncomingValue, x => oncomingValue = x, targetOncomingValue, duration))
            .Insert(duration * 4 , DOTween.To(() => oncomingValueMoney, x => oncomingValueMoney = x, targetOncomingValueMoney, duration))
            
            .Insert(duration * 5 , DOTween.To(() => gem1Value, x => gem1Value = x, targetGem1Value, duration))
            .Insert(duration * 5 , DOTween.To(() => gem2Value, x => gem2Value = x, targetGem2Value, duration))
            .Insert(duration * 5 , DOTween.To(() => gem3Value, x => gem3Value = x, targetGem3Value, duration))
            .Insert(duration * 5 , DOTween.To(() => gem4Value, x => gem4Value = x, targetGem4Value, duration))
            .Insert(duration * 5 , DOTween.To(() => gemsValueMoney, x => gemsValueMoney = x, targetGemsValueMoney, duration))
            
            .Append(DOTween.To(() => resultValueMoney, x => resultValueMoney = x, targetResultValueMoney, duration))
            .Append(X2AdButton.transform.DOScale(Vector3.one, duration))
            .Insert(duration * 8 , garageButton.transform.DOScale(Vector3.one, duration))
            .Insert(duration * 8 , replayButton.transform.DOScale(Vector3.one, duration));
    }

    private void OnX2Ad(int index)
    {
        if (index == (int)TypeOfAd.X2)
        {
            X2AdButton.interactable = false;
            DOTween.Sequence()
                .Append(DOTween.To(() => resultValueMoney, x => resultValueMoney = x, targetResultValueMoney, duration))
                .Insert(0, X2AdButton.transform.DOScale(Vector3.zero, duration));
        }
    }

    private void OnDisable()
    {
        garageButton.onClick.RemoveListener(() => SceneManager.LoadScene(1));
        replayButton.onClick.RemoveListener(() => SceneManager.LoadScene(0));
        X2AdButton.onClick.RemoveListener(() => YandexGame.RewVideoShow((int)TypeOfAd.X2));
        
        YandexGame.RewardVideoEvent -= OnX2Ad;
    }

    private void Update()
    {
        scoreLabel.text = Mathf.RoundToInt(scoreValue).ToString();
        
        distanceLabel.text = distanceValue.ToString("F");
        overtakingLabel.text = Mathf.RoundToInt(overtakingValue).ToString();
        highSpeedLabel.text = highSpeedValue.ToString("F");
        oncomingLabel.text = oncomingValue.ToString("F");
        
        gem1Label.text = Mathf.RoundToInt(gem1Value).ToString();
        gem2Label.text = Mathf.RoundToInt(gem2Value).ToString();
        gem3Label.text = Mathf.RoundToInt(gem3Value).ToString();
        gem4Label.text = Mathf.RoundToInt(gem4Value).ToString();
        
        distanceMoneyLabel.text = Mathf.RoundToInt(distanceValueMoney).ToString();
        overtakingMoneyLabel.text = Mathf.RoundToInt(overtakingValueMoney).ToString();
        highSpeedMoneyLabel.text = Mathf.RoundToInt(highSpeedValueMoney).ToString();
        oncomingMoneyLabel.text = Mathf.RoundToInt(oncomingValueMoney).ToString();
        gemsMoneyLabel.text = Mathf.RoundToInt(gemsValueMoney).ToString();
        
        resultLabel.text = Mathf.RoundToInt(resultValueMoney).ToString();
    }
}
