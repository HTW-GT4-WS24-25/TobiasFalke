using UnityEngine;

public class LevelController : MonoBehaviour
{
    private LevelModel levelModel;
    public GameObject[] spawnableObstacles;
    private float timeSinceLastSpawn;

    void Awake()
    {
        levelModel = new LevelModel();
    }
    void Update()
    {
        CheckObstacleSpawn();
    }

    private void CheckObstacleSpawn()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= levelModel.GetObstacleSpawnInterval())
        {
            SpawnObstacle();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnObstacle()
    {
        float spawnXPosition = Random.Range(-levelModel.GetLevelWidth(), levelModel.GetLevelWidth());
        Vector3 spawnPosition = new Vector3(spawnXPosition, 10, 0);
        var spawningObject = spawnableObstacles[Random.Range(0, spawnableObstacles.Length)];
        Instantiate(spawningObject, spawnPosition, Quaternion.identity);
    }
}