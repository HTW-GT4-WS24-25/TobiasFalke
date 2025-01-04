using Mono.Cecil;
using UnityEngine;

public class Obstacle : MonoBehaviour, IObject
{
    [SerializeField] private ObstacleType obstacleType;
    
    private enum ObstacleType
    {
        Wall,
        BigObstacle,
        SmallObstacle,
        Rail,
        Hole
    }

    private bool IsJumpable => obstacleType != ObstacleType.Wall;

    public void Collide(GameObject obstacle, PlayerStats playerStats, PlayerMovement playerMovement)
    {
        if (playerMovement.IsJumping && IsJumpable)
        {
            IncreaseScore(playerStats);
            return;
        }
        if (obstacleType == ObstacleType.Rail)
        {
            HandleGrindCollision(playerStats);
        }
        else if (!IsJumpable || !playerMovement.IsJumping)
        {
            TriggerCollisionEffect(playerStats);
        }
    }
    
    private void HandleGrindCollision(PlayerStats playerStats)
    {
        Debug.Log("Grinding!");
        // TODO: implement logic for grinding -> Coroutine?
        AudioManager.Instance.PlaySound("grind");
        playerStats.UpdateSpecial(15); // TODO: adjust based on balancing
    }
    
    private void TriggerCollisionEffect(PlayerStats playerStats)
    {
        Debug.Log("Collision!");
        AudioManager.Instance.PlaySound("crash");
        UpdatePlayerHealth(playerStats);
        // TODO: add animation to player when colliding
        // TODO: have player phrase through obstacle or be pushed below the screen (game over)
    }
    
    private void IncreaseScore(PlayerStats playerStats)
    {
        int amount = 10;
        switch (obstacleType)
        {
            case ObstacleType.BigObstacle:
                amount = 50;
                break;
            case ObstacleType.SmallObstacle:
                amount = 10;
                break;
            case ObstacleType.Hole:
                amount = 60;
                break;
        }
        playerStats.UpdateScore(amount);
    }
    
    private void UpdatePlayerHealth(PlayerStats playerStats)
    {
        int amount = -20;
        switch (obstacleType)
        {
            case ObstacleType.Wall:
                amount = -50;
                break;
            case ObstacleType.BigObstacle:
                amount = -20;
                break;
            case ObstacleType.SmallObstacle:
                amount = -10;
                break;
            case ObstacleType.Hole:
                amount = -100;
                break;
        }
        
        playerStats.UpdateHealth(amount);
    }
}
