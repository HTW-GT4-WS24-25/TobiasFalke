using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelView LevelView;
    private LevelModel levelModel;
    public GameObject[] spawnableObstacles;
    public GameObject[] spawnablePickUps;
    private float timeSinceLastObstacleSpawn;
    private float timeSinceLastPickUpSpawn;

    void Awake()
    {
        levelModel = new LevelModel();
    }

    void Start()
    {
        UpdateLevelView();
        BroadcastLevelSpeedChange();
        EventManagerR.AddListener<GameEvents.LevelChangedEvent>(OnLevelChanged);
    }
    void Update()
    {
        TriggerSpawnInterval();
    }
    
    private void OnDestroy()
    {
        EventManagerR.RemoveListener<GameEvents.LevelChangedEvent>(OnLevelChanged);
    }

    private void UpdateLevelView()
    {
        EventManagerR.Broadcast(new GameEvents.LevelSpeedChangedEvent(LevelModel.GetStageSpeed()));
        EventManagerR.Broadcast(new GameEvents.LevelChangedEvent(levelModel.GetCurrentStage()));
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
        float spawnXPosition = Random.Range(-levelModel.GetLevelWidth(), levelModel.GetLevelWidth());
        Vector3 spawnPosition = new Vector3(spawnXPosition, 10, 0);

        if (spawnables.Length == 0)
            return;

        GameObject objectToSpawn = spawnables[Random.Range(0, spawnables.Length)];
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        spawnedObject.layer = LayerMask.NameToLayer(layer);
    }

    private void BroadcastLevelSpeedChange()
    {
        float newSpeed = LevelModel.GetStageSpeed();
        EventManagerR.Broadcast(new GameEvents.LevelSpeedChangedEvent(newSpeed));
    }

    private void OnLevelChanged(GameEvents.LevelChangedEvent evt)
    {
        levelModel.SetCurrentStage(evt.NewLevel);
        BroadcastLevelSpeedChange();
    }
}