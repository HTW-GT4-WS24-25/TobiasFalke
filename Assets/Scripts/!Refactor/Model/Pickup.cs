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
       // Debug.Log("Pickup fall speed updated to: " + fallSpeed);
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
        var evt = Events.PickupEvent;
        evt.ItemType = itemType;
        EventManager.Broadcast(evt);
        Destroy(gameObject);
    }
}