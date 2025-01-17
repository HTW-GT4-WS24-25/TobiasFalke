using Config;
using Events;
using Interfaces;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private GameObject[] spawnableObstacles;
        [SerializeField] private GameObject[] spawnablePickUps;
        
        private LevelModel levelModel;
        
        private float timeSinceLastObstacleSpawn;
        private float timeSinceLastPickUpSpawn;
  
        private void Awake()
        {
            levelModel = new LevelModel
            {
                CurrentStage = GameConfig.StartingStage,
                StageSpeed = GameConfig.BaseStageSpeed,
                StageDuration = GameConfig.StageDuration,
                StageWidth = GameConfig.BaseStageWidth,
                StageHeight = GameConfig.BaseStageHeight,
                ObstacleSpawnInterval = GameConfig.BaseObstacleSpawnInterval,
                PickupSpawnInterval = GameConfig.BasePickupSpawnInterval
            };
        }

        private void Start()
        {
            RegisterEvents();
        }
        
        private void Update()
        {
            CountPlayTime();
            TriggerTimedEvents();
        }

        private void CountPlayTime()
        {
            levelModel.ElapsedTime += Time.deltaTime;
        }
        
        private void TriggerTimedEvents()
        {
            AttemptStageUpdate();
            AttemptSpawn(ref timeSinceLastObstacleSpawn, levelModel.ObstacleSpawnInterval, spawnableObstacles);
            AttemptSpawn(ref timeSinceLastPickUpSpawn, levelModel.PickupSpawnInterval, spawnablePickUps);
        }

        private void AttemptStageUpdate()
        {
            float nextStageThreshold = levelModel.StageDuration * levelModel.CurrentStage;
            if (levelModel.ElapsedTime >= nextStageThreshold) levelModel.CurrentStage++;
        }

        private void AttemptSpawn(ref float timeSinceLastSpawn, float interval, GameObject[] spawnables)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= interval)
            {
                TrySpawnObject(spawnables, "World");
                timeSinceLastSpawn = 0f;
            }
        }

        private void TrySpawnObject(GameObject[] spawnables, string layer)
        {
            if (spawnables.Length == 0) return;

            const int maxSpawnAttempts = 5;
            GameObject objectToSpawn = spawnables[Random.Range(0, spawnables.Length)];

            for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
            {
                float spawnXPosition = Random.Range(-levelModel.StageWidth/2, levelModel.StageWidth/2);
                Vector3 spawnPosition = new Vector3(spawnXPosition, levelModel.StageHeight, 0f);

                if (PositionOccupied(spawnPosition, 1f)) continue;
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                // TODO: find a better way to achieve objects initializing at correct stage speed
                spawnedObject.GetComponent<FallingObject>().fallSpeed = levelModel.StageSpeed;
                break;
            }
        }

        private bool PositionOccupied(Vector3 position, float radius)
        {
            Collider2D hit = Physics2D.OverlapCircle(position, radius);
            return hit != null;
        }
        
        private void OnStageChanged(LevelEvent.StageChanged evt)
        {
            EnhanceLevelDifficulty();
        }

        private void EnhanceLevelDifficulty()
        {
            float newStageSpeed = GameConfig.BaseStageSpeed + GameConfig.SpeedIncreasePerStage;
            levelModel.StageSpeed = newStageSpeed;
            float newObstacleSpawnInterval = GameConfig.BaseObstacleSpawnInterval / levelModel.CurrentStage;
            levelModel.ObstacleSpawnInterval = newObstacleSpawnInterval;
            float newPickupSpawnInterval = GameConfig.BasePickupSpawnInterval / levelModel.CurrentStage;
            levelModel.PickupSpawnInterval = newPickupSpawnInterval;
        }
        
        private void RegisterEvents()
        {
            EventManager.AddListener<LevelEvent.StageChanged>(OnStageChanged);
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            EventManager.RemoveListener<LevelEvent.StageChanged>(OnStageChanged);
        }
    }
}