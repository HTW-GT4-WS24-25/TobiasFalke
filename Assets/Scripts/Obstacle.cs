using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour, IObject
{
    [SerializeField] private ObstacleType obstacleType;
    
    public enum ObstacleType
    {
        Wall,
        BigObstacle,
        SmallObstacle,
        Rail,
        Hole
    }

    private bool IsJumpable => obstacleType != ObstacleType.Wall;
    
    public ObstacleType GetObstacleType => obstacleType;

    public void Collide(GameObject obstacle, PlayerStats playerStats, PlayerMovement playerMovement, Animator animator)
    {
        if (playerMovement.IsJumping && IsJumpable)
        {
            IncreaseScore(playerStats);
        }
        if (obstacleType == ObstacleType.Rail)
        {
            playerMovement.SetIsOverRail = true;
            Debug.Log("Player is over rail!");
        }
        if (!IsJumpable || !playerMovement.IsJumping)
        {
            TriggerCollisionEffect(playerStats, animator);
        }
    }
    
    public void ExitCollision(GameObject obstacle, PlayerMovement playerMovement)
    {
        if (obstacleType == ObstacleType.Rail)
        {
            playerMovement.SetIsOverRail = false;
            Debug.Log("Player is no longer over rail!");
        }
    }
    
    private void TriggerCollisionEffect(PlayerStats playerStats, Animator animator)
    {
        if(playerStats.isInvincible) return;
        StartCoroutine(SetInvincibility(playerStats, animator));
        Debug.Log("Collision!");
        AudioManager.Instance.PlaySound("crash");
        UpdatePlayerHealth(playerStats);
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
            case ObstacleType.Rail:
                amount = -30;
                break;
        }
        
        playerStats.UpdateHealth(amount);
    }
    
    private static IEnumerator SetInvincibility(PlayerStats playerStats, Animator animator)
    {
        playerStats.isInvincible = true;
        animator.SetBool("isInvincible", true);
        Debug.Log("Player is invincible for " + playerStats.invincibilityDuration + " seconds.");
        yield return new WaitForSeconds(playerStats.invincibilityDuration);
        playerStats.isInvincible = false;
        animator.SetBool("isInvincible", false);
        Debug.Log("Player is no longer invincible.");
    }
}
