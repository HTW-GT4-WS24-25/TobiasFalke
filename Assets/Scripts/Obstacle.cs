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
        Debug.Log("Grinding!");
        // TODO: implement logic for grinding -> Coroutine?
        AudioManager.Instance.PlaySound("grind");
        playerStats.UpdateSpecial(15); // TODO: adjust based on balancing
    }
    
    private void TriggerCollisionEffect(PlayerStats playerStats)
    {
        Debug.Log("Collision!");
        playerStats.UpdateHealth(-20); // TODO: make damage depend on type of obstacle?
        AudioManager.Instance.PlaySound("crash");
        // TODO: add animation to player when colliding
        // TODO: have player phrase through obstacle or be pushed below the screen (game over)
    }
}
