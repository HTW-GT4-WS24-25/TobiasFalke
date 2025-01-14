using Events;
using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class LevelController : MonoBehaviour
    {
        public LevelView levelView;
        private LevelModel levelModel;
        public GameObject[] spawnableObstacles;
        public GameObject[] spawnablePickUps;
        private float timeSinceLastObstacleSpawn;
        private float timeSinceLastPickUpSpawn;
        [SerializeField] private int maxSpawnAttempts = 10;
        [SerializeField] private float spawnCheckRadius = 1.0f;

        void Awake()
        {
            levelModel = new LevelModel();
        }

        void Start()
        {
            BroadcastLevelUpdate();
            EventManager.AddListener<LevelEvents.StageChangedEvent>(OnLevelChanged);
        }

        void Update()
        {
            TriggerSpawnIntervals();
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<LevelEvents.StageChangedEvent>(OnLevelChanged);
        }

        private void BroadcastLevelUpdate()
        {
            EventManager.Broadcast(new LevelEvents.StageSpeedChangedEvent(LevelModel.GetStageSpeed()));
            EventManager.Broadcast(new LevelEvents.StageChangedEvent(levelModel.GetCurrentStage()));
        }

        void TriggerSpawnIntervals()
        {
            AttemptSpawn(ref timeSinceLastObstacleSpawn, levelModel.GetObstacleSpawnInterval(), spawnableObstacles);
            AttemptSpawn(ref timeSinceLastPickUpSpawn, levelModel.GetPickUpSpawnInterval(), spawnablePickUps);
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

            for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
            {
                float spawnXPosition = Random.Range(-levelModel.GetStageWidth(), levelModel.GetStageWidth());
                Vector3 spawnPosition = new Vector3(spawnXPosition, 10f, 0f);

                if (!PositionOccupied(spawnPosition, spawnCheckRadius))
                {
                    GameObject objectToSpawn = spawnables[Random.Range(0, spawnables.Length)];
                    GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                    spawnedObject.layer = LayerMask.NameToLayer(layer);
                    break; // Exit after successful spawn
                }
            }
        }

        private bool PositionOccupied(Vector3 position, float radius)
        {
            Collider2D hit = Physics2D.OverlapCircle(position, radius);
            return hit != null;
        }

        private void OnLevelChanged(LevelEvents.StageChangedEvent evt)
        {
            levelModel.SetCurrentStage(evt.NewStage);
            BroadcastLevelUpdate();
        }
    }
}