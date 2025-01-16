using System;
using System.Collections;
using Events;
using Model;
using UnityEngine;
using Config;


namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovementController movementController;
        private PlayerModel playerModel;
        private InputManager playerInput;

        private void Awake()
        {
            movementController = GetComponent<PlayerMovementController>();
            playerModel = new PlayerModel
            {
                HealthPoints = PlayerConfig.MaxHealthPoints,
                SpecialPoints = PlayerConfig.MaxSpecialPoints,
                Speed = PlayerConfig.BaseSpeed,
                JumpHeight = PlayerConfig.JumpHeight,
                JumpDuration = PlayerConfig.JumpDuration,
                GrindActionScore = PlayerConfig.GrindActionScore,
                TrickActionScore = PlayerConfig.TrickActionScore,
                TrickActionDuration = PlayerConfig.TrickActionDuration,
                SpecialActionDuration = PlayerConfig.SpecialActionDuration,
                InvincibilityDuration = PlayerConfig.InvincibilityDuration
            };
            movementController.Initialize(playerModel);
            playerInput = InputManager.Instance;
        }

        private void Start()
        {
            RegisterEvents();
        }
        
        private void RegisterEvents()
        {
            EventManager.AddListener<PlayerEvent.TrickActionTriggered>(OnTrickAction);
            EventManager.AddListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.AddListener<PlayerEvent.ObstacleCollision>(OnObstacleCollision);
            EventManager.AddListener<PlayerEvent.ObstacleEvasion>(OnObstacleEvasion);
            EventManager.AddListener<PlayerEvent.PickupCollision>(OnPickupCollision);
        }
        
        private void Update()
        {
            UpdateScore();
        }
        
        private void UpdateScore()
        {
            playerModel.ScorePoints += Time.deltaTime * playerModel.ScoreMultiplier;
        }
        
        private void OnTrickAction(PlayerEvent.TrickActionTriggered evt)
        {
            StartCoroutine(TrickActionActive(evt.TrickActionScore));
        }
        
        private IEnumerator TrickActionActive(float duration){
            yield return new WaitForSeconds(duration);
        }

        private void OnSpecialAction(PlayerEvent.SpecialActionTriggered evt)
        {
            // TODO: implement
            StartCoroutine(SpecialActionActive(evt.SpecialActionDuration));
        }

        private IEnumerator SpecialActionActive(float duration){
            yield return new WaitForSeconds(duration);
        }
        
        private void OnObstacleCollision(PlayerEvent.ObstacleCollision evt)
        {
            Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();

            if (playerModel.IsDoingJumpAction && obstacle.canJumpOver)
            {
                playerModel.ScorePoints += obstacle.DetermineScore();
            }
            if (obstacle.Type == ObstacleType.Rail)
            {
                playerModel.IsDoingGrindAction = true;
            }
            if (!obstacle.canJumpOver || !playerModel.IsDoingJumpAction)
            {
                TriggerCollision(obstacle);
            }
        }
        
        private void TriggerCollision(Obstacle obstacle)
        {
            if (playerModel.IsInvincible) return;
            AudioManager.Instance.PlaySound("crash");
            StartCoroutine(InvincibilityActive());
            int collisionDamage = obstacle.DetermineDamageAmount();
            playerModel.HealthPoints -= collisionDamage;
        }

        private IEnumerator InvincibilityActive()
        {
            playerModel.IsInvincible = true;
            yield return new WaitForSeconds(playerModel.InvincibilityDuration);
            playerModel.IsInvincible = false;
        }

        private void OnObstacleEvasion(PlayerEvent.ObstacleEvasion evt)
        {
            if (playerModel.IsInvincible) return;
            Obstacle obstacle = evt.Obstacle.GetComponent<Obstacle>();
            int evasionScore = obstacle.DetermineScore();

            playerModel.ScorePoints += evasionScore;
            if (obstacle.Type == ObstacleType.Rail)
            {
                playerModel.IsAboveRail = false;
            }
        }
        
        private void OnPickupCollision(PlayerEvent.PickupCollision evt)
        {
            PickupType pickupType = evt.PickupType;
            TriggerItemEffect(pickupType);
        }

        private void TriggerItemEffect(PickupType pickupType)
        {
            AudioManager.Instance.PlaySound("item");
            switch (pickupType)
            {
                case PickupType.HealthBoost:
                    playerModel.HealthPoints += 50f;
                    EventManager.Broadcast(new PlayerEvent.HealthPointsChanged(playerModel.HealthPoints + 50f));
                    break;
                case PickupType.SpecialBoost:
                    playerModel.SpecialPoints += 25f;
                    break;
                case PickupType.ScoreBoost:
                    playerModel.ScorePoints += Mathf.Max(100, playerModel.ScorePoints / 16) * playerModel.ScoreMultiplier;
                    break;
                case PickupType.SpeedBoost:
                    playerModel.SpeedMultiplier += 0.25f;
                    break;
                case PickupType.HealthBoom:
                    playerModel.HealthPoints = PlayerConfig.MaxHealthPoints;
                    break;
                case PickupType.SpecialBoom:
                    playerModel.SpecialPoints += 50f;
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
            EventManager.RemoveListener<PlayerEvent.TrickActionTriggered>(OnTrickAction);
            EventManager.RemoveListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.RemoveListener<PlayerEvent.ObstacleCollision>(OnObstacleCollision);
            EventManager.RemoveListener<PlayerEvent.ObstacleEvasion>(OnObstacleEvasion);
            EventManager.RemoveListener<PlayerEvent.PickupCollision>(OnPickupCollision);
        }
    }
}