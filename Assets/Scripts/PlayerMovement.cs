using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5;
    public float baseJumpDuration = 1.0f;
    [SerializeField] public float speed = 5;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] public float jumpDuration = 0.5f;
    
    private Vector2 _movementInput;
    
    [SerializeField] public SpriteRenderer _playerSprite;
    [SerializeField] private SpriteRenderer shadowSprite;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private Animator _animator;
    
    private Animator _shadowAnimator;

    private bool _isJumping;
    private bool _isGrinding;
    private bool _isOverRail;
    
    private float _jumpTime;
    private float _initialJumpY;
    private float _shadowSpriteY;
    private float _initialShadowSpriteY;

    public bool IsJumping => _isJumping;
    public bool IsGrinding => _isGrinding;
    public bool SetIsOverRail
    {
        set => _isOverRail = value;
    }

    private void Awake()
    {
        _shadowAnimator = shadowSprite.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Updating player velocity, position, and handling jump/grind logic.
        UpdateVelocity();
        UpdatePlayerPosition();
        HandleJump();
        HandleGrinding();

        // Example of triggering Flip animation (for debugging purposes).
        if (Input.GetKeyDown("k"))
        {
            AudioManager.Instance.PlaySound("flip");
            _animator.SetTrigger("Flip");
            _shadowAnimator.SetTrigger("Flip");
        }
    }

    private void OnMove(InputValue inputValue)
    {
        if (!_isGrinding) _movementInput = inputValue.Get<Vector2>();
    }
    
    private void OnJump()
    {
        if (!_isJumping)
        {
            AudioManager.Instance.StopBackgroundTrack(); // stops driving sound while in air.
            AudioManager.Instance.PlaySound("jump");
            _isJumping = true;
            _jumpTime = 0;
            _initialJumpY = transform.position.y;

           

            _shadowSpriteY = _initialJumpY - _playerSprite.bounds.extents.y;
            ToggleShadowSprite();
        }
    }

    private void HandleJump()
    {
        if (!_isJumping)
        {
            // TODO: add music track for driving
            // AudioManager.Instance.PlayBackgroundTrack("driving");
            return;
        }
        
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
        HandleLanding();
    }

    private void HandleLanding()
    {
        AudioManager.Instance.PlaySound("land");
        if (_isOverRail) StartGrinding();
        _isJumping = false;
        transform.position = new Vector3(transform.position.x, _initialJumpY, transform.position.z);
        ToggleShadowSprite();
    }
    
    private void HandleGrinding()
    {
        if (!_isGrinding) return;
        if (_isJumping || !_isOverRail)
        {
            FinishGrinding();
        }
    }

    private void ToggleShadowSprite()
    {
        shadowSprite.enabled = !shadowSprite.enabled;
    }
    
    private void StartGrinding()
    {
        Debug.Log("Started grinding!");
        _isGrinding = true;
        
        AudioManager.Instance.PlaySound("grinding");
        
        // Just rotate player sprite for now.
        transform.Rotate(0, 0, 10);
    }

    private void FinishGrinding()
    {
        Debug.Log("Finished grinding!");
        _isGrinding = false;
        transform.Rotate(0, 0, -10);
    }

    private void UpdateVelocity()
    {
        var movementX = _movementInput.x * speed;
        var movementY = _movementInput.y * speed;
        if (_isGrinding)
        {
            movementX = 0f;
            movementY = 0f;
        }
        _rigidBody.linearVelocity = new Vector2(movementX, movementY);
    }

    private void UpdatePlayerPosition()
    {
        if (_isGrinding) return;
        // Get half of the player's width and height in world units.
        var playerHalfWidth = _playerSprite.bounds.extents.x;
        var playerHalfHeight = _playerSprite.bounds.extents.y;

        // Calculate the world bounds of the screen.
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        // Clamp the player's position within the screen bounds.
        var clampedX = Mathf.Clamp(transform.position.x, bottomLeft.x + playerHalfWidth, topRight.x - playerHalfWidth);
        var clampedY = Mathf.Clamp(transform.position.y, bottomLeft.y + playerHalfHeight, topRight.y - playerHalfHeight);

        // Apply the clamped position.
        transform.position = new Vector2(clampedX, clampedY);
    }
}