using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private Road roadPrefab;
    [SerializeField] private int tilesCount = 5;
    [SerializeField] private float offset = 6.25f;
    [SerializeField] private float destroyDistance = 8;
    private Queue<Road> roads;
    private Vector3 spawnPoint;
    private int countOfRoads;
    private CarController player;

    private void Start()
    {
        player = CarController.Instance;
        roads = new Queue<Road>(tilesCount);
        
        for (int i = 0; i < tilesCount; i++)
        {
            spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + offset * i);
            var prefab = Instantiate(roadPrefab, spawnPoint, Quaternion.identity, this.transform);
            roads.Enqueue(prefab);
        }
    }

    private void Update()
    {
        if ((roads.Peek().transform.position - player.transform.position).z < -destroyDistance)
        {
            spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + offset * (tilesCount + countOfRoads));
            countOfRoads++;
            var road = roads.Dequeue();
            road.transform.SetPositionAndRotation(spawnPoint, RandomRotation());
            road.RandomVariant();
            roads.Enqueue(road);
        }
    }
    private static Quaternion RandomRotation() => Quaternion.Euler(0f, Random.value <= 0.5f ? 0f : 180f, 0f);
}
