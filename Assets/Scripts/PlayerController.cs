using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int _maxHealth = 100;
    public int _maxSpecial = 100;
    
    private PlayerStats stats;
    private PlayerMovement playerMovement;

    private void Start()
    {
        stats = new PlayerStats
        {
            _health = _maxHealth,
            _special = 0,
            _score = 0f
        };

        playerMovement = GetComponent<PlayerMovement>(); // Assuming PlayerMovement is attached to the player
    }

    private void Update()
    {
        if (Input.GetKeyDown("x")) TriggerSpecialAction();
        stats._score += Time.deltaTime * 5;
    }

    private void LateUpdate()
    {
        UIManager.Instance.SetScore(stats._score);
        if (stats._special >= 100) UIManager.Instance.SetSpecialActionButtonVisibility(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckItemCollision(other);

        if (stats._health <= 0)
        {
            Destroy(gameObject);
            // TODO: connect to GameManager and trigger Game Over
        }
    }

    private void CheckItemCollision(Collision2D other)
    {
        ItemManager item = other.gameObject.GetComponent<ItemManager>();
        if (item != null)
        {
            stats = item.TriggerItemEffect(stats, item.itemType);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ObstacleManger obstacleManager = other.GetComponent<ObstacleManger>();

            if (obstacleManager != null)
            {
                // If the obstacle is jumpable and the player is jumping, do nothing
                if (obstacleManager.ObstacleJumpable && playerMovement.IsJumping)
                {
                    return;
                }

                // Handle the collision based on the type of obstacle
                if (obstacleManager.ObstacleGrindable)
                {
                    HandleGrindCollision();
                }
                else
                {
                    TriggerCollisionEffect(other);
                }
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        stats.ChangeSpecial(10);
    }

    private void HandleGrindCollision()
    {
        // Implement grind logic (if the player is on a grind rail, for example)
        Debug.Log("Player is grinding over an obstacle.");
        // Add logic to manage player's state while grinding
        
        // continously while grinding:
        stats.ChangeSpecial(5);
    }

    private void TriggerCollisionEffect(Collider2D other)
    {
        stats.ChangeHealth(-50);
        // Implement collision effect, such as animation or sound
        Debug.Log("Collision effect triggered.");
        Destroy(other.gameObject);
    }

    private void TriggerSpecialAction()
    {
        stats._special = 0;
        UIManager.Instance.SetSpecialActionButtonVisibility(false);
        // TODO: implement special action
    }
}
