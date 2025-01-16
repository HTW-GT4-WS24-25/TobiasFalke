using System.Collections;
using Events;
using Model;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerView playerView;
        private PlayerModel playerModel;
        private PlayerMovementController movementController;
        private InputManager playerInput;

        private void Awake()
        {
            playerModel = new PlayerModel();
            movementController = GetComponent<PlayerMovementController>();
            movementController.Initialize(playerModel, playerView);
            playerInput = InputManager.Instance;
        }

        private void Start()
        {
            RegisterEvents();
            BroadcastPlayerStatus();
        }
        
        private void Update()
        {
            movementController.HandleInput();
        }

        private void FixedUpdate()
        {
            movementController.UpdateMovement();
            UpdateScore();
        }

        private void UpdateScore()
        {
            playerModel.SetScore(playerModel.GetScore() + Time.deltaTime);
        }

        private void OnSpecialAction(PlayerEvents.SpecialActionTriggered obj)
        {
            // TODO: make action time based
            EventManager.Broadcast(new PlayerEvents.HealthChanged(100));
            EventManager.Broadcast(new PlayerEvents.SpecialChanged(0));
            EventManager.Broadcast(new PlayerEvents.ScoreChanged(playerModel.GetScore() + 200));
            EventManager.Broadcast(new PlayerEvents.JumpDurationChanged(playerModel.GetJumpDuration() + 1.0f));
            EventManager.Broadcast(new PlayerEvents.SpeedChanged(playerModel.GetSpeed() + 1.0f));
            StartCoroutine(SpecialActionDelay(playerModel.GetSpecialActionDuration()));
            AudioManager.Instance.PlaySound("specialAction");
        }

        private IEnumerator SpecialActionDelay(float duration){
            yield return new WaitForSeconds(duration);
        }
        
        private void OnTrickAction(PlayerEvents.TrickActionEvent obj)
        {
            playerModel.IncreaseScore(obj.TrickActionScore);
        }
        

        private void OnPickupCollision(PlayerEvents.PickupCollisionEvent obj)
        {
            PickupType pickupType = obj.PickupType;
            TriggerItemEffect(pickupType);
        }

        private void TriggerItemEffect(PickupType pickupType)
        {
            AudioManager.Instance.PlaySound("item");
            switch (pickupType)
            {
                case PickupType.HealthBoost:
                    EventManager.Broadcast(new PlayerEvents.HealthChanged(playerModel.GetHealth() + 50f));
                    break;
                case PickupType.SpecialBoost:
                    EventManager.Broadcast(new PlayerEvents.SpecialChanged(playerModel.GetSpecial() + 30f));
                    break;
                case PickupType.ScoreBoost:
                    EventManager.Broadcast(new PlayerEvents.ScoreChanged(playerModel.GetScore() + 100f));
                    break;
                case PickupType.SpeedBoost:
                    EventManager.Broadcast(new PlayerEvents.SpeedChanged(playerModel.GetSpeed() + 1f));
                    break;
                case PickupType.JumpBoost:
                    EventManager.Broadcast(new PlayerEvents.JumpDurationChanged(playerModel.GetJumpDuration() + 1f));
                    break;
            }
        }
        private void OnJumpDurationChanged(PlayerEvents.JumpDurationChanged obj)
        {
            playerModel.SetJumpDuration(obj.NewJumpDuration);
        }

        private void OnSpeedChanged(PlayerEvents.SpeedChanged obj)
        {
            playerModel.SetSpeed(obj.NewSpeed);
        }

        private void OnScoreChanged(PlayerEvents.ScoreChanged obj)
        {
            playerModel.SetScore(obj.NewScore);
        }

        private void OnSpecialChanged(PlayerEvents.SpecialChanged obj)
        {
            playerModel.SetSpecial(obj.NewSpecial);
        }

        private void OnHealthChanged(PlayerEvents.HealthChanged obj)
        {
            playerModel.SetHealth(obj.NewHealth);
        }
        
        private void RegisterEvents()
        {
            EventManager.AddListener<PlayerEvents.HealthChanged>(OnHealthChanged);
            EventManager.AddListener<PlayerEvents.SpecialChanged>(OnSpecialChanged);
            EventManager.AddListener<PlayerEvents.ScoreChanged>(OnScoreChanged);
            EventManager.AddListener<PlayerEvents.SpeedChanged>(OnSpeedChanged);
            EventManager.AddListener<PlayerEvents.JumpDurationChanged>(OnJumpDurationChanged);
            EventManager.AddListener<PlayerEvents.TrickActionEvent>(OnTrickAction);
            EventManager.AddListener<PlayerEvents.SpecialActionTriggered>(OnSpecialAction);
            EventManager.AddListener<PlayerEvents.PickupCollisionEvent>(OnPickupCollision);
        }
        
        private void BroadcastPlayerStatus()
        {
            EventManager.Broadcast(new PlayerEvents.ScoreChanged(playerModel.GetScore()));
            EventManager.Broadcast(new PlayerEvents.HealthChanged(playerModel.GetHealth()));
            EventManager.Broadcast(new PlayerEvents.SpecialChanged(playerModel.GetSpecial()));
            EventManager.Broadcast(new PlayerEvents.SpeedChanged(playerModel.GetSpeed()));
            EventManager.Broadcast(new PlayerEvents.JumpDurationChanged(playerModel.GetJumpDuration()));
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
        
        private void UnsubscribeEvents()
        {
            EventManager.RemoveListener<PlayerEvents.TrickActionEvent>(OnTrickAction);
            EventManager.RemoveListener<PlayerEvents.SpecialActionTriggered>(OnSpecialAction);
            EventManager.RemoveListener<PlayerEvents.PickupCollisionEvent>(OnPickupCollision);
            EventManager.RemoveListener<PlayerEvents.HealthChanged>(OnHealthChanged);
            EventManager.RemoveListener<PlayerEvents.SpecialChanged>(OnSpecialChanged);
            EventManager.RemoveListener<PlayerEvents.ScoreChanged>(OnScoreChanged);
            EventManager.RemoveListener<PlayerEvents.SpeedChanged>(OnSpeedChanged);
            EventManager.RemoveListener<PlayerEvents.JumpDurationChanged>(OnJumpDurationChanged);
        }

    }
}