using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private SpriteRenderer shadowSprite;
    
    private Rigidbody2D _rigidBody;
    private Vector2 _movementInput;
    private SpriteRenderer _playerSprite;

    private bool _isJumping;
    private float _jumpTime;
    private float _initialJumpY;
    private float _shadowSpriteY;
    private float _initialShadowSpriteY;
    private Animator _animator;
    private Animator _shadowAnimator;

    public bool IsJumping => _isJumping;
    public bool disableMovement { get; set; } = false; // This flag will disable movement when true

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _shadowAnimator = shadowSprite.GetComponent<Animator>();
        // ToggleShadowSprite();
    }

    private void FixedUpdate()
    {
        // Prevent movement if disableMovement is true
        if (disableMovement) return;

        // Calculate the new velocity based on input
        _rigidBody.linearVelocity = new Vector2(_movementInput.x * speed, _movementInput.y * speed);

        // Get half of the player's width and height in world units
        float playerHalfWidth = _playerSprite.bounds.extents.x;
        float playerHalfHeight = _playerSprite.bounds.extents.y;

        // Calculate the world bounds of the screen
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        // Clamp the player's position within the screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, bottomLeft.x + playerHalfWidth, topRight.x - playerHalfWidth);
        float clampedY = Mathf.Clamp(transform.position.y, bottomLeft.y + playerHalfHeight, topRight.y - playerHalfHeight);

        // Apply the clamped position
        transform.position = new Vector2(clampedX, clampedY);

        // Handle jumping if the player is currently jumping
        if (_isJumping)
        {
            HandleJump();
        }
        else
        {   
            // TODO: add music track for driving
            // AudioManager.Instance.PlayBackgroundTrack("driving");
        }

        // Example of triggering Flip animation (for debugging purposes)
        if (Input.GetKeyDown("k"))
        {
            AudioManager.Instance.PlaySound("flip");
            _animator.SetTrigger("Flip");
            _shadowAnimator.SetTrigger("Flip");
        }
    }

    private void OnMove(InputValue inputValue)
    {
        // Prevent movement input if disableMovement is true
        if (disableMovement) return;

        _movementInput = inputValue.Get<Vector2>();
    }
    
    private void OnJump()
    {
        // Prevent jumping if disableMovement is true or if the player is already jumping
        if (disableMovement || _isJumping) return;

        AudioManager.Instance.StopBackgroundTrack(); // stops driving sound while in air
        AudioManager.Instance.PlaySound("jump");
        _isJumping = true;
        _jumpTime = 0;
        _initialJumpY = transform.position.y;
        _shadowSpriteY = _initialJumpY - _playerSprite.bounds.extents.y;
        ToggleShadowSprite();
        //ToggleCollider();
    
    }

    private void HandleJump()
    {
        // Calculate jump progress.
        _jumpTime += Time.fixedDeltaTime;
        var progress = _jumpTime / jumpDuration;

        // Sine wave curve for smooth jump.
        var verticalOffset = jumpHeight * Mathf.Sin(Mathf.PI * progress);

        // Apply vertical offset to player.
        transform.position = new Vector3(transform.position.x, _initialJumpY + verticalOffset, transform.position.z);
        // Add y offset to the shadow.
        shadowSprite.transform.position = new Vector3(shadowSprite.transform.position.x, _shadowSpriteY, shadowSprite.transform.position.z);

        // Jump is done.
        if (!(progress >= 1)) return;
        AudioManager.Instance.PlaySound("land");
        _isJumping = false;
        transform.position = new Vector3(transform.position.x, _initialJumpY, transform.position.z);
        ToggleShadowSprite();
        //ToggleCollider();
    }

    private void ToggleShadowSprite()
    {
        shadowSprite.enabled = !shadowSprite.enabled;
    }
    
    /**
     * Toggles collision between player and obstacles on and off.
     */
    private void ToggleCollider()
    {
        GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled;
    }
}