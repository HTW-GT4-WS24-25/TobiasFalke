using UnityEngine;

public class ObstacleManger : MonoBehaviour
{
    public bool ObstacleJumpable;
    public bool ObstacleGrindable;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (ObstacleJumpable && player.GetComponent<PlayerMovement>().IsJumping)
                {
                    // If the obstacle is jumpable and the player is jumping, do nothing
                    return;
                }

                Destroy(gameObject);

                // Trigger the collision effect
                player.TriggerCollisionEffect();
            }
        }
    }
}
