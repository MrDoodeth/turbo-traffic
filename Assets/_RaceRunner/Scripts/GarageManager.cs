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
        QualitySettings.SetQualityLevel(YG2.saves.qualityIndex);
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

        selectedCar = YG2.saves.currentCarIndex;
        selectedColor = YG2.saves.currentColorIndex;

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

        foreach (var toggle in colorToggles)
        {
            toggle.onValueChanged.AddListener(index =>
            {
                if (!suppressToggleEvent)
                {
                    SelectColor(Array.IndexOf(colorToggles, toggle));
                }
            });
        }

        SelectCar(selectedCar);
        SelectColor(selectedColor);

        OnUIUpdate();
    }
    private void OnDisable()
    {
        OnUIUpdated -= OnUIUpdate;
    }
    private void OnUIUpdate()
    {
        suppressToggleEvent = true;
        
        moneyLabel.text = YG2.saves.moneyAmount.ToString("N0");
        highScoreLabel.text = YG2.saves.highScore.ToString();
        priceColorLabel.text = PAINT_PRICE.ToString("N0");
        priceCarLabel.text = carsConfig.cars[selectedCar].price.ToString("N0");

        colorToggles[selectedColor].isOn = true;
        
        lockImage.gameObject.SetActive(!YG2.saves.isFreeVariantCars[selectedCar, 0]);
        buyButton.interactable = YG2.saves.moneyAmount >= carsConfig.cars[selectedCar].price;
        paintButton.gameObject.SetActive(YG2.saves.isFreeVariantCars[selectedCar, 0] && !YG2.saves.isFreeVariantCars[selectedCar, selectedColor]);
        paintButton.interactable = YG2.saves.moneyAmount >= PAINT_PRICE;

        playButton.interactable = YG2.saves.isFreeVariantCars[selectedCar, selectedColor];
        colorPanel.gameObject.SetActive(YG2.saves.isFreeVariantCars[selectedCar, 0]);
        
        UpdateInfo();
        
        suppressToggleEvent = false;
    }

    private void UpdateInfo()
    {
        var acceleration = carsConfig.cars[selectedCar].car.GetAcceleration();
        var controllability = carsConfig.cars[selectedCar].car.GetSensitivity();
        var braking = carsConfig.cars[selectedCar].car.GetBraking();
        infoPanel.SetInfo(acceleration, controllability, braking);
    }


    public void Buy()
    {
        selectedColor = 0;
        
        YG2.saves.isFreeVariantCars[selectedCar, 0] = true;
        YG2.saves.currentCarIndex = selectedCar;
        YG2.saves.moneyAmount -= carsConfig.cars[selectedCar].price;
        YG2.SaveProgress();
        AudioManager.Instance.PlayBuySound();
        OnUIUpdated?.Invoke();
    }
    public void SwipeLeft()
    {
        if (selectedCar <= 0)
            selectedCar = cars.Count - 1;
        else
            selectedCar--;
        SelectCar(selectedCar);
        SelectColor(selectedColor);
    }
    public void SwipeRight()
    {
        if (selectedCar >= cars.Count - 1)
            selectedCar = 0;
        else
            selectedCar++;
        SelectCar(selectedCar);
        SelectColor(selectedColor);
    }

    public void OnStartRace()
    {
        YG2.saves.currentCarIndex = selectedCar;
        YG2.saves.currentColorIndex = selectedColor;
        YG2.SaveProgress();
        YG2.InterstitialAdvShow();
        SceneManager.LoadScene(1);
    }
    private void SelectCar(int index)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].gameObject.SetActive(i == index);
        }
        if (!YG2.saves.isFreeVariantCars[selectedCar, selectedColor])
            selectedColor = 0;
        colorToggles[selectedColor].isOn = true;
    }

    public void SelectColor(int index)
    {   
        if (suppressToggleEvent) return;
        
        if (colorToggles[index].isOn)
        {
            selectedColor = index;
            cars[selectedCar].ChangeMaterial(carsConfig.cars[selectedCar].materials[index]);
        
            OnUIUpdated?.Invoke();
        }
    }

    public void PaintCar()
    {
        YG2.saves.isFreeVariantCars[selectedCar, selectedColor] = true;
        YG2.saves.currentColorIndex = selectedColor;
        YG2.saves.moneyAmount -= PAINT_PRICE;
        YG2.SaveProgress();
        AudioManager.Instance.PlayBuySound();
        OnUIUpdated?.Invoke();
    }
}