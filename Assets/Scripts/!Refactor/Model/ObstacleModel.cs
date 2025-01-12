using UnityEngine;

public class ObstacleModel : MonoBehaviour, IMovable
{
    public float Speed { get; set; } = 3f;

    void Update()
    {
        Move();
    }

    // Implementing the IMovable interface
    public void Move()
    {
        transform.Translate(Vector3.down * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // Handle collision with the player, e.g., reduce health or trigger game over
            Debug.Log("Player hit by obstacle!");
            // Example: player.DecreaseHealth(10);
        }
    }
}