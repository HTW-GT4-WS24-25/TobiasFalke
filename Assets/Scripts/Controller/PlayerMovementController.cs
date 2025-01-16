using System.Collections;
using Events;
using Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class PlayerMovementController : MonoBehaviour
    {
        private PlayerModel playerModel;
        private PlayerView playerView;
        private Rigidbody2D rb2d;
        private Vector2 movementInput;
        private float jumpTime;
        private float initialJumpY;
        private bool isAboveRail;

        public void Initialize(PlayerModel model, PlayerView view)
        {
            playerModel = model;
            playerView = view;
            rb2d = GetComponent<Rigidbody2D>();
            RegisterMovementEvents();
        }

        private void RegisterMovementEvents()
        {
            EventManager.AddListener<PlayerEvents.ObstacleCollisionEvent>(OnObstacleCollision);
            EventManager.AddListener<PlayerEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        }

        private void UnregisterMovementEvents()
        {
            EventManager.RemoveListener<PlayerEvents.ObstacleCollisionEvent>(OnObstacleCollision);
            EventManager.RemoveListener<PlayerEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        }

        private void OnDestroy()
        {
            UnregisterMovementEvents();
        }

        public void HandleInput()
        {
            float moveSpeed = playerModel.GetSpeed();
            playerModel.SetVelocity(new Vector2(movementInput.x * moveSpeed, movementInput.y * moveSpeed / 2));
            playerView.SetRunning(movementInput.x != 0);
            playerView.UpdateDirection(movementInput.x);

            if (Input.GetKeyDown("f"))
            {
                EventManager.Broadcast(new PlayerEvents.TrickActionEvent(1f));
            }
        }

        public void UpdateMovement()
        {
            HandleMovement();
            HandleJump();
            HandleGrinding();
        }

        private void HandleMovement()
        {
            rb2d.linearVelocity = playerModel.GetVelocity();
        }

        private void HandleJump()
        {
            if (!playerModel.GetIsJumping()) return;

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
            Debug.Log("Grinding!");
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

        private void OnMove(InputValue inputValue)
        {
            if (!playerModel.GetIsGrinding()) movementInput = inputValue.Get<Vector2>();
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
                float shadowSpriteHeight = initialJumpY;
                EventManager.Broadcast(new PlayerEvents.JumpEvent(shadowSpriteHeight));
            }
        }

        private void OnObstacleCollision(PlayerEvents.ObstacleCollisionEvent evt)
        {
            Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();

            if (playerModel.GetIsJumping() && obstacle.IsJumpable)
            {
                playerModel.IncreaseScore(obstacle.DetermineScore());
            }

            if (obstacle.Type == ObstacleType.Rail)
            {
                isAboveRail = true;
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
            EventManager.Broadcast(new PlayerEvents.ScoreChanged(score));
            if (obstacle.Type == ObstacleType.Rail)
            {
                isAboveRail = false;
            }
        }

        private void TriggerCollision(Obstacle obstacle)
        {
            if (playerModel.GetIsInvincible()) return;
            AudioManager.Instance.PlaySound("crash");
            StartCoroutine(SetInvincibility());
            int damage = obstacle.DetermineDamageAmount();
            EventManager.Broadcast(new PlayerEvents.HealthChanged(playerModel.GetHealth() + damage));
            if (playerModel.GetHealth() <= 0)
            {
                EventManager.Broadcast(new GameModel.GameStateChanged(GameModel.GameState.Loose));
            }
        }

        private IEnumerator SetInvincibility()
        {
            playerModel.SetIsInvincible(true);
            yield return new WaitForSeconds(playerModel.GetInvincibilityDuration());
            playerModel.SetIsInvincible(false);
        }
    }
}