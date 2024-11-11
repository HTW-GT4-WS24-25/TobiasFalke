using System;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] private GameObject[] obstaclePrefab;
        private float obstacleSpeed = 10f;
        public float obstacleSpawnTime = 5f;
        
        private float timeUntilObstacleSpawn;

        private void Update()
        {
            SpawnLoop();
        }

        private void SpawnLoop()
        {
            timeUntilObstacleSpawn += Time.deltaTime;
            if (timeUntilObstacleSpawn >= obstacleSpawnTime)
            {
                
                Spawn();
                timeUntilObstacleSpawn = 0f;
            }
        }

        private void Spawn()
        {
            GameObject obstacleToSpawn = obstaclePrefab[Random.Range(0, obstaclePrefab.Length)];
            GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
            
            Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
            Debug.Log(obstacleSpeed);
            obstacleRB.linearVelocity = new Vector2(0f, 1f * -obstacleSpeed);
            
        }
    }
}