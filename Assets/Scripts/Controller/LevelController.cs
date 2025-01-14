using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelView LevelView;
    private LevelModel levelModel;
    public GameObject[] spawnableObstacles;
    public GameObject[] spawnablePickUps;
    private float timeSinceLastObstacleSpawn;
    private float timeSinceLastPickUpSpawn;

    [SerializeField] private int maxSpawnAttempts = 10; // Maximum number of attempts to find a free spot
    [SerializeField] private float spawnCheckRadius = 1.0f; // Radius for overlap check

    void Awake()
    {
        levelModel = new LevelModel();
    }

    void Start()
    {
        UpdateLevelView();
        BroadcastLevelSpeedChange();
        EventManager.AddListener<LevelEvents.StageChangedEventR>(OnLevelChanged);
    }

    void Update()
    {
        TriggerSpawnInterval();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<LevelEvents.StageChangedEventR>(OnLevelChanged);
    }

    private void UpdateLevelView()
    {
        EventManager.Broadcast(new LevelEvents.StageSpeedChangedEventR(LevelModel.GetStageSpeed()));
        EventManager.Broadcast(new LevelEvents.StageChangedEventR(levelModel.GetCurrentStage()));
    }

    void TriggerSpawnInterval()
    {
        HandleSpawning(ref timeSinceLastObstacleSpawn, levelModel.GetObstacleSpawnInterval(), spawnableObstacles);
        HandleSpawning(ref timeSinceLastPickUpSpawn, levelModel.GetPickUpSpawnInterval(), spawnablePickUps);
    }

    private void HandleSpawning(ref float timeSinceLastSpawn, float spawnInterval, GameObject[] spawnables)
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnObject(spawnables, "World");
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnObject(GameObject[] spawnables, string layer)
    {
        if (spawnables.Length == 0)
            return;

        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            float spawnXPosition = Random.Range(-levelModel.GetStageWidth(), levelModel.GetStageWidth());
            Vector3 spawnPosition = new Vector3(spawnXPosition, 10, 0);

            if (!IsPositionOccupied(spawnPosition, spawnCheckRadius))
            {
                GameObject objectToSpawn = spawnables[Random.Range(0, spawnables.Length)];
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                spawnedObject.layer = LayerMask.NameToLayer(layer);
                break; // Exit loop once successfully spawned
            }
        }
    }

    private bool IsPositionOccupied(Vector3 position, float radius)
    {
        // Check for overlapping colliders which indicate occupation
        Collider2D hit = Physics2D.OverlapCircle(position, radius);
        return hit != null;
    }

    private void BroadcastLevelSpeedChange()
    {
        float newSpeed = LevelModel.GetStageSpeed();
        EventManager.Broadcast(new LevelEvents.StageSpeedChangedEventR(newSpeed));
    }

    private void OnLevelChanged(LevelEvents.StageChangedEventR evt)
    {
        levelModel.SetCurrentStage(evt.NewStage);
        BroadcastLevelSpeedChange();
    }
}