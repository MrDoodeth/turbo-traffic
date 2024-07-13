using UnityEngine;
using YG;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private CarController[] playerCars;
    public RoadSpawner roadSpawner;
    public EnemyCarSpawner enemySpawner;
    private CarController player;

    private void Awake()
    {
        player = Instantiate(playerCars[YandexGame.savesData.currentCarIndex], Vector3.zero, Quaternion.identity);
        roadSpawner.player = player;
        enemySpawner.player = player;
    }
}
