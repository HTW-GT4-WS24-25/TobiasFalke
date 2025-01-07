using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleSpawner : ObjectSpawner
{
    // adjustable depending on level or difficulty
    [FormerlySerializedAs("obstaclePrefabs")] [SerializeField] private GameObject[] spawnableObstacles;
    public Transform obstacleParent;
    private float spawnTimeInterval = 2f;
    private float initialSpawnDelay = 2f;
    // for inner calculation only
    private float _timeSinceLastSpawn;
    private bool _firstSpawnOccured;

    private void Update()
    {
        spawnTimeInterval = 2f / GameModel.Instance.GameSpeed;
        _timeSinceLastSpawn += Time.deltaTime;
        var currentInterval = _firstSpawnOccured ? spawnTimeInterval : initialSpawnDelay;
        if (!(_timeSinceLastSpawn >= currentInterval)) return;
        var spawningObject = spawnableObstacles[Random.Range(0, spawnableObstacles.Length)];
        SpawnObject(spawningObject, obstacleParent);
        _timeSinceLastSpawn = 0f;
        _firstSpawnOccured = true;
    }
}