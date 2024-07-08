using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private int tilesCount = 5;
    [SerializeField] private float offset = 6.25f;
    [SerializeField] private float destroyDistance = 8;
    private Queue<GameObject> roads;
    [SerializeField] private Transform playerTr;
    private Vector3 spawnPoint;
    private int countOfRoads;

    private void Awake()
    {
        roads = new Queue<GameObject>(tilesCount);
        
        for (int i = 0; i < tilesCount; i++)
        {
            spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + offset * i);
            var prefab = Instantiate(roadPrefab, spawnPoint, Quaternion.identity);
            roads.Enqueue(prefab);
        }
    }

    private void Update()
    {
        if (Vector3.Distance(roads.Peek().transform.position, playerTr.position) > destroyDistance)
        {
            spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + offset * (tilesCount + countOfRoads));
            countOfRoads++;
            var road = roads.Dequeue();
            road.transform.position = spawnPoint;
            roads.Enqueue(road);
        }
    }
}
