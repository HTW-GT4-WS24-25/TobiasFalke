using Model;
using UnityEngine;
using Event = Utility.Event;

namespace Events
{
    public static class PlayerEvent
    {
        public class ScorePointsChanged : Event
        {
            public float NewScorePoints { get; }

            public ScorePointsChanged(float newScorePoints)
            {
                NewScorePoints = newScorePoints;
            }
        }

        public class HealthPointsChanged : Event
        {
            public float NewHealthPoints { get; private set; }

            public HealthPointsChanged(float newHealthPoints)
            {
                NewHealthPoints = newHealthPoints;
            }
        }
        
        public class SpecialPointsChanged : Event
        {
            public float NewSpecialPoints { get; private set; }

            public SpecialPointsChanged(float newSpecialPoints)
            {
                NewSpecialPoints = newSpecialPoints;
            }
        }
        
        public class ScoreMultiplierChanged : Event
        {
            public float NewScoreMultiplier { get; private set; }

            public ScoreMultiplierChanged(float newScoreMultiplier)
            {
                NewScoreMultiplier = newScoreMultiplier;
            }
        }
        
        public class SpeedMultiplierChanged : Event
        {
            public float NewSpeedMultiplier { get; private set; }

            public SpeedMultiplierChanged(float newSpeedMultiplier)
            {
                NewSpeedMultiplier = newSpeedMultiplier;
            }
        }
        

        public class SpeedChanged : Event
        {
            public float NewSpeed { get; private set; }

            public SpeedChanged(float newSpeed)
            {
                NewSpeed = newSpeed;
            }
        }

        public class JumpDurationChanged : Event
        {
            public float NewJumpDuration { get; private set; }

            public JumpDurationChanged(float newJumpDuration)
            {
                NewJumpDuration = newJumpDuration;
            }
        }
        
        public class JumpActionTriggered : Event
        {
            public float JumpDuration { get; }
            public float JumpHeight;

            public JumpActionTriggered(float jumpDuration, float jumpHeight)
            {
                JumpDuration = jumpDuration;
                JumpHeight = jumpHeight;
            }
        }
        
        public class LandActionTriggered : Event
        {
        }
        
        public class GrindActionTriggered : Event
        {
            public float GrindScore { get; }

            public GrindActionTriggered(float grindScore)
            {
                GrindScore = grindScore;
            }
        }
    
        public class TrickActionTriggered : Event
        {
            public float TrickActionScore { get; }

            public TrickActionTriggered(float trickActionScore)
            {
                TrickActionScore = trickActionScore;
            }
        }

        public class SpecialActionTriggered : Event
        {
            public float SpecialActionDuration { get; }

            public SpecialActionTriggered(float specialActionDuration)
            {
                SpecialActionDuration = specialActionDuration;
            }
        }
        
        public class InvincibilityTriggered : Event
        {
            public float InvincibilityDuration { get; }

            public InvincibilityTriggered(float invincibilityDuration)
            {
                InvincibilityDuration = invincibilityDuration;
            }
        }
        
        public class ObstacleCollision : Event
        {
            public GameObject Obstacle { get; }

            public ObstacleCollision(GameObject obstacle)
            {
                Obstacle = obstacle;
            }
        }

        public class ObstacleEvasion : Event
        {
            public GameObject Obstacle { get; }

            public ObstacleEvasion(GameObject obstacle)
            {
                Obstacle = obstacle;
            }
        }

        public class PickupCollision : Event
        {
            public PickupType PickupType { get; }

            public PickupCollision(PickupType pickupType)
            {
                PickupType = pickupType;
            }
        }
        
        public class GameOverTriggered : Event
        {
            public float ScoreOnLoose { get; }

            public GameOverTriggered(float scoreOnLoose)
            {
                ScoreOnLoose = scoreOnLoose;
            }
        }
    }
}