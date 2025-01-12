using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject obstaclePrefab;  // Prefab for obstacles
    public float spawnInterval = 3f;  // Interval between obstacle spawns
    public Vector2 spawnRange = new Vector2(-8f, 8f);  // Horizontal range for spawning

    private float timeSinceLastSpawn = 0f;

    void Update()
    {
        // Track time and spawn obstacles regularly
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnObstacle();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnObstacle()
    {
        // Randomly set a spawn position within the range
        float spawnXPosition = Random.Range(spawnRange.x, spawnRange.y);
        Vector3 spawnPosition = new Vector3(spawnXPosition, transform.position.y, 0);
        
        // Instantiate the obstacle
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
    }
}