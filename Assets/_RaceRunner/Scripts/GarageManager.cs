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
    [SerializeField] private ToggleGroup colorToggleGroup;
    [SerializeField] private Transform colorPanel;
    [Space]
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button paintButton;
    [FormerlySerializedAs("priceLabel")]
    [Space]
    
    [Header("Labels")]
    [SerializeField] private TMP_Text priceCarLabel;
    [SerializeField] private TMP_Text priceColorLabel;
    [SerializeField] private TMP_Text moneyLabel;
    [SerializeField] private TMP_Text highScoreLabel;
    private int selectedCar;
    private int selectedColor;
    private readonly List<CarController> cars = new List<CarController>();
    public static Action OnUIUpdated;
    private const int PAINT_PRICE = 2000;
    private Toggle[] colorToggles;
    private bool suppressToggleEvent = false;

    private void Awake()
    {
        QualitySettings.SetQualityLevel(YandexGame.savesData.qualityIndex);
        colorToggles = colorToggleGroup.GetComponentsInChildren<Toggle>();
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
        
        selectedCar = YandexGame.savesData.currentCarIndex;
        selectedColor = YandexGame.savesData.currentColorIndex;
        
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
        
        cars[selectedCar].gameObject.SetActive(true);
        cars[selectedCar].ChangeMaterial(carsConfig.cars[selectedCar].materials[selectedColor]);
        
        foreach (var toggle in colorToggles)
        {
            toggle.onValueChanged.AddListener(index =>
            {
                if (!suppressToggleEvent)
                {
                    UpdateColor(Array.IndexOf(colorToggles, toggle));
                }
            });
        }

        OnUIUpdate();
    }

    private void OnDisable()
    {
        OnUIUpdated -= OnUIUpdate;
    }
    private void OnUIUpdate()
    {
        suppressToggleEvent = true;
        
        moneyLabel.text = YandexGame.savesData.moneyAmount.ToString("N0");
        highScoreLabel.text = YandexGame.savesData.highScore.ToString();
        priceColorLabel.text = PAINT_PRICE.ToString("N0");
        priceCarLabel.text = carsConfig.cars[selectedCar].price.ToString();

        colorToggles[selectedColor].isOn = true;
        
        lockImage.gameObject.SetActive(!YandexGame.savesData.isFreeVariantCars[selectedCar, 0]);
        buyButton.interactable = YandexGame.savesData.moneyAmount >= carsConfig.cars[selectedCar].price;
        paintButton.gameObject.SetActive(YandexGame.savesData.isFreeVariantCars[selectedCar, 0] && !YandexGame.savesData.isFreeVariantCars[selectedCar, selectedColor]);
        paintButton.interactable = YandexGame.savesData.moneyAmount >= PAINT_PRICE;

        playButton.interactable = YandexGame.savesData.isFreeVariantCars[selectedCar, selectedColor];
        colorPanel.gameObject.SetActive(YandexGame.savesData.isFreeVariantCars[selectedCar, 0]);
        
        UpdateInfo();
        
        suppressToggleEvent = false;
    }


    public void Buy()
    {
        selectedColor = 0;
        
        YandexGame.savesData.isFreeVariantCars[selectedCar, 0] = true;
        YandexGame.savesData.currentCarIndex = selectedCar;
        YandexGame.savesData.moneyAmount -= carsConfig.cars[selectedCar].price;
        YandexGame.SaveProgress();
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
        YandexGame.savesData.currentColorIndex = selectedColor;
        YandexGame.SaveProgress();
        SceneManager.LoadScene(0);
    }
    private void Select(int index)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].gameObject.SetActive(i == index);
        }
        if (!YandexGame.savesData.isFreeVariantCars[selectedCar, selectedColor])
            selectedColor = 0;
        
        UpdateColor(selectedColor);
    }
    private void UpdateInfo()
    {
        var acceleration = carsConfig.cars[selectedCar].car.GetAcceleration();
        var controllability = carsConfig.cars[selectedCar].car.GetSensitivity();
        var braking = carsConfig.cars[selectedCar].car.GetBraking();
        infoPanel.SetInfo(acceleration, controllability, braking);
    }

    public void UpdateColor(int index)
    {   
        if (suppressToggleEvent) return; 
        
        selectedColor = index;
        cars[selectedCar].ChangeMaterial(carsConfig.cars[selectedCar].materials[index]);
        
        OnUIUpdated?.Invoke();
    }

    public void PaintCar()
    {
        YandexGame.savesData.isFreeVariantCars[selectedCar, selectedColor] = true;
        YandexGame.savesData.currentColorIndex = selectedColor;
        YandexGame.savesData.moneyAmount -= PAINT_PRICE;
        YandexGame.SaveProgress();
        AudioManager.Instance.PlayBuySound();
        OnUIUpdated?.Invoke();
    }
}