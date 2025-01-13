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
        EventManagerR.AddListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
        EventManagerR.AddListener<PlayerEvents.TrickActionEvent>(OnTrickAction);
        EventManagerR.AddListener<PlayerEvents.ObstacleCollisionEvent>(OnObstacleCollision);
        EventManagerR.AddListener<PlayerEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        EventManagerR.AddListener<PlayerEvents.PickupCollisionEvent>(OnUpgradeCollision);
    }

    private void BroadcastPlayerStatus()
    {
        EventManagerR.Broadcast(new PlayerEvents.ScoreChangedEvent(playerModel.GetScore()));
        EventManagerR.Broadcast(new PlayerEvents.HealthChangedEvent(playerModel.GetHealth()));
        EventManagerR.Broadcast(new PlayerEvents.SpecialChangedEvent(playerModel.GetSpecial()));
        EventManagerR.Broadcast(new PlayerEvents.SpeedChangedEvent(playerModel.GetSpeed()));
        EventManagerR.Broadcast(new PlayerEvents.JumpDurationChangedEvent(playerModel.GetJumpDuration()));
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        EventManagerR.RemoveListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
        EventManagerR.RemoveListener<PlayerEvents.TrickActionEvent>(OnTrickAction);
        EventManagerR.RemoveListener<PlayerEvents.ObstacleCollisionEvent>(OnObstacleCollision);
        EventManagerR.RemoveListener<PlayerEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        EventManagerR.RemoveListener<PlayerEvents.PickupCollisionEvent>(OnUpgradeCollision);
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
            AudioManager.Instance.StopBackgroundTrack();
            AudioManager.Instance.PlaySound("jump");
            playerModel.SetIsJumping(true);
            jumpTime = 0;
            initialJumpY = transform.position.y;
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
        AudioManager.Instance.PlaySound("land");
        if (isAboveRail) StartGrinding();
        playerModel.SetIsJumping(false);
    }

    private void StartGrinding()
    {
        playerModel.SetIsGrinding(true);
        AudioManager.Instance.PlaySound("grinding");
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

    private void MovePlayer()
    {
        rb2d.linearVelocity = playerModel.GetVelocity();
    }

    private void OnObstacleCollision(PlayerEvents.ObstacleCollisionEvent evt)
    {
        Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();
        Debug.Log("Collision is triggered.");

        if (playerModel.GetIsJumping() && obstacle.IsJumpable)
        {
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

    private void OnObstacleExit(PlayerEvents.ObstacleCollisionExitEvent evt)
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
        Debug.Log("Collision!");
        AudioManager.Instance.PlaySound("crash");
        int damage = obstacle.DetermineDamageAmount();
        EventManagerR.Broadcast(new PlayerEvents.HealthChangedEvent(damage));
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
        playerModel.IncreaseScore(Time.deltaTime);
    }

    private void OnSpecialAction(PlayerEvents.SpecialActionEvent obj)
    {
        // Handle special action
    }

    private void OnTrickAction(PlayerEvents.TrickActionEvent obj)
    {
        playerModel.SetScore(playerModel.GetScore() + obj.Points);
    }

    private void OnUpgradeCollision(PlayerEvents.PickupCollisionEvent obj)
    {
        // Handle upgrade collision
    }
}