using UnityEngine;

public class Item : MonoBehaviour, IObject
{
    [SerializeField] private ItemType itemType;

    private float fallSpeed;
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
    
    private void Update()
    {
        fallSpeed = GameModel.Instance.GameSpeed / 2;
        transform.Translate(new Vector3(0f, -fallSpeed * Time.deltaTime, 0f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickupEvent evt = Events.PickupEvent;
            evt.Item = this;
            EventManager.Broadcast(evt);
        }
    }
    
    public void Collide(GameObject item, PlayerStats playerStats, PlayerMovement playerMovement, Animator animator)
    {
        AudioManager.Instance.PlaySound("item");
        TriggerItemEffect(playerStats, itemType);
        Destroy(gameObject); // Destroy the item after pickup
    }
    
    public void TriggerItemEffect(PlayerStats playerStats, ItemType type)
    {
        switch (type)
        {
            case ItemType.HealthBoost:
                playerStats.ChangeHealth(50);
                break;
            case ItemType.HealthBoom:
                playerStats.ChangeHealth(100);
                break;
            case ItemType.SpecialBoost:
                playerStats.ChangeSpecial(25);
                break;
            case ItemType.SpecialBoom:
                playerStats.ChangeSpecial(50);
                break;
            case ItemType.SpeedBoost:
                playerStats.ChangeSpeedMultiplier(50);
                break;
            case ItemType.SpeedBoom:
                playerStats.ChangeSpeedMultiplier(100);
                break;
            case ItemType.JumpBoom:
                playerStats.ChangeJumpDuration(50);
                break;
            case ItemType.JumpBoost:
                playerStats.ChangeJumpDuration(100);
                break;
            case ItemType.ScoreBoost:
                playerStats.ChangeSpecial(100);
                break;
            case ItemType.ScoreBoom:
                playerStats.ChangeSpecial(500);
                break;
            case ItemType.ScoreMultiplierBoost:
                playerStats.ChangeSpecial(30);
                break;
        }
    }
}
