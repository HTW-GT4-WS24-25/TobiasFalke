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
    private float _intialShadowSpriteY;
    private Animator _animator;
    private Animator _shadowAnimator;

    public bool IsJumping => _isJumping;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponent<SpriteRenderer>();
        //ToggleShadowSprite();
        _animator = GetComponent<Animator>();
        _shadowAnimator = shadowSprite.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = new Vector2(_movementInput.x * speed, _movementInput.y * speed);

        if (_isJumping)
        {
            HandleJump();
        }

        //TODO: just an example of triggering Flip
        if (Input.GetKeyDown("k"))
        {
            _animator.SetTrigger("Flip");
            _shadowAnimator.SetTrigger("Flip");
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
    
    private void OnJump()
    {
        if (!_isJumping)
        {
            _isJumping = true;
            _jumpTime = 0;
            _initialJumpY = transform.position.y;
            _shadowSpriteY = _initialJumpY - _playerSprite.bounds.extents.y;
            ToggleShadowSprite();
            //ToggleCollider();
        }
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