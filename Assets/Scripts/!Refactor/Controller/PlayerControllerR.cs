using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerR : MonoBehaviour
{
    public PlayerView playerView;
    private PlayerModel playerModel;
    private Rigidbody2D rb2d;
    private Vector2 _movementInput;
    private float jumpTime;
    private float initialJumpY;
    private bool isAboveRail;

    void Awake()
    {
        playerModel = new PlayerModel();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RegisterEvents();
        BroadcastPlayerStatus();
    }
    
    private void RegisterEvents()
    {
        EventManagerR.AddListener<GameEvents.SpecialActionEvent>(OnSpecialAction);
        EventManagerR.AddListener<GameEvents.TrickActionEvent>(OnTrickAction);
        EventManagerR.AddListener<GameEvents.ObstacleCollisionEvent>(OnObstacleCollision);
        EventManagerR.AddListener<GameEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        EventManagerR.AddListener<GameEvents.UpgradeCollisionEvent>(OnUpgradeCollision);
    }
    
    private void BroadcastPlayerStatus()
    {
        EventManagerR.Broadcast(new GameEvents.ScoreChangedEvent(playerModel.GetScore()));
        EventManagerR.Broadcast(new GameEvents.HealthChangedEvent(playerModel.GetHealth()));
        EventManagerR.Broadcast(new GameEvents.SpecialChangedEvent(playerModel.GetSpecial()));
        EventManagerR.Broadcast(new GameEvents.SpeedChangedEvent(playerModel.GetSpeed()));
        EventManagerR.Broadcast(new GameEvents.JumpDurationChangedEvent(playerModel.GetJumpDuration()));
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }
    
    private void UnregisterEvents()
    {
        EventManagerR.RemoveListener<GameEvents.SpecialActionEvent>(OnSpecialAction);
        EventManagerR.RemoveListener<GameEvents.TrickActionEvent>(OnTrickAction);
        EventManagerR.RemoveListener<GameEvents.ObstacleCollisionEvent>(OnObstacleCollision);
        EventManagerR.RemoveListener<GameEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        EventManagerR.RemoveListener<GameEvents.UpgradeCollisionEvent>(OnUpgradeCollision);
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        UpdateScore();
        HandleJump();
        HandleGrinding();
    }
    
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
    
    private void HandleInput()
    {
        float moveSpeed = playerModel.GetSpeed();
        playerModel.SetVelocity(new Vector2(_movementInput.x * moveSpeed, _movementInput.y * moveSpeed / 2));
        playerView.SetRunning(_movementInput.x != 0);
        playerView.UpdateDirection(_movementInput.x);
    }
    
    private void OnJump()
    {
        if (!playerModel.GetIsJumping())
        {
            AudioManager.Instance.StopBackgroundTrack(); // stops driving sound while in air.
            AudioManager.Instance.PlaySound("jump");
            playerModel.SetIsJumping(true);
            jumpTime = 0;
            initialJumpY = transform.position.y;
            // _shadowSpriteY = _initialJumpY - _playerSprite.bounds.extents.y;
            // ToggleShadowSprite();
        }
    }

    private void HandleJump()
    {
        if (!playerModel.GetIsJumping())
        {
            // TODO: add music track for driving
            // AudioManager.Instance.PlayBackgroundTrack("driving");
            return;
        }
        
        jumpTime += Time.fixedDeltaTime;
        var progress = jumpTime / playerModel.GetJumpDuration();
        var verticalOffset = playerModel.GetJumpHeight() * Mathf.Sin(Mathf.PI * progress);
        transform.position = new Vector3(transform.position.x, initialJumpY + verticalOffset, transform.position.z);
        // shadowSprite.transform.position = new Vector3(shadowSprite.transform.position.x, _shadowSpriteY, shadowSprite.transform.position.z);
        if (!(progress >= 1)) return;
        HandleLanding();
    }
    
    private void HandleLanding()
    {
        AudioManager.Instance.PlaySound("land");
        if (isAboveRail) StartGrinding();
        playerModel.SetIsJumping(false);
        transform.position = new Vector3(transform.position.x, initialJumpY, transform.position.z);
        // ToggleShadowSprite();
    }
    
    private void StartGrinding()
    {
        playerModel.SetIsGrinding(true);
        AudioManager.Instance.PlaySound("grinding");
        transform.Rotate(0, 0, 10);
    }
    
    private void HandleGrinding()
    {
        if (!playerModel.GetIsGrinding()) return;
        if (playerModel.GetIsJumping() || !isAboveRail)
        {
            FinishGrinding();
        }
    }
    
    private void FinishGrinding()
    {
        playerModel.SetIsGrinding(false);
        transform.Rotate(0, 0, -10);
    }


    private void MovePlayer()
    {
        rb2d.linearVelocity = playerModel.GetVelocity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Upgrade"))
        {
            Debug.Log("Upgrade collected!");
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle hit!");
        }
    }

    void UpdateScore()
    {
        playerModel.IncreaseScore(Time.deltaTime);
    }

    private void OnSpecialAction(GameEvents.SpecialActionEvent obj)
    {
        // Handle special action
    }

    private void OnTrickAction(GameEvents.TrickActionEvent obj)
    {
        playerModel.SetScore(playerModel.GetScore() + obj.Points);
    }

    private void OnObstacleExit(GameEvents.ObstacleCollisionExitEvent obj)
    {
        // Handle obstacle exit
    }

    private void OnObstacleCollision(GameEvents.ObstacleCollisionEvent obj)
    {
        // Handle obstacle collision
    }

    private void OnUpgradeCollision(GameEvents.UpgradeCollisionEvent obj)
    {
        // Handle upgrade collision
    }
}