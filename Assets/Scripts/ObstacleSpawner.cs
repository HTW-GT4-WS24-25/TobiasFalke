using UnityEngine;

// TODO: Destroy obstacle after it leaves the screen

public class ObstacleSpawner : ObjectSpawner
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    public Transform obstacleParent;
    public float spawnTimeInterval = 5f;
    public float initialSpawnDelay = 5f;
    private float _timeSinceLastSpawn;
    private bool _firstSpawnOccured;

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        var currentInterval = _firstSpawnOccured ? spawnTimeInterval : initialSpawnDelay;
        if (!(_timeSinceLastSpawn >= currentInterval)) return;
        var spawningObject = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        SpawnObject(spawningObject, obstacleParent);
        _timeSinceLastSpawn = 0f;
        _firstSpawnOccured = true;
    }
}