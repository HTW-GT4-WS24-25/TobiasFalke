using UnityEngine;

public enum ObstacleType
{
    Wall,
    BigObstacle,
    SmallObstacle,
    Rail,
    Hole
}

public class Obstacle : MonoBehaviour, IObject
{
    [SerializeField] internal ObstacleType Type;
    private float fallSpeed;
    public bool IsJumpable => Type != ObstacleType.Wall;

    private void OnEnable()
    {
        EventManagerR.AddListener<LevelEvents.StageSpeedChangedEvent>(OnLevelSpeedChanged);
        InitializeFallSpeed(LevelModel.GetStageSpeed());
    }

    private void OnDisable()
    {
        EventManagerR.RemoveListener<LevelEvents.StageSpeedChangedEvent>(OnLevelSpeedChanged);
    }

    private void Update()
    {
        MoveDownwards();
    }

    public void InitializeFallSpeed(float initialSpeed)
    {
        fallSpeed = initialSpeed;
    }

    public void UpdateFallSpeed(float newSpeed)
    {
        fallSpeed = newSpeed;
    }

    public void MoveDownwards()
    {
        transform.Translate(Vector3.down * (fallSpeed * Time.deltaTime));
    }

    private void OnLevelSpeedChanged(LevelEvents.StageSpeedChangedEvent evt)
    {
        UpdateFallSpeed(evt.StageSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        Debug.Log("'Player collision is triggered.");
        var evt = Events.ObstacleCollisionEvent;
        evt.DamageValue = DetermineDamageAmount();
        evt.Obstacle = this;
        EventManager.Broadcast(evt);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        Debug.Log("Player collision is exited.");
        var evt = Events.ObstacleCollisionEvent;
        evt.Obstacle = this;
        EventManager.Broadcast(evt);
    }

    public int DetermineScore()
    {
        return Type switch
        {
            ObstacleType.BigObstacle => 50,
            ObstacleType.SmallObstacle => 10,
            ObstacleType.Hole => 60,
            _ => 10
        };
    }
    
    public int DetermineDamageAmount()
    {
        return Type switch
        {
            ObstacleType.Wall => -50,
            ObstacleType.BigObstacle => -20,
            ObstacleType.SmallObstacle => -10,
            ObstacleType.Hole => -100,
            ObstacleType.Rail => -30,
            _ => -20
        };
    }
}