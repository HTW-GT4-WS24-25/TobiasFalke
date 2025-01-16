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

        private void OnSpecialAction(PlayerEvent.SpecialActionTriggered obj)
        {
            // TODO: make action time based
            EventManager.Broadcast(new PlayerEvent.HealthChanged(100));
            EventManager.Broadcast(new PlayerEvent.SpecialChanged(0));
            EventManager.Broadcast(new PlayerEvent.ScoreChanged(playerModel.GetScore() + 200));
            EventManager.Broadcast(new PlayerEvent.JumpDurationChanged(playerModel.GetJumpDuration() + 1.0f));
            EventManager.Broadcast(new PlayerEvent.SpeedChanged(playerModel.GetSpeed() + 1.0f));
            StartCoroutine(SpecialActionDelay(playerModel.GetSpecialActionDuration()));
            AudioManager.Instance.PlaySound("specialAction");
        }

        private IEnumerator SpecialActionDelay(float duration){
            yield return new WaitForSeconds(duration);
        }
        
        private void OnTrickAction(PlayerEvent.TrickActionTriggered obj)
        {
            playerModel.IncreaseScore(obj.TrickActionScore);
        }
        

        private void OnPickupCollision(PlayerEvent.PickupCollision obj)
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
                    EventManager.Broadcast(new PlayerEvent.HealthChanged(playerModel.GetHealth() + 50f));
                    break;
                case PickupType.SpecialBoost:
                    EventManager.Broadcast(new PlayerEvent.SpecialChanged(playerModel.GetSpecial() + 30f));
                    break;
                case PickupType.ScoreBoost:
                    EventManager.Broadcast(new PlayerEvent.ScoreChanged(playerModel.GetScore() + 100f));
                    break;
                case PickupType.SpeedBoost:
                    EventManager.Broadcast(new PlayerEvent.SpeedChanged(playerModel.GetSpeed() + 1f));
                    break;
                case PickupType.JumpBoost:
                    EventManager.Broadcast(new PlayerEvent.JumpDurationChanged(playerModel.GetJumpDuration() + 1f));
                    break;
            }
        }
        private void OnJumpDurationChanged(PlayerEvent.JumpDurationChanged obj)
        {
            playerModel.SetJumpDuration(obj.NewJumpDuration);
        }

        private void OnSpeedChanged(PlayerEvent.SpeedChanged obj)
        {
            playerModel.SetSpeed(obj.NewSpeed);
        }

        private void OnScoreChanged(PlayerEvent.ScoreChanged obj)
        {
            playerModel.SetScore(obj.NewScore);
        }

        private void OnSpecialChanged(PlayerEvent.SpecialChanged obj)
        {
            playerModel.SetSpecial(obj.NewSpecial);
        }

        private void OnHealthChanged(PlayerEvent.HealthChanged obj)
        {
            playerModel.SetHealth(obj.NewHealth);
        }
        
        private void RegisterEvents()
        {
            EventManager.AddListener<PlayerEvent.HealthChanged>(OnHealthChanged);
            EventManager.AddListener<PlayerEvent.SpecialChanged>(OnSpecialChanged);
            EventManager.AddListener<PlayerEvent.ScoreChanged>(OnScoreChanged);
            EventManager.AddListener<PlayerEvent.SpeedChanged>(OnSpeedChanged);
            EventManager.AddListener<PlayerEvent.JumpDurationChanged>(OnJumpDurationChanged);
            EventManager.AddListener<PlayerEvent.TrickActionTriggered>(OnTrickAction);
            EventManager.AddListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.AddListener<PlayerEvent.PickupCollision>(OnPickupCollision);
        }
        
        private void BroadcastPlayerStatus()
        {
            EventManager.Broadcast(new PlayerEvent.ScoreChanged(playerModel.GetScore()));
            EventManager.Broadcast(new PlayerEvent.HealthChanged(playerModel.GetHealth()));
            EventManager.Broadcast(new PlayerEvent.SpecialChanged(playerModel.GetSpecial()));
            EventManager.Broadcast(new PlayerEvent.SpeedChanged(playerModel.GetSpeed()));
            EventManager.Broadcast(new PlayerEvent.JumpDurationChanged(playerModel.GetJumpDuration()));
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
        
        private void UnsubscribeEvents()
        {
            EventManager.RemoveListener<PlayerEvent.TrickActionTriggered>(OnTrickAction);
            EventManager.RemoveListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.RemoveListener<PlayerEvent.PickupCollision>(OnPickupCollision);
            EventManager.RemoveListener<PlayerEvent.HealthChanged>(OnHealthChanged);
            EventManager.RemoveListener<PlayerEvent.SpecialChanged>(OnSpecialChanged);
            EventManager.RemoveListener<PlayerEvent.ScoreChanged>(OnScoreChanged);
            EventManager.RemoveListener<PlayerEvent.SpeedChanged>(OnSpeedChanged);
            EventManager.RemoveListener<PlayerEvent.JumpDurationChanged>(OnJumpDurationChanged);
        }

    }
}