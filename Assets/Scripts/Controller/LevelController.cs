using Events;
using Interfaces;
using Model;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Controller
{
    public class LevelController : MonoBehaviour
    {
        private LevelModel levelModel;
        [SerializeField] private GameObject[] spawnableObstacles;
        private float timeSinceLastObstacleSpawn;
        [SerializeField] private GameObject[] spawnablePickUps;
        private float timeSinceLastPickUpSpawn;
  
        private void Awake()
        {
            Debug.Log("level" +GameConfig.Instance);
            levelModel = new LevelModel
            {
                CurrentStage = GameConfig.Instance.StartingStage,
                StageSpeed = GameConfig.Instance.BaseStageSpeed,
                StageDuration = GameConfig.Instance.StageDuration,
                StageWidth = GameConfig.Instance.BaseStageWidth,
                StageHeight = GameConfig.Instance.BaseStageHeight,
                ObstacleSpawnInterval = GameConfig.Instance.BaseObstacleSpawnInterval,
                PickupSpawnInterval = GameConfig.Instance.BasePickupSpawnInterval
            };
            Debug.Log("health points now" + levelModel.StageSpeed);
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
            LevelModel.ElapsedTime += Time.deltaTime;
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
            if (LevelModel.ElapsedTime >= nextStageThreshold) levelModel.CurrentStage++;
        }

        private void AttemptSpawn(ref float timeSinceLastSpawn, float interval, GameObject[] spawnables)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (!(timeSinceLastSpawn >= interval)) return;
            TrySpawnObject(spawnables);
            timeSinceLastSpawn = 0f;
        }

        private void TrySpawnObject(GameObject[] spawnables)
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

        private static bool PositionOccupied(Vector3 position, float radius)
        {
            Collider2D hit = Physics2D.OverlapCircle(position, radius);
            return hit;
        }
        
        private void OnStageChanged(LevelEvent.StageChanged evt)
        {
            EnhanceLevelDifficulty();
        }

        private void EnhanceLevelDifficulty()
        {
            float newStageSpeed = GameConfig.Instance.BaseStageSpeed + GameConfig.Instance.SpeedIncreasePerStage;
            levelModel.StageSpeed = newStageSpeed;
            float newObstacleSpawnInterval = GameConfig.Instance.BaseObstacleSpawnInterval / levelModel.CurrentStage;
            levelModel.ObstacleSpawnInterval = newObstacleSpawnInterval;
            float newPickupSpawnInterval = GameConfig.Instance.BasePickupSpawnInterval / levelModel.CurrentStage;
            levelModel.PickupSpawnInterval = newPickupSpawnInterval;
        }
        
        private void RegisterEvents()
        {
            EventManager.Add<LevelEvent.StageChanged>(OnStageChanged);
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            EventManager.Remove<LevelEvent.StageChanged>(OnStageChanged);
        }
    }
}