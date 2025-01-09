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


public class Item : MonoBehaviour
{
    [SerializeField] private ItemType itemType;

    private float fallSpeed;
    
    
    private void Update()
    {
        fallSpeed = GameModel.Instance.GameSpeed / 2;
        transform.Translate(new Vector3(0f, -fallSpeed * Time.deltaTime, 0f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var evt = Events.PickupEvent;
        evt.ItemType = itemType;
        EventManager.Broadcast(evt);
        Destroy(gameObject); // Destroy the item after pickup.
    }
}