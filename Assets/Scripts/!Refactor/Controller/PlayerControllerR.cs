using UnityEngine;

public class PlayerControllerR : MonoBehaviour
{
    // Reference to the Player's model and view
    private Player playerModel;
    private Rigidbody2D rb2d;  // Assuming you're using Rigidbody2D for physics

    public float moveSpeed = 5f;

    void Awake()
    {
        // Initialize the model and get necessary components
        playerModel = new Player();  // Assume a simple Player model with relevant properties
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle input in Update to ensure responsiveness
        HandleInput();
    }

    private void HandleInput()
    {
        // Example for horizontal movement, you can expand on this for other controls
        float horizontalInput = Input.GetAxis("Horizontal");

        // Update the player model state based on input
        playerModel.Velocity = new Vector2(horizontalInput * moveSpeed, rb2d.linearVelocity.y);

        // More input handling logic can go here (e.g., jump, special abilities)
    }

    void FixedUpdate()
    {
        // Apply the movement logic here using the physics engine for smoother movement
        MovePlayer();
    }

    private void MovePlayer()
    {
        rb2d.linearVelocity = new Vector2(playerModel.Velocity.x, rb2d.linearVelocity.y);
    }

    // Example collision handling where the player might pick up upgrades or hit obstacles
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Upgrade"))
        {
            // Handle upgrade logic, probably interacting with player model
            Debug.Log("Upgrade collected!");
            // Implement upgrade application logic

            Destroy(collision.gameObject);  // Destroy or deactivate the upgrade
        }
        else if (collision.CompareTag("Obstacle"))
        {
            // Handle collision with obstacles, possibly reducing health or ending the game
            Debug.Log("Obstacle hit!");
            // Implement collision handling logic
        }
    }
}

public class Player
{
    public Vector2 Velocity { get; set; }

    // Additional player properties and methods would go here
    // For example, health, score, and methods to increase these values
}