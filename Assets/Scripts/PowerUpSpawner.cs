using System.Collections.Generic;
using UnityEngine;

// Manages spawning of power-ups and utility objects.

public class PowerUpSpawner : ObjectSpawner
{
    public GameObject[] spawnablePowerUps;
    public float spawnTimeInterval = 5f;
    public float initialSpawnDelay = 5f;
    public Transform itemParent; // parent object of spawned items in scene

    // for inner calculation
    private float _timeSinceLastSpawn;
    private bool _firstSpawnOccured;

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        var currentInterval = _firstSpawnOccured ? spawnTimeInterval : initialSpawnDelay;
        if (!(_timeSinceLastSpawn >= currentInterval)) return;
        var spawningObject = spawnablePowerUps[Random.Range(0, spawnablePowerUps.Length)];
        SpawnObject(spawningObject, itemParent);
        _timeSinceLastSpawn = 0f;
        _firstSpawnOccured = true;
    }
}