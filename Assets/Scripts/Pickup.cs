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
        EventManagerR.AddListener<GameEvents.LevelSpeedChangedEvent>(OnLevelSpeedChanged);
        InitializeFallSpeed(LevelModel.GetStageSpeed());
    }

    private void OnDisable()
    {
        EventManagerR.RemoveListener<GameEvents.LevelSpeedChangedEvent>(OnLevelSpeedChanged);
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
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    private void OnLevelSpeedChanged(GameEvents.LevelSpeedChangedEvent evt)
    {
        UpdateFallSpeed(evt.LevelSpeed);
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