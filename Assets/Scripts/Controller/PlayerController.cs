using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Events;
using Model;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public PlayerView playerView;
    private PlayerModel playerModel;
    private Rigidbody2D rb2d;

    // Movement
    private Vector2 movementInput;
    private float jumpTime;
    private float initialJumpY;
    private bool isAboveRail;

    private const string JUMP_SOUND = "jump";
    private const string LAND_SOUND = "land";
    private const string GRINDING_SOUND = "grinding";
    private const string CRASH_SOUND = "crash";
    private const string SPECIAL_ACTION_SOUND = "specialAction";
    private const string ITEM_SOUND = "item";

    private void Awake()
    {
        playerModel = new PlayerModel();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RegisterEvents();
        BroadcastPlayerStatus();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        HandleGrinding();
        UpdateScore();
    }

    private void RegisterEvents()
    {
        EventManager.AddListener<PlayerEvents.HealthChangedEvent>(OnHealthChanged);
        EventManager.AddListener<PlayerEvents.SpecialChangedEvent>(OnSpecialChanged);
        EventManager.AddListener<PlayerEvents.ScoreChangedEvent>(OnScoreChanged);
        EventManager.AddListener<PlayerEvents.SpeedChangedEvent>(OnSpeedChanged);
        EventManager.AddListener<PlayerEvents.JumpDurationChangedEvent>(OnJumpDurationChanged);
        EventManager.AddListener<PlayerEvents.TrickActionEvent>(OnTrickAction);
        EventManager.AddListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
        EventManager.AddListener<PlayerEvents.ObstacleCollisionEvent>(OnObstacleCollision);
        EventManager.AddListener<PlayerEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        EventManager.AddListener<PlayerEvents.PickupCollisionEvent>(OnPickupCollision);
    }

    private void UnregisterEvents()
    {
        EventManager.RemoveListener<PlayerEvents.HealthChangedEvent>(OnHealthChanged);
        EventManager.RemoveListener<PlayerEvents.SpecialChangedEvent>(OnSpecialChanged);
        EventManager.RemoveListener<PlayerEvents.ScoreChangedEvent>(OnScoreChanged);
        EventManager.RemoveListener<PlayerEvents.SpeedChangedEvent>(OnSpeedChanged);
        EventManager.RemoveListener<PlayerEvents.JumpDurationChangedEvent>(OnJumpDurationChanged);
        EventManager.RemoveListener<PlayerEvents.TrickActionEvent>(OnTrickAction);
        EventManager.RemoveListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
        EventManager.RemoveListener<PlayerEvents.ObstacleCollisionEvent>(OnObstacleCollision);
        EventManager.RemoveListener<PlayerEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        EventManager.RemoveListener<PlayerEvents.PickupCollisionEvent>(OnPickupCollision);
    }

    private void BroadcastPlayerStatus()
    {
        EventManager.Broadcast(new PlayerEvents.ScoreChangedEvent(playerModel.GetScore()));
        EventManager.Broadcast(new PlayerEvents.HealthChangedEvent(playerModel.GetHealth()));
        EventManager.Broadcast(new PlayerEvents.SpecialChangedEvent(playerModel.GetSpecial()));
        EventManager.Broadcast(new PlayerEvents.SpeedChangedEvent(playerModel.GetSpeed()));
        EventManager.Broadcast(new PlayerEvents.JumpDurationChangedEvent(playerModel.GetJumpDuration()));
    }

    private void OnMove(InputValue inputValue)
    {
        if (!playerModel.GetIsGrinding()) 
            movementInput = inputValue.Get<Vector2>();
    }

    private void HandleMovement()
    {
        rb2d.linearVelocity = playerModel.GetVelocity();
    }

    private void HandleInput()
    {
        UpdateMovementInput();
        HandleTrickInput();
        playerView.SetRunning(movementInput.x != 0);
        playerView.UpdateDirection(movementInput.x);
    }

    private void UpdateMovementInput()
    {
        float moveSpeed = playerModel.GetSpeed();
        playerModel.SetVelocity(new Vector2(movementInput.x * moveSpeed, movementInput.y * moveSpeed / 2));
    }

    private void HandleTrickInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            EventManager.Broadcast(new PlayerEvents.TrickActionEvent(1f));
        }
    }

    private void OnJump()
    {
        if (!playerModel.GetIsJumping())
        {
            AudioManager.Instance.StopBackgroundTrack();
            AudioManager.Instance.PlaySound(JUMP_SOUND);
            playerModel.SetIsJumping(true);
            jumpTime = 0;
            initialJumpY = transform.position.y;
            float shadowSpriteHeight = initialJumpY;
            EventManager.Broadcast(new PlayerEvents.JumpEvent(shadowSpriteHeight));
        }
    }

    private void HandleJump()
    {
        if (!playerModel.GetIsJumping()) return;

        UpdateJumpProgress();

        if (IsJumpComplete())
            HandleLanding();
    }

    private void UpdateJumpProgress()
    {
        jumpTime += Time.fixedDeltaTime;
        var progress = jumpTime / playerModel.GetJumpDuration();
        var verticalOffset = playerModel.GetJumpHeight() * Mathf.Sin(Mathf.PI * progress);
        transform.position = new Vector3(transform.position.x, initialJumpY + verticalOffset, transform.position.z);
    }

    private bool IsJumpComplete()
    {
        return jumpTime / playerModel.GetJumpDuration() >= 1;
    }

    private void HandleLanding()
    {
        AudioManager.Instance.PlaySound(LAND_SOUND);
        if (isAboveRail) StartGrinding();
        playerModel.SetIsJumping(false);
    }

    private void StartGrinding()
    {
        Debug.Log("Grinding!");
        playerModel.SetIsGrinding(true);
        AudioManager.Instance.PlaySound(GRINDING_SOUND);
    }

    private void HandleGrinding()
    {
        if (playerModel.GetIsGrinding() && (!playerModel.GetIsJumping() && !isAboveRail))
        {
            FinishGrinding();
        }
    }

    private void FinishGrinding()
    {
        playerModel.SetIsGrinding(false);
    }

    private IEnumerator SetInvincibility()
    {
        playerModel.SetIsInvincible(true);
        yield return new WaitForSeconds(playerModel.GetInvincibilityDuration());
        playerModel.SetIsInvincible(false);
    }

    private void UpdateScore()
    {
        playerModel.SetScore(playerModel.GetScore() + Time.deltaTime); 
    }

    private void OnSpecialAction(PlayerEvents.SpecialActionEvent obj)
    {
        AudioManager.Instance.PlaySound(SPECIAL_ACTION_SOUND);
    }

    private void OnTrickAction(PlayerEvents.TrickActionEvent obj)
    {
        playerModel.IncreaseScore(obj.TrickActionScore);
    }
    
    private void OnObstacleCollision(PlayerEvents.ObstacleCollisionEvent evt)
    {
        Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();

        if (playerModel.GetIsJumping() && obstacle.IsJumpable)
        {
            Debug.Log("Jumped above obstacle");
            playerModel.IncreaseScore(obstacle.DetermineScore());
            return;
        }

        if (obstacle.Type == ObstacleType.Rail)
        {
            isAboveRail = true;
            Debug.Log("Player is over rail!");
        }

        if (!obstacle.IsJumpable || !playerModel.GetIsJumping())
        {
            TriggerCollision(obstacle);
        }
    }

    private void OnObstacleExit(PlayerEvents.ObstacleCollisionExitEvent evt)
    {
        if (playerModel.GetIsInvincible()) return;
        Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();
        int score = obstacle.DetermineScore();
        EventManager.Broadcast(new PlayerEvents.ScoreChangedEvent(score));

        if (obstacle.Type == ObstacleType.Rail)
        {
            isAboveRail = false;
            Debug.Log("Player is no longer over rail!");
        }
    }

    private void TriggerCollision(Obstacle obstacle)
    {
        if (playerModel.GetIsInvincible()) return;

        AudioManager.Instance.PlaySound(CRASH_SOUND);
        StartCoroutine(SetInvincibility());
        int damage = obstacle.DetermineDamageAmount();
        EventManager.Broadcast(new PlayerEvents.HealthChangedEvent(playerModel.GetHealth() + damage));

        if (playerModel.GetHealth() <= 0)
        {
            EventManager.Broadcast(new GameEvents.GameStateChangedEvent(GameModel.GameState.GameOver));
        }
    }

    private void OnPickupCollision(PlayerEvents.PickupCollisionEvent obj)
    {
        TriggerItemEffect(obj.PickupType);
    }

    private void TriggerItemEffect(PickupType pickupType)
    {
        AudioManager.Instance.PlaySound(ITEM_SOUND);
        switch (pickupType)
        {
            case PickupType.HealthBoost:
                EventManager.Broadcast(new PlayerEvents.HealthChangedEvent(playerModel.GetHealth() + 50f));
                break;
            case PickupType.SpecialBoost:
                EventManager.Broadcast(new PlayerEvents.SpecialChangedEvent(playerModel.GetSpecial() + 30f));
                break;
            case PickupType.ScoreBoost:
                EventManager.Broadcast(new PlayerEvents.ScoreChangedEvent(playerModel.GetScore() + 100f));
                break;
            case PickupType.SpeedBoost:
                EventManager.Broadcast(new PlayerEvents.SpeedChangedEvent(playerModel.GetSpeed() + 1f));
                break;
            case PickupType.JumpBoost:
                EventManager.Broadcast(new PlayerEvents.JumpDurationChangedEvent(playerModel.GetJumpDuration() + 1f));
                break;
            default:
                Debug.LogWarning("Unhandled pickup type: {pickupType}");
                break;
        }
    }

    // Event handler methods
    private void OnJumpDurationChanged(PlayerEvents.JumpDurationChangedEvent obj)
    {
        playerModel.SetJumpDuration(obj.NewJumpDuration);
    }

    private void OnSpeedChanged(PlayerEvents.SpeedChangedEvent obj)
    {
        playerModel.SetSpeed(obj.NewSpeed);
    }

    private void OnScoreChanged(PlayerEvents.ScoreChangedEvent obj)
    {
        playerModel.SetScore(obj.NewScore);
    }

    private void OnSpecialChanged(PlayerEvents.SpecialChangedEvent obj)
    {
        playerModel.SetSpecial(obj.NewSpecial);
    }

    private void OnHealthChanged(PlayerEvents.HealthChangedEvent obj)
    {
        playerModel.SetHealth(obj.NewHealth);
    }
}