using UnityEngine;

public class PlayerModel : MonoBehaviour, IMovable
{
    // Player attributes
    public int Health { get; private set; }
    public int Score { get; private set; }
    public float Speed { get; set; } = 5f;

    private Rigidbody2D rb2d;

    void Awake()
    {
        Health = 100; // Initialize health
        Score = 0; // Initialize score
        rb2d = GetComponent<Rigidbody2D>(); // Ensure a Rigidbody2D component is attached
    }

    // Method to increase the player's health
    public void IncreaseHealth(int amount)
    {
        Health += amount;
    }

    // Method to increase the player's score
    public void IncreaseScore(int points)
    {
        Score += points;
    }

    // Implementing the IMovable interface's Move method
    public void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 newVelocity = new Vector2(horizontalInput * Speed, rb2d.linearVelocity.y);
        rb2d.linearVelocity = newVelocity;
    }

    // Call Move in FixedUpdate for consistent physics-based movement
    void FixedUpdate()
    {
        Move();
    }

    // Handle collisions with other game objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.OnCollect(this);
        }
        // Additional handling for other interactions (like obstacles) can be added here
    }
}