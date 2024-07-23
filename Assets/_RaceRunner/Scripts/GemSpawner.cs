using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class GemSpawner : MonoBehaviour
{
    public GemSlot[] gemsPrefabs;
    [SerializeField] private Transform[] firstLaneSpawnPoints;
    [SerializeField] private Transform[] secondLaneSpawnPoints;
    [SerializeField] private Transform[] thirdLaneSpawnPoints;
    [SerializeField] private Transform[] foursLaneSpawnPoints;
    [Space]
    
    [SerializeField] private float maxSpawnInterval;
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float spawnTimeShift;

    private ObjectPool<Gem>[] pools;
    private float elapsedTime;
    public static event Action<int[]> OnGemPickedUp;

    private void Start()
    {
        if (gemsPrefabs != null)
            pools = new ObjectPool<Gem>[gemsPrefabs.Length];
        
        for (int i = 0; i < pools.Length; i++)
        {
            int index = i;
            pools[i] = new ObjectPool<Gem>(() =>
                {
                    var gem = Instantiate(gemsPrefabs[index].gem);
                    gem.gemSpawner = this;
                    gem.poolIndex = index;
                    return gem;
                },
                gem => { gem.gameObject.SetActive(true); },
                gem => { gem.gameObject.SetActive(false); },
                gem => { Destroy(gem.gameObject); },
                true,
                firstLaneSpawnPoints.Length * 2, firstLaneSpawnPoints.Length * 4);
        }
    }
    private float GetSpawnInterval() => maxSpawnInterval + (Mathf.Clamp(CarController.currentSpeed, 0f, maxSpeed) - CarController.MIN_SPEED) *
        (minSpawnInterval - maxSpawnInterval) / (maxSpeed - CarController.MIN_SPEED);
    private void Update()
    {
        if (!CarController.isControllable)
            return;
        
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= GetSpawnInterval())
        {
            float currentTimeShift = 0;
            foreach (var spawnPoint in GetRandomLane())
            {
                var gem = GetRandomGem();
                gem.transform.position = spawnPoint.position;
                gem.timeShift = currentTimeShift;
                currentTimeShift += spawnTimeShift;
            }
            elapsedTime = 0;
        }
    }

    private Transform[] GetRandomLane()
    {
        var variant = Random.Range(0, 4);

        switch (variant)
        {
            case 0:
                return firstLaneSpawnPoints;
            case 1:
                return secondLaneSpawnPoints;
            case 2:
                return thirdLaneSpawnPoints;
            case 3:
                return foursLaneSpawnPoints;
            
            default:
                return null;
        }
    }
    private Gem GetRandomGem() => pools[Random.Range(0, pools.Length)].Get();
    
    public void ReleaseGem(Gem gem, bool isPlayer)
    {
        if (isPlayer && CarController.isControllable)
        {
            RaceManager.gemsAmount[gem.poolIndex]++;
            OnGemPickedUp?.Invoke(RaceManager.gemsAmount);
        }
        pools[gem.poolIndex].Release(gem);
    }
}

[Serializable]
public class GemSlot
{
    public Gem gem;
    public int price;
}