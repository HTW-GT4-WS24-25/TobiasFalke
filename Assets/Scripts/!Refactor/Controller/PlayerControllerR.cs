using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerR : MonoBehaviour
{
    public PlayerView playerView;
    private PlayerModel playerModel;
    private Rigidbody2D rb2d;
    private Vector2 _movementInput;

    void Awake()
    {
        playerModel = new PlayerModel();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Register events here
        EventManagerR.AddListener<GameEvents.SpecialActionEvent>(OnSpecialAction);
        EventManagerR.AddListener<GameEvents.TrickActionEvent>(OnTrickAction);
        EventManagerR.AddListener<GameEvents.ObstacleCollisionEvent>(OnObstacleCollision);
        EventManagerR.AddListener<GameEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        EventManagerR.AddListener<GameEvents.UpgradeCollisionEvent>(OnUpgradeCollision);

        // initialize UI
        EventManagerR.Broadcast(new GameEvents.ScoreChangedEvent(playerModel.GetScore()));
        EventManagerR.Broadcast(new GameEvents.HealthChangedEvent(playerModel.GetHealth()));
        EventManagerR.Broadcast(new GameEvents.SpecialChangedEvent(playerModel.GetSpecial()));
        EventManagerR.Broadcast(new GameEvents.SpeedChangedEvent(playerModel.GetSpeed()));
        EventManagerR.Broadcast(new GameEvents.JumpDurationChangedEvent(playerModel.GetJumpDuration()));
    }

    private void OnDestroy()
    {
        // Unregister events here
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