using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour, IObject
{
    [SerializeField] public ObstacleType obstacleType {get; private set; }

    private float fallSpeed;
    


    private bool IsJumpable => obstacleType != ObstacleType.Wall;
    
    public ObstacleType GetObstacleType => obstacleType;

    private void Update()
    {
        fallSpeed = GameModel.Instance.GameSpeed / 2;
        transform.Translate(new Vector3(0f, -fallSpeed * Time.deltaTime, 0f));
    }
    
    //TOOD throw ObstacleCollisionEvent
    public void Collide(GameObject obstacle, PlayerMovement playerMovement)
    {
        if (playerMovement.IsJumping && IsJumpable)
        {
            IncreaseScore();
        }
        if (obstacleType == ObstacleType.Rail)
        {
            playerMovement.SetIsOverRail = true;
            Debug.Log("Player is over rail!");
        }
        if (!IsJumpable || !playerMovement.IsJumping)
        {
            TriggerCollisionEffect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsJumpable)
        {
            
        }
    }
    
    // public void ExitCollision(GameObject obstacle, PlayerMovement playerMovement)
    // {
    //     if (obstacleType == ObstacleType.Rail)
    //     {
    //         playerMovement.SetIsOverRail = false;
    //         Debug.Log("Player is no longer over rail!");
    //     }
    // }
    
    //TODO Move to AnimationController or something
    private void TriggerCollisionEffect()
    {
        if(playerStats.isInvincible) return;
        StartCoroutine(SetInvincibility(playerStats, animator));
        Debug.Log("Collision!");
        AudioManager.Instance.PlaySound("crash");
        UpdatePlayerHealth(playerStats);
    }
    
    private void IncreaseScore()
    {
        //TODO remove event change to determinScore
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

        TrickEvent evt = Events.TrickEvent;
        evt.Points = amount;
        EventManager.Broadcast(evt);
        //playerStats.ChangeScore(amount);
    }
    
    public int DeterminDamageAmount()
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
        
        return amount;
    }
    
    //TODO move to Animation Controller
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
public enum ObstacleType
{
    Wall,
    BigObstacle,
    SmallObstacle,
    Rail,
    Hole
}
