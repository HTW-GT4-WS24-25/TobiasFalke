using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpDuration = 0.5f;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;

    private bool _isJumping = false;
    private float _jumpTime;
    private float _initialJumpY;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector2(_movementInput.x * speed, _movementInput.y * speed);

        if (_isJumping)
        {
            HandleJump();
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

        // Jump is done
        if (!(progress >= 1)) return;
        _isJumping = false;
        transform.position = new Vector3(transform.position.x, _initialJumpY, transform.position.z);
    }
}