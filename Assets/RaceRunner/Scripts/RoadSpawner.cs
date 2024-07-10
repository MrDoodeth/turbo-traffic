using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private int tilesCount = 5;
    [SerializeField] private float offset = 6.25f;
    [SerializeField] private float destroyDistance = 8;
    [SerializeField] private CarController player;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    private Queue<GameObject> roads;
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
        //Constraint
        player.transform.position = new Vector3 (Mathf.Clamp(player.transform.position.x, minX, maxX), player.transform.position.y, player.transform.position.z);

        if (player.transform.position.x <= minX || player.transform.position.x >= maxX)
            player.isOnBorder = true;
        else
            player.isOnBorder = false;
        
        if ((roads.Peek().transform.position - player.transform.position).z < -destroyDistance)
        {
            spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + offset * (tilesCount + countOfRoads));
            countOfRoads++;
            var road = roads.Dequeue();
            road.transform.position = spawnPoint;
            roads.Enqueue(road);
        }
    }
}
