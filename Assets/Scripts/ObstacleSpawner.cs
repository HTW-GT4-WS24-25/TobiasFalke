using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleSpawner : ObjectSpawner
{
    // adjustable depending on level or difficulty
    [FormerlySerializedAs("obstaclePrefabs")] [SerializeField] private GameObject[] spawnableObstacles;
    public Transform obstacleParent;
    public float spawnTimeInterval = 5f;
    public float initialSpawnDelay = 5f;
    // for inner calculation only
    private float _timeSinceLastSpawn;
    private bool _firstSpawnOccured;

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        var currentInterval = _firstSpawnOccured ? spawnTimeInterval : initialSpawnDelay;
        if (!(_timeSinceLastSpawn >= currentInterval)) return;
        var spawningObject = spawnableObstacles[Random.Range(0, spawnableObstacles.Length)];
        SpawnObject(spawningObject, obstacleParent);
        _timeSinceLastSpawn = 0f;
        _firstSpawnOccured = true;
    }
}