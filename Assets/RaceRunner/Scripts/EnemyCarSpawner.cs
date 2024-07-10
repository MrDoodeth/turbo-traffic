using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCarSpawner : MonoBehaviour
{
    [SerializeField] private CarController player;
    [Space]
    
    [SerializeField] private EnemyCar[] cars;
    [SerializeField, Range(0f, 1f)] private float trafficDensity;
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpawnInterval;
    [Space]
    
    [Header("Spawners")]
    [SerializeField] private Transform[] passingLanes;
    [SerializeField] private Transform[] oncomingLanes;
    private float elapsedTime;
    private List<EnemyCar> deactiveEnemyCars;

    private void Awake()
    {
        SpawnEnemies();
    }
    private void SpawnEnemies()
    {
        deactiveEnemyCars = new List<EnemyCar>();
        for (int i = 0; i < cars.Length; i++)
        {
            var car = Instantiate(cars[i]);
            car.gameObject.SetActive(false);
            car.SetSpawnParameters(player.transform, this);
            deactiveEnemyCars.Add(car);
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= GetSpawnInterval())
        {
            if (Random.value <= trafficDensity)
            {
                //Passing
                SpawnEnemyCar(passingLanes);
                
                //Oncoming
                SpawnEnemyCar(oncomingLanes);
            }
            elapsedTime = 0;
        }
    }
    
    private float GetSpawnInterval() => minSpawnInterval + (player.currentSpeed - player.minSpeed) *
        (maxSpawnInterval - minSpawnInterval) / (player.maxSpeed - player.minSpeed);

    private EnemyCar GetEnemyCar()
    {
        var car = deactiveEnemyCars[Random.Range(0, deactiveEnemyCars.Count)];
        deactiveEnemyCars.Remove(car);
        car.gameObject.SetActive(true);
        return car;
    }

    public void ReleaseEnemyCar(EnemyCar car)
    {
        car.gameObject.SetActive(false);
        deactiveEnemyCars.Add(car);
    }
    
    private void SpawnEnemyCar(Transform[] spawnPoints)
    {
        var variant = Random.Range(0, 3);
        EnemyCar car;

        switch (variant)
        {
            case 0:
                car = GetEnemyCar();
                car.transform.SetPositionAndRotation(spawnPoints[0].position, spawnPoints[0].rotation);
                break;
            case 1:
                car = GetEnemyCar();
                car.transform.SetPositionAndRotation(spawnPoints[1].position, spawnPoints[1].rotation);
                break;
            case 2:
                var car1 = GetEnemyCar();
                var car2 = GetEnemyCar();
                car1.transform.SetPositionAndRotation(spawnPoints[0].position, spawnPoints[0].rotation);
                car2.transform.SetPositionAndRotation(spawnPoints[1].position, spawnPoints[1].rotation);
                break;
        }
    }
}