using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class GarageManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private CarsConfig carsConfig;
    [Space]
    
    [Header("Modules")]
    [SerializeField] private Transform playerCars;
    [SerializeField] private InfoPanel infoPanel;
    [SerializeField] private Image lockImage;
    [Space]
    
    [Header("Buttons")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button linkButton;
    [SerializeField] private Button purchaseButton;
    [Space]
    
    [Header("Labels")]
    [SerializeField] private TMP_Text priceLabel;
    [SerializeField] private TMP_Text moneyLabel;
    [SerializeField] private TMP_Text highScoreLabel;
    private int selectedCar;
    private readonly List<CarController> cars = new List<CarController>();
    public static event Action OnUIUpdated;

    private void Awake()
    {
        QualitySettings.SetQualityLevel(YandexGame.savesData.qualityIndex);
    }

    private void OnUIUpdate()
    {
        moneyLabel.text = YandexGame.savesData.moneyAmount.ToString();
        highScoreLabel.text = YandexGame.savesData.highScore.ToString();
    }

    private void OnEnable()
    {
        OnUIUpdated += OnUIUpdate;
            
        foreach (var carPrefab in carsConfig.cars)
        {
            var car = Instantiate(carPrefab.car, playerCars);
            Destroy(car.transform.GetChild(2).gameObject);
            car.enabled = false;
            car.transform.localPosition = Vector3.zero;
            car.gameObject.SetActive(false);
            cars.Add(car);
        }
        
        lockImage.gameObject.SetActive(false);
        
        cars[YandexGame.savesData.currentCarIndex].gameObject.SetActive(true);
        
        leftButton.onClick.AddListener(SwipeLeft);
        rightButton.onClick.AddListener(SwipeRight);
        playButton.onClick.AddListener(OnStartRace);
        linkButton.onClick.AddListener(() => Application.OpenURL("https://yandex.ru/games/developer/75165"));
        purchaseButton.onClick.AddListener(Buy);
        selectedCar = YandexGame.savesData.currentCarIndex;
        
        OnUIUpdated?.Invoke();
        UpdateInfo();
    }

    private void OnDisable()
    {
        OnUIUpdated -= OnUIUpdate;
        
        leftButton.onClick.RemoveListener(SwipeLeft);
        rightButton.onClick.RemoveListener(SwipeRight);
        playButton.onClick.RemoveListener(OnStartRace);
        linkButton.onClick.RemoveListener(() => Application.OpenURL("https://yandex.ru/games/developer/75165"));
        purchaseButton.onClick.RemoveListener(Buy);
    }

    private void Buy()
    {
        lockImage.gameObject.SetActive(false);
        playButton.interactable = true;
        YandexGame.savesData.isFreeCars[selectedCar] = true;
        YandexGame.savesData.currentCarIndex = selectedCar;
        YandexGame.savesData.moneyAmount -= carsConfig.cars[selectedCar].price;
        OnUIUpdated?.Invoke();
    }
    private void SwipeLeft()
    {
        if (selectedCar <= 0)
            selectedCar = cars.Count - 1;
        else
            selectedCar--;
        Select(selectedCar);
    }
    private void SwipeRight()
    {
        if (selectedCar >= cars.Count - 1)
            selectedCar = 0;
        else
            selectedCar++;
        Select(selectedCar);
    }

    private void OnStartRace()
    {
        YandexGame.savesData.currentCarIndex = selectedCar;
        SceneManager.LoadScene(0);
    }
    private void Select(int index)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].gameObject.SetActive(i == index);
        }

        //Lock
        bool isFreeCar = YandexGame.savesData.isFreeCars[selectedCar];
        lockImage.gameObject.SetActive(!isFreeCar);
        priceLabel.text = carsConfig.cars[selectedCar].price.ToString();
        playButton.interactable = isFreeCar;
        purchaseButton.interactable = YandexGame.savesData.moneyAmount >= carsConfig.cars[selectedCar].price;
        
        //InfoPanel
        UpdateInfo();
    }
    private void UpdateInfo()
    {
        var acceleration = carsConfig.cars[selectedCar].car.GetAcceleration();
        var controllability = carsConfig.cars[selectedCar].car.GetSensitivity();
        var braking = carsConfig.cars[selectedCar].car.GetBraking();
        infoPanel.SetInfo(acceleration, controllability, braking);
    }
}