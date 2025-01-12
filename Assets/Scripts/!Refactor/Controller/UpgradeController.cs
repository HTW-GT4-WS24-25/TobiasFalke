using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public GameObject upgradePrefab;  // Prefab to instantiate for upgrades
    public float spawnInterval = 5f;  // Interval between spawns
    public Vector2 spawnRange = new Vector2(-8f, 8f);  // Horizontal range for spawning

    private float timeSinceLastSpawn = 0f;

    void Update()
    {
        // Track time and spawn upgrades at regular intervals
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnUpgrade();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnUpgrade()
    {
        // Randomly determine spawn position within defined range
        float spawnXPosition = Random.Range(spawnRange.x, spawnRange.y);
        Vector3 spawnPosition = new Vector3(spawnXPosition, transform.position.y, 0);
        
        // Instantiate the upgrade prefab
        Instantiate(upgradePrefab, spawnPosition, Quaternion.identity);
    }
}