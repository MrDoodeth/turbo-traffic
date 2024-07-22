using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
    [SerializeField] private SettingPanel settingPanel;
    [Space]
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button buyButton;
    [Space]
    
    [Header("Labels")]
    [SerializeField] private TMP_Text priceLabel;
    [SerializeField] private TMP_Text moneyLabel;
    [SerializeField] private TMP_Text highScoreLabel;
    private int selectedCar;
    private readonly List<CarController> cars = new List<CarController>();
    public static Action OnUIUpdated;

    private void Awake()
    {
        QualitySettings.SetQualityLevel(YandexGame.savesData.qualityIndex);
    }

    private void OnUIUpdate()
    {
        moneyLabel.text = YandexGame.savesData.moneyAmount.ToString();
        highScoreLabel.text = YandexGame.savesData.highScore.ToString();
    }

    private void Start()
    {
        settingPanel.UpdateSound();
        settingPanel.UpdateMusic();
    }

    private void OnEnable()
    {
        settingPanel.UpdateSound();
        settingPanel.UpdateMusic();
        
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
        
        selectedCar = YandexGame.savesData.currentCarIndex;
        
        OnUIUpdated?.Invoke();
        UpdateInfo();
    }

    private void OnDisable()
    {
        OnUIUpdated -= OnUIUpdate;
    }

    public void Buy()
    {
        lockImage.gameObject.SetActive(false);
        playButton.interactable = true;
        YandexGame.savesData.isFreeCars[selectedCar] = true;
        YandexGame.savesData.currentCarIndex = selectedCar;
        YandexGame.savesData.moneyAmount -= carsConfig.cars[selectedCar].price;
        AudioManager.Instance.PlayBuySound();
        OnUIUpdated?.Invoke();
    }
    public void SwipeLeft()
    {
        if (selectedCar <= 0)
            selectedCar = cars.Count - 1;
        else
            selectedCar--;
        Select(selectedCar);
    }
    public void SwipeRight()
    {
        if (selectedCar >= cars.Count - 1)
            selectedCar = 0;
        else
            selectedCar++;
        Select(selectedCar);
    }
    public void ShowURL() => Application.OpenURL("https://yandex.ru/games/developer/75165");

    public void OnStartRace()
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
        buyButton.interactable = YandexGame.savesData.moneyAmount >= carsConfig.cars[selectedCar].price;
        
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