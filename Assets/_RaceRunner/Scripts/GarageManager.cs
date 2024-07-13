using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class GarageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerCars;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button goButton;
    private int selectedCar;
    private void Awake()
    {
        playerCars[YandexGame.savesData.currentCarIndex].SetActive(true);
        leftButton.onClick.AddListener(SwipeLeft);
        rightButton.onClick.AddListener(SwipeRight);
        goButton.onClick.AddListener(OnStartRace);
        goButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        selectedCar = YandexGame.savesData.currentCarIndex;
    }

    private void SwipeLeft()
    {
        if (selectedCar <= 0)
            selectedCar = playerCars.Length - 1;
        else
            selectedCar--;
        Select(selectedCar);
    }
    private void SwipeRight()
    {
        if (selectedCar >= playerCars.Length - 1)
            selectedCar = 0;
        else
            selectedCar++;
        Select(selectedCar);
    }

    private void OnStartRace()
    {
        YandexGame.savesData.currentCarIndex = selectedCar;
    }
    private void Select(int index)
    {
        for (int i = 0; i < playerCars.Length; i++)
        {
            playerCars[i].gameObject.SetActive(i == index);
        }
    }
}