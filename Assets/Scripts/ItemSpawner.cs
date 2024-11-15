using UnityEngine;

// TODO: Item destroyen, sobald es den Bildschirmrand verlässt

public class ItemSpawner : ObjectSpawner
{
    [SerializeField] private GameObject[] itemPrefabs;
    public Transform itemParent;
    public float spawnTimeInterval = 5f;
    public float initialSpawnDelay = 5f;
    private float _timeSinceLastSpawn;
    private bool _firstSpawnOccured;

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        var currentInterval = _firstSpawnOccured ? spawnTimeInterval : initialSpawnDelay;
        if (!(_timeSinceLastSpawn >= currentInterval)) return;
        var spawningObject = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
        SpawnObject(spawningObject, itemParent);
        _timeSinceLastSpawn = 0f;
        _firstSpawnOccured = true;
    }
}