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

    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickupEvent evt = Events.PickupEvent;
            evt.ItemType = this.itemType;
            EventManager.Broadcast(evt);
            Destroy(gameObject); // Destroy the item after pickup

        }
    }
}