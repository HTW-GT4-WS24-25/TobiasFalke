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
        EventManagerR.AddListener<LevelEvents.StageSpeedChangedEventR>(OnLevelSpeedChanged);
        InitializeFallSpeed(LevelModel.GetStageSpeed());
    }

    private void OnDisable()
    {
        EventManagerR.RemoveListener<LevelEvents.StageSpeedChangedEventR>(OnLevelSpeedChanged);
    }

    private void Update()
    {
        MoveDownwards();
        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
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

    private void OnLevelSpeedChanged(LevelEvents.StageSpeedChangedEventR evt)
    {
        UpdateFallSpeed(evt.StageSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        DetermineDamageAmount();
        var evt = new PlayerEvents.ObstacleCollisionEventR(gameObject);
        EventManagerR.Broadcast(evt);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        DetermineDamageAmount();
        var evt = new PlayerEvents.ObstacleCollisionExitEventR(gameObject);
        EventManagerR.Broadcast(evt);
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