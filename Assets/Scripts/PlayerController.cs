using UnityEngine;

// TODO: Health-Logik und SP-Bar-Logik einbauen

public class PlayerController : MonoBehaviour
{
    private int _playerHealth = 3;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Obstacle"))
        {
            _playerHealth--;
            if (_playerHealth == 0)
            {
                Destroy(gameObject);
                // TODO: connect to GameManager and trigger Game Over
            }
        }

        // Check if the collided object has an Item component
        ItemManager item = other.gameObject.GetComponent<ItemManager>();
        if (item != null)
        {
            switch (item.itemType)
            {
                case ItemManager.ItemType.SpeedBoost:
                    IncreasePlayerSpeed();  // Call method to increase player speed
                    break;

                case ItemManager.ItemType.ScoreMultiplier:
                    DoubleScore();  // Call method to double score points
                    break;

                case ItemManager.ItemType.HealthRecovery:
                    RegainHealth();  // Call method to restore health
                    break;

                case ItemManager.ItemType.JumpDuration:
                    JumpDuration();  // Call method to extend jump duration
                    break;
            }

            Destroy(other.gameObject);  // Destroy the item after pickup
        }
    }

        void IncreasePlayerSpeed()
    {
        Debug.Log("Speed Boost Item Picked!");
        // Logic to increase player speed
    }

    void DoubleScore()
    {
        Debug.Log("Score Multiplier Item Picked!");
        // Logic to double score points
    }

    void RegainHealth()
    {
        Debug.Log("Health Recovery Item Picked!");
        // Logic to regain player health
    }

    void JumpDuration()
    {
        Debug.Log("Jump Duration Item Picked!");
        // Logic to extend Jump Duration
    }
}