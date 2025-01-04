using UnityEngine;

public class ItemSpawner : ObjectSpawner
{
    // adjustable depending on level or difficulty
    [SerializeField] public GameObject[] spawnableItems;
    public float spawnTimeInterval = 5f;
    public float initialSpawnDelay = 5f;
    public Transform itemParent; 
    // for inner calculation only
    private float _timeSinceLastSpawn;
    private bool _firstSpawnOccured;

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        var currentInterval = _firstSpawnOccured ? spawnTimeInterval : initialSpawnDelay;
        if (!(_timeSinceLastSpawn >= currentInterval)) return;
        var spawningObject = spawnableItems[Random.Range(0, spawnableItems.Length)];
        SpawnObject(spawningObject, itemParent);
        _timeSinceLastSpawn = 0f;
        _firstSpawnOccured = true;
    }
}