using Events;
using UnityEngine;
using Utility;
using static Utility.GameConstants;

namespace Model
{
    public class PlayerModel
    {
        // MODIFIABLE BY PICKUPS
        private float scorePoints;
        private float scoreMultiplier = 1f;
        private int healthPoints;
        private int specialPoints;
        private float speed;
        private float speedMultiplier = 1f;
        private float jumpDuration;
        private int maxHealthPoints = 100;
        private int maxSpecialPoints = 100;
        private float maxSpeedMultiplier;
        private float jumpHeight;
    
        // CONSTANT VALUES
        private float grindActionScore;
        private float trickActionScore;
        private float trickActionDuration;
        private float specialActionDuration;
        private float invincibilityDuration;
    
        // PLAYER STATES
        private bool isAboveRail;
        private bool isDoingJumpAction;
        private bool isDoingGrindAction;
        private bool isDoingTrickAction;
        private bool isDoingSpecialAction;
        private bool isInvincible;
        
        public int MaxHealthPoints { get; set; }

        public int MaxSpecialPoints { get; set; }

        public float MaxSpeedMultiplier { get; set; }
        public float JumpHeight { get; set; }

        public float GrindActionScore { get; set; }

        public float TrickActionScore { get; set; }
    
        public float TrickActionDuration { get; set; }
    
        public float SpecialActionDuration { get; set; }
    
        public float InvincibilityDuration { get; set; }

    
        public float ScorePoints
        {
            get => scorePoints;
            set
            {
                scorePoints = value;
                EventManager.Trigger(new PlayerEvent.ScorePointsChanged(scorePoints));
            }
        }
    
        public int HealthPoints
        {
            get => healthPoints;
            set
            {
                healthPoints = value > healthPoints ? Mathf.Min(value, maxHealthPoints) : value;
                EventManager.Trigger(new PlayerEvent.HealthPointsChanged(healthPoints));
            }
        }
    
        public int SpecialPoints
        {
            get => specialPoints;
            set
            {
                specialPoints = value > specialPoints ? Mathf.Min(value, maxSpecialPoints) : value;
                EventManager.Trigger(new PlayerEvent.SpecialPointsChanged(specialPoints));
            }
        }

        public float ScoreMultiplier
        {
            get => scoreMultiplier;
            set
            {
                scoreMultiplier = value;
                EventManager.Trigger(new PlayerEvent.ScoreMultiplierChanged(scoreMultiplier));
            }
        }

        public float SpeedMultiplier
        {
            get => speedMultiplier;
            set
            {
                speedMultiplier = value > speedMultiplier ? Mathf.Min(value, MaxSpeedMultiplier) : value;
                EventManager.Trigger(new PlayerEvent.SpeedMultiplierChanged(speedMultiplier));
            }
        }
        
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                EventManager.Trigger(new PlayerEvent.SpeedChanged(speed));
            }
        }

        public float JumpDuration
        {
            get => jumpDuration;
            set
            {
                jumpDuration = value;
                EventManager.Trigger(new PlayerEvent.JumpDurationChanged(jumpDuration));
            }
        }
    
        public bool IsAboveRail { get; set; }
    
        public bool IsDoingJumpAction
        {
            get => isDoingJumpAction;
            set
            {
                isDoingJumpAction = value;
                EventManager.Trigger(new PlayerEvent.JumpActionTriggered(jumpDuration, JumpHeight));
            }
        }

        public bool IsDoingGrindAction
        {
            get => isDoingGrindAction;
            set
            {
                isDoingGrindAction = value;
                if (isDoingGrindAction) AudioManager.Instance.StopBackgroundTrack();
                EventManager.Trigger(new PlayerEvent.GrindActionTriggered(grindActionScore));
            }
        }

        public bool IsDoingTrickAction
        {
            get => isDoingTrickAction;
            set
            {
                isDoingTrickAction = value;
                if (isDoingTrickAction) EventManager.Trigger(new PlayerEvent.TrickActionTriggered(trickActionDuration));
            }
        }

        public bool IsDoingSpecialAction
        {
            get => isDoingSpecialAction;
            set
            {
                isDoingSpecialAction = value;
                EventManager.Trigger(new PlayerEvent.SpecialActionTriggered(specialActionDuration));
            }
        }

        public bool IsInvincible
        {
            get => isInvincible;
            set
            {
                isInvincible = value;
                EventManager.Trigger(new PlayerEvent.InvincibilityTriggered(invincibilityDuration));
            }
        }
    }
}