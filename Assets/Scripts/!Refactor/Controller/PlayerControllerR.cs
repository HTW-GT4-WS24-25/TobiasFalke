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

    private void OnObstacleCollision(GameEvents.ObstacleCollisionEvent evt)
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

    private void OnObstacleExit(GameEvents.ObstacleCollisionExitEvent evt)
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
        EventManagerR.Broadcast(new GameEvents.HealthChangedEvent(damage));
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

    private void OnSpecialAction(GameEvents.SpecialActionEvent obj)
    {
        // Handle special action
    }

    private void OnTrickAction(GameEvents.TrickActionEvent obj)
    {
        playerModel.SetScore(playerModel.GetScore() + obj.Points);
    }

    private void OnUpgradeCollision(GameEvents.UpgradeCollisionEvent obj)
    {
        // Handle upgrade collision
    }
}