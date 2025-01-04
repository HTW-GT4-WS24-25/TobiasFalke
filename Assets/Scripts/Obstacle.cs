using UnityEngine;

public class Obstacle : MonoBehaviour, IObject
{
    public bool canJumpOver;
    public bool canGrindOver;

    public void Collide(GameObject obstacle, PlayerStats playerStats, PlayerMovement playerMovement)
    {
        // start grinding over obstacle if possible
        if (canGrindOver)
        {
            HandleGrindCollision(playerStats);
        }
        // trigger collision if not grinding or jumping against non-jump-able object
        else if (!canJumpOver || !playerMovement.IsJumping)
        {
            TriggerCollisionEffect(playerStats);
        }
    }
    
    private void HandleGrindCollision(PlayerStats playerStats)
    {
        Debug.Log("Player is grinding over an obstacle.");
        // TODO: implement logic for grinding
        // TODO: make continuous while grinding:
        playerStats.UpdateSpecial(100);
    }
    
    private void TriggerCollisionEffect(PlayerStats playerStats)
    {
        Debug.Log("Collision effect triggered.");
        // TODO: make damage depend on type of obstacle?
        playerStats.UpdateHealth(-20);
        // Implement collision effect, such as animation or sound
        // TODO: add animation to player when colliding
        AudioManager.Instance.PlaySound("collision");
    }
}
