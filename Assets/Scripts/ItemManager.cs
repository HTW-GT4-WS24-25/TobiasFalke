using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemType itemType;

    public enum ItemType
    {
        SpeedBoost,
        ScoreMultiplier,
        HealthRecovery,
        JumpDuration
    }
    
    public PlayerStats TriggerItemEffect(PlayerStats playerStats, ItemType type)
    {
        switch (type)
        {
            case ItemType.SpeedBoost:
                playerStats.ChangeSpeed(0);
                break;

            case ItemType.ScoreMultiplier:
                playerStats.MultiplyScore(1.25f);
                break;

            case ItemType.HealthRecovery:
                playerStats.ChangeHealth(50);
                break;

            case ItemType.JumpDuration:
                playerStats.ChangeJumpDuration(0f);
                break;
        }

        Destroy(gameObject); // Destroy the item after pickup
        return playerStats;
    }
}
