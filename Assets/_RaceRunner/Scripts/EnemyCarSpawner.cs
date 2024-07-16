using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCarSpawner : MonoBehaviour
{
    [SerializeField] private EnemyCar[] cars;
    [SerializeField, Range(0f, 1f)] private float trafficDensity;
    [SerializeField] private float maxSpawnInterval;
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpeed;
    [Space]
    
    [Header("Spawners")]
    [SerializeField] private Transform[] passingLanes;
    [SerializeField] private Transform[] oncomingLanes;
    [SerializeField, Range(0f, 5f)] private float maxRandomSpawnDistance;
    private float elapsedTime;
    private List<EnemyCar> deactiveEnemyCars;
    private CarController player;

    private void Awake()
    {
        player = CarController.Instance;
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
    
    private float GetSpawnInterval() => maxSpawnInterval + (Mathf.Clamp(player.currentSpeed, 0f, maxSpeed) - CarController.MIN_SPEED) *
        (minSpawnInterval - maxSpawnInterval) / (maxSpeed - CarController.MIN_SPEED);

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
        if (deactiveEnemyCars.Count == 0 || player.currentSpeed <= 10f)
            return;
        
        var variant = Random.Range(0, 3);
        EnemyCar car;
        Vector3 spawnPoint1 = spawnPoints[0].position + Vector3.forward * Random.Range(-maxRandomSpawnDistance, maxRandomSpawnDistance);
        Vector3 spawnPoint2 = spawnPoints[1].position + Vector3.forward * Random.Range(-maxRandomSpawnDistance, maxRandomSpawnDistance);

        switch (variant)
        {
            case 0:
                car = GetEnemyCar();
                car.transform.SetPositionAndRotation(spawnPoint1, spawnPoints[0].rotation);
                break;
            case 1:
                car = GetEnemyCar();
                car.transform.SetPositionAndRotation(spawnPoint2, spawnPoints[1].rotation);
                break;
            case 2:
                var car1 = GetEnemyCar();
                var car2 = GetEnemyCar();
                car1.transform.SetPositionAndRotation(spawnPoint1, spawnPoints[0].rotation);
                car2.transform.SetPositionAndRotation(spawnPoint2, spawnPoints[1].rotation);
                break;
        }
    }
}