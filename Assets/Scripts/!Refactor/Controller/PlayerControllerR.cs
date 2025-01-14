using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerR : MonoBehaviour
{
    public PlayerView playerView;
    private PlayerModel playerModel;
    
    // movement
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
        EventManagerR.AddListener<PlayerEvents.TrickActionEventR>(OnTrickAction);
        EventManagerR.AddListener<PlayerEvents.SpecialActionEventR>(OnSpecialAction);
        EventManagerR.AddListener<PlayerEvents.ObstacleCollisionEventR>(OnObstacleCollision);
        EventManagerR.AddListener<PlayerEvents.ObstacleCollisionExitEventR>(OnObstacleExit);
        EventManagerR.AddListener<PlayerEvents.PickupCollisionEventR>(OnPickupCollision);
    }

    private void BroadcastPlayerStatus()
    {
        EventManagerR.Broadcast(new PlayerEvents.ScoreChangedEventR(playerModel.GetScore()));
        EventManagerR.Broadcast(new PlayerEvents.HealthChangedEventR(playerModel.GetHealth()));
        EventManagerR.Broadcast(new PlayerEvents.SpecialChangedEventR(playerModel.GetSpecial()));
        EventManagerR.Broadcast(new PlayerEvents.SpeedChangedEventR(playerModel.GetSpeed()));
        EventManagerR.Broadcast(new PlayerEvents.JumpDurationChangedEventR(playerModel.GetJumpDuration()));
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        EventManagerR.RemoveListener<PlayerEvents.TrickActionEventR>(OnTrickAction);
        EventManagerR.RemoveListener<PlayerEvents.SpecialActionEventR>(OnSpecialAction);
        EventManagerR.RemoveListener<PlayerEvents.ObstacleCollisionEventR>(OnObstacleCollision);
        EventManagerR.RemoveListener<PlayerEvents.ObstacleCollisionExitEventR>(OnObstacleExit);
        EventManagerR.RemoveListener<PlayerEvents.PickupCollisionEventR>(OnPickupCollision);
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

    private void OnMove(InputValue inputValue)
    {
        if (!playerModel.GetIsGrinding()) _movementInput = inputValue.Get<Vector2>();
    }
    
    private void HandleMovement()
    {
        rb2d.linearVelocity = playerModel.GetVelocity();
    }
    
    private void OnJump()
    {
        if (!playerModel.GetIsJumping())
        {
            AudioManagerR.Instance.StopBackgroundTrack();
            AudioManagerR.Instance.PlaySound("jump");
            playerModel.SetIsJumping(true);
            jumpTime = 0;
            initialJumpY = transform.position.y;
            float shadowSpriteHeight = initialJumpY;
            EventManagerR.Broadcast(new PlayerEvents.JumpEventR(shadowSpriteHeight));
        }
    }

    private void HandleInput()
    {
        // TODO: overhaul input management
        float moveSpeed = playerModel.GetSpeed();
        playerModel.SetVelocity(new Vector2(_movementInput.x * moveSpeed, _movementInput.y * moveSpeed / 2));
        playerView.SetRunning(_movementInput.x != 0);
        playerView.UpdateDirection(_movementInput.x);
        // TODO: implement trigger via input manager
        if (Input.GetKeyDown("k"))
        {
            EventManagerR.Broadcast(new PlayerEvents.TrickActionEventR());
        }
    }
    
    private void HandleJump()
    {
        if (!playerModel.GetIsJumping())
        {
            return;
        }

        jumpTime += Time.fixedDeltaTime;
        var progress = jumpTime / playerModel.GetJumpDuration();
        var verticalOffset = playerModel.GetJumpHeight() * Mathf.Sin(Mathf.PI * progress);
        transform.position = new Vector3(transform.position.x, initialJumpY + verticalOffset, transform.position.z);
        if (progress >= 1) HandleLanding();
    }

    private void HandleLanding()
    {
        AudioManagerR.Instance.PlaySound("land");
        if (isAboveRail) StartGrinding();
        playerModel.SetIsJumping(false);
    }

    private void StartGrinding()
    {
        Debug.Log("Grinding!");
        playerModel.SetIsGrinding(true);
        AudioManagerR.Instance.PlaySound("grinding");
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
    }
    
    private void OnObstacleCollision(PlayerEvents.ObstacleCollisionEventR evt)
    {
        Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();

        if (playerModel.GetIsJumping() && obstacle.IsJumpable)
        {
            Debug.Log("Jumped above obstacle");
            playerModel.IncreaseScore(obstacle.DetermineScore());
        }

        if (obstacle.Type == ObstacleType.Rail)
        {
            isAboveRail = true;
            Debug.Log("Player is over rail!");
        }

        if (!obstacle.IsJumpable || !playerModel.GetIsJumping())
        {
            TriggerCollisionEffect(obstacle);
        }
    }

    private void OnObstacleExit(PlayerEvents.ObstacleCollisionExitEventR evt)
    {
        Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();
        if (obstacle.Type == ObstacleType.Rail)
        {
            isAboveRail = false;
            Debug.Log("Player is no longer over rail!");
        }
    }

    private void TriggerCollisionEffect(Obstacle obstacle)
    {
        if (playerModel.GetIsInvincible()) return;

        StartCoroutine(SetInvincibility());
        AudioManagerR.Instance.PlaySound("crash");
        int damage = obstacle.DetermineDamageAmount();
        EventManagerR.Broadcast(new PlayerEvents.HealthChangedEventR(damage));
        if (playerModel.GetHealth() <= 0)
        {
            // TODO:  EventManagerR.Broadcast(new GameEvents.TriggerGameOver());
        }
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
        EventManagerR.Broadcast(new PlayerEvents.ScoreChangedEventR(playerModel.GetScore() + Time.deltaTime));
    }

    private void OnSpecialAction(PlayerEvents.SpecialActionEventR obj)
    {
        // Trigger status changes of special action.
        // stats.TriggerSpecialAction(); // TODO: make special action stat raise temporary (6 seconds (?))
        EventManagerR.Broadcast(new PlayerEvents.SpecialActionEventR());
        AudioManagerR.Instance.PlaySound("specialAction");
    }

    private void OnTrickAction(PlayerEvents.TrickActionEventR obj)
    {
        playerModel.IncreaseScore(obj.Points);
    }

    private void OnPickupCollision(PlayerEvents.PickupCollisionEventR obj)
    {
        AudioManagerR.Instance.PlaySound("item");
        // TODO: implement item effect
        // TriggerItemEffect(stats,evt.ItemType);
        // if (itemEffects.TryGetValue(type, out IItemEffect effect)) {effect.ApplyEffect(playerStats); }
    }
}