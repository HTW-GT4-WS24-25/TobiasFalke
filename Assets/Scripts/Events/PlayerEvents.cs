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
        public class ScoreChanged : GameEvent
        {
            public float NewScore { get; }

            public ScoreChanged(float newScore)
            {
                NewScore = newScore;
            }
        }

        public class HealthChanged : GameEvent
        {
            public float NewHealth { get; private set; }

            public HealthChanged(float newHealth)
            {
                NewHealth = newHealth;
            }
        }

        public class SpecialChanged : GameEvent
        {
            public float NewSpecial { get; private set; }

            public SpecialChanged(float newSpecial)
            {
                NewSpecial = newSpecial;
            }
        }

        public class SpeedChanged : GameEvent
        {
            public float NewSpeed { get; private set; }

            public SpeedChanged(float newSpeed)
            {
                NewSpeed = newSpeed;
            }
        }

        public class JumpDurationChanged : GameEvent
        {
            public float NewJumpDuration { get; private set; }

            public JumpDurationChanged(float newJumpDuration)
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

        public class SpecialActionTriggered : GameEvent
        {
            public float SpecialActionDuration { get; }

            public SpecialActionTriggered(float specialActionDuration)
            {
                SpecialActionDuration = specialActionDuration;
            }
        }
    }
}