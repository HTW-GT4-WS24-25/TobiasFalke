using Model;
using UnityEngine;

namespace Events
{
    public static class PlayerEvents
    {
        public class ObstacleCollisionEvent : GameEvent
        {
            public GameObject Obstacle { get; }

            public ObstacleCollisionEvent(GameObject obstacle)
            {
                Obstacle = obstacle;
            }
        }

        public class ObstacleCollisionExitEvent : GameEvent
        {
            public GameObject Obstacle { get; }

            public ObstacleCollisionExitEvent(GameObject obstacle)
            {
                Obstacle = obstacle;
            }
        }

        public class PickupCollisionEvent : GameEvent
        {
            public PickupType PickupType { get; }

            public PickupCollisionEvent(PickupType pickupType)
            {
                PickupType = pickupType;
            }
        }
        public class ScoreChangedEvent : GameEvent
        {
            public float NewScore { get; }

            public ScoreChangedEvent(float newScore)
            {
                NewScore = newScore;
            }
        }

        public class HealthChangedEvent : GameEvent
        {
            public float NewHealth { get; private set; }

            public HealthChangedEvent(float newHealth)
            {
                NewHealth = newHealth;
            }
        }

        public class SpecialChangedEvent : GameEvent
        {
            public float NewSpecial { get; private set; }

            public SpecialChangedEvent(float newSpecial)
            {
                NewSpecial = newSpecial;
            }
        }

        public class SpeedChangedEvent : GameEvent
        {
            public float NewSpeed { get; private set; }

            public SpeedChangedEvent(float newSpeed)
            {
                NewSpeed = newSpeed;
            }
        }

        public class JumpDurationChangedEvent : GameEvent
        {
            public float NewJumpDuration { get; private set; }

            public JumpDurationChangedEvent(float newJumpDuration)
            {
                NewJumpDuration = newJumpDuration;
            }
        }

        public class JumpEvent : GameEvent
        {
            public float initialJumpHeight;
            public float jumpDuration { get; }

            public JumpEvent(float jumpHeight)
            {
                initialJumpHeight = jumpHeight;
            }
        }
    
        public class TrickActionEvent : GameEvent
        {
            public float TrickActionScore { get; }

            public TrickActionEvent(float trickActionScore)
            {
                TrickActionScore = trickActionScore;
            }
        }

        public class SpecialActionEvent : GameEvent
        {
            public float SpecialActionDuration { get; }

            public SpecialActionEvent(float specialActionDuration)
            {
                SpecialActionDuration = specialActionDuration;
            }
        }
    }
}