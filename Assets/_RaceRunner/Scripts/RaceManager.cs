using UnityEngine;
using YG;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private CarsConfig carsConfig;
    [SerializeField] private RoadSpawner roadSpawner;
    [SerializeField] private EnemyCarSpawner enemySpawner;
    [SerializeField] private TargetHolder[] targetHolders;
    [SerializeField] private StartPanel startPanel;
    private CarController player;

    private void Awake()
    {
        player = Instantiate(carsConfig.cars[YandexGame.savesData.currentCarIndex].car, Vector3.zero, Quaternion.identity);
        roadSpawner.player = player;
        enemySpawner.player = player;
        startPanel.player = player;
        foreach (var targetHolder in targetHolders)
        {
            targetHolder.target = player.transform;
        }
    }
}
