using System;
using System.Collections;
using Events;
using Model;
using UnityEngine;
using Utility;
using static Utility.GameConstants;

namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovementController movementController;
        private PlayerModel playerModel;

        private void Awake()
        {
            movementController = GetComponent<PlayerMovementController>();
            Debug.Log("player" +GameConfig.Instance);
            playerModel = new PlayerModel
            {
                MaxHealthPoints = GameConfig.Instance.MaxHealthPoints,
                HealthPoints = GameConfig.Instance.MaxHealthPoints,
                MaxSpecialPoints = GameConfig.Instance.MaxSpecialPoints,
                SpecialPoints = 0,
                Speed = GameConfig.Instance.BaseSpeed,
                MaxSpeedMultiplier = GameConfig.Instance.MaxSpeedMultiplier,
                JumpHeight = GameConfig.Instance.JumpHeight,
                JumpDuration = GameConfig.Instance.JumpDuration,
                GrindActionScore = GameConfig.Instance.GrindActionScore,
                TrickActionScore = GameConfig.Instance.TrickActionScore,
                TrickActionDuration = GameConfig.Instance.TrickActionDuration,
                SpecialActionDuration = GameConfig.Instance.SpecialActionDuration,
                InvincibilityDuration = GameConfig.Instance.InvincibilityDuration
            };
            Debug.Log("health points now" + playerModel.HealthPoints);
        }

        private void Start()
        {
            movementController.Initialize(playerModel);
            RegisterEvents();
        }
        
        private void RegisterEvents()
        {
            EventManager.Add<PlayerEvent.ObstacleCollision>(OnObstacleCollision);
            EventManager.Add<PlayerEvent.ObstacleEvasion>(OnObstacleEvasion);
            EventManager.Add<PlayerEvent.PickupCollision>(OnPickupCollision);
        }
        
        private void Update()
        {
            UpdateScore();
        }
        
        private void UpdateScore()
        {
            playerModel.ScorePoints += Time.deltaTime * 10 * playerModel.ScoreMultiplier;
        }

        private void OnSpecialAction()
        {
            if (playerModel.SpecialPoints >= playerModel.MaxSpecialPoints)
            {
                AudioManager.Instance.PlaySound(Audio.SpecialActionSFX);
                playerModel.IsDoingSpecialAction = true;
                playerModel.IsInvincible = true;
                StartCoroutine(InvincibilityActive(playerModel.SpecialActionDuration));
            }
        }
        
        private void OnObstacleCollision(PlayerEvent.ObstacleCollision evt)
        {
            Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();
            if (playerModel.IsDoingJumpAction && obstacle.canJumpOver) playerModel.ScorePoints += obstacle.DetermineScore();
            if (obstacle.Type == ObstacleType.Rail) playerModel.IsAboveRail = true;
            if (!obstacle.canJumpOver || !playerModel.IsDoingJumpAction) TriggerCollision(obstacle);
        }
        
        private void TriggerCollision(Obstacle obstacle)
        {
            if (playerModel.IsInvincible) return;
            AudioManager.Instance.PlaySound(Audio.WallCollisionSFX);
            int collisionDamage = obstacle.DetermineDamageAmount();
            playerModel.HealthPoints -= collisionDamage;
            playerModel.IsInvincible = true;
            StartCoroutine(InvincibilityActive(playerModel.InvincibilityDuration));
            if (playerModel.HealthPoints <= 0)
            {
                AudioManager.Instance.PlaySound(Audio.GameOverSFX);
                EventManager.Trigger(new PlayerEvent.GameOverTriggered(playerModel.ScorePoints));
            }
        }

        private IEnumerator InvincibilityActive(float duration)
        {
            yield return new WaitForSeconds(duration);
            playerModel.IsInvincible = false;
        }

        private void OnObstacleEvasion(PlayerEvent.ObstacleEvasion evt)
        {
            Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();
            if (obstacle.Type == ObstacleType.Rail) playerModel.IsAboveRail = false;
            if (playerModel.IsInvincible) return;
            int evasionScore = obstacle.DetermineScore();
            playerModel.ScorePoints += evasionScore;
        }
        
        private void OnPickupCollision(PlayerEvent.PickupCollision evt)
        {
            PickupType pickupType = evt.PickupType;
            TriggerItemEffect(pickupType);
        }

        private void TriggerItemEffect(PickupType pickupType)
        {
            AudioManager.Instance.PlaySound(Audio.PickupCollision1SFX);
            switch (pickupType)
            {
                case PickupType.HealthBoost:
                    playerModel.HealthPoints += 50;
                    break;
                case PickupType.SpecialBoost:
                    playerModel.SpecialPoints += 25;
                    break;
                case PickupType.ScoreBoost:
                    playerModel.ScorePoints += Mathf.Max(100, playerModel.ScorePoints / 16) * playerModel.ScoreMultiplier;
                    break;
                case PickupType.SpeedBoost:
                    playerModel.SpeedMultiplier += 0.25f;
                    break;
                case PickupType.HealthBoom:
                    playerModel.HealthPoints += 100;
                    break;
                case PickupType.SpecialBoom:
                    playerModel.SpecialPoints += 50;
                    break;
                case PickupType.SpeedBoom:
                    playerModel.SpeedMultiplier += 0.5f;
                    break;
                case PickupType.ScoreBoom:
                    playerModel.ScorePoints += Mathf.Max(200, playerModel.ScorePoints / 8);
                    break;
                case PickupType.ScoreMultiplierBoost:
                    playerModel.SpeedMultiplier += 0.25f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pickupType), pickupType, null);
            }
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
        
        private void UnsubscribeEvents()
        {
            EventManager.Remove<PlayerEvent.ObstacleCollision>(OnObstacleCollision);
            EventManager.Remove<PlayerEvent.ObstacleEvasion>(OnObstacleEvasion);
            EventManager.Remove<PlayerEvent.PickupCollision>(OnPickupCollision);
        }
    }
}