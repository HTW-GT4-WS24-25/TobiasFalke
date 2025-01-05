using UnityEngine;

public class Item : MonoBehaviour, IObject
{
    [SerializeField] private ItemType itemType;

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
    
    public void Collide(GameObject item, PlayerStats playerStats, PlayerMovement playerMovement)
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
                playerStats.UpdateHealth(50);
                break;
            case ItemType.HealthBoom:
                playerStats.UpdateHealth(100);
                break;
            case ItemType.SpecialBoost:
                playerStats.UpdateSpecial(25);
                break;
            case ItemType.SpecialBoom:
                playerStats.UpdateSpecial(50);
                break;
            case ItemType.SpeedBoost:
                playerStats.UpdateSpeedMultiplier(50);
                break;
            case ItemType.SpeedBoom:
                playerStats.UpdateSpeedMultiplier(100);
                break;
            case ItemType.JumpBoom:
                playerStats.UpdateJumpDuration(50);
                break;
            case ItemType.JumpBoost:
                playerStats.UpdateJumpDuration(100);
                break;
            case ItemType.ScoreBoost:
                playerStats.UpdateSpecial(100);
                break;
            case ItemType.ScoreBoom:
                playerStats.UpdateSpecial(500);
                break;
            case ItemType.ScoreMultiplierBoost:
                playerStats.UpdateSpecial(30);
                break;
        }
    }
}
