using UnityEngine;

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
    
    private float GetSpawnInterval() => minSpawnInterval + (player.currentSpeed - player.minSpeed) *
        (maxSpawnInterval - minSpawnInterval) / (player.maxSpeed - player.minSpeed);
    private void Update()
    {
        elapsedTime += Time.deltaTime;

        print(GetSpawnInterval());
        if (elapsedTime >= GetSpawnInterval())
        {
            if (Random.value <= trafficDensity)
            {
                var variant = Random.Range(0, 3);
                EnemyCar car;
                
                
                //Passing
                switch (variant)
                {
                    case 0:
                        car = Instantiate(cars[Random.Range(0, cars.Length)]);
                        car.transform.SetPositionAndRotation(passingLanes[0].position, passingLanes[0].rotation);
                        break;
                    case 1:
                        car = Instantiate(cars[Random.Range(0, cars.Length)]);
                        car.transform.SetPositionAndRotation(passingLanes[1].position, passingLanes[1].rotation);
                        break;
                    case 2:
                        var car1 = Instantiate(cars[Random.Range(0, cars.Length)]);
                        var car2 = Instantiate(cars[Random.Range(0, cars.Length)]);
                        car1.transform.SetPositionAndRotation(passingLanes[0].position, passingLanes[0].rotation);
                        car2.transform.SetPositionAndRotation(passingLanes[1].position, passingLanes[1].rotation);
                        break;
                }
                
                //Oncoming
                switch (variant)
                {
                    case 0:
                        car = Instantiate(cars[Random.Range(0, cars.Length)]);
                        car.transform.SetPositionAndRotation(oncomingLanes[0].position, oncomingLanes[0].rotation);
                        break;
                    case 1:
                        car = Instantiate(cars[Random.Range(0, cars.Length)]);
                        car.transform.SetPositionAndRotation(oncomingLanes[1].position, oncomingLanes[1].rotation);
                        break;
                    case 2:
                        var car1 = Instantiate(cars[Random.Range(0, cars.Length)]);
                        var car2 = Instantiate(cars[Random.Range(0, cars.Length)]);
                        car1.transform.SetPositionAndRotation(oncomingLanes[0].position, oncomingLanes[0].rotation);
                        car2.transform.SetPositionAndRotation(oncomingLanes[1].position, oncomingLanes[1].rotation);
                        break;
                }
            }
            elapsedTime = 0;
        }
    }
}