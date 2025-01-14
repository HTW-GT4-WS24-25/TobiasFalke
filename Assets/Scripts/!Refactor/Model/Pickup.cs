using UnityEngine;

public enum ItemType
{
    HealthBoost,
    HealthBoom,
    SpecialBoost,
    SpecialBoom,
    SpeedBoost,
    SpeedBoom,
    JumpBoost,
    JumpBoom,
    ScoreBoost,
    ScoreBoom,
    ScoreMultiplierBoost
}

public class Pickup : MonoBehaviour, IObject
{
    [SerializeField] private ItemType itemType;
    private float fallSpeed;

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
        var evt = new PlayerEvents.PickupCollisionEventR(gameObject);
        EventManagerR.Broadcast(evt);
        TriggerItemEffect();
        Destroy(gameObject);
    }

    private void TriggerItemEffect()
    {
        switch (itemType)
        {
            case ItemType.HealthBoost:
                EventManagerR.Broadcast(new PlayerEvents.HealthChangedEventR(50f));
                break;
            case ItemType.SpecialBoost:
                EventManagerR.Broadcast(new PlayerEvents.SpecialChangedEventR(30f));
                break;
            case ItemType.ScoreBoost:
                EventManagerR.Broadcast(new PlayerEvents.ScoreChangedEventR(100f));
                break;
            case ItemType.SpeedBoost:
                EventManagerR.Broadcast(new PlayerEvents.SpeedChangedEventR(1f));
                break;
            case ItemType.JumpBoost:
                EventManagerR.Broadcast(new PlayerEvents.JumpDurationChangedEventR(50f));
                break;
        }
    }
}