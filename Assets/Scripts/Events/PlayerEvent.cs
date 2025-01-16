using Model;
using UnityEngine;

namespace Events
{
    public static class PlayerEvent
    {
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
        public class ScoreChanged : Event
        {
            public float NewScore { get; }

            public ScoreChanged(float newScore)
            {
                NewScore = newScore;
            }
        }

        public class HealthChanged : Event
        {
            public float NewHealth { get; private set; }

            public HealthChanged(float newHealth)
            {
                NewHealth = newHealth;
            }
        }

        public class SpecialChanged : Event
        {
            public float NewSpecial { get; private set; }

            public SpecialChanged(float newSpecial)
            {
                NewSpecial = newSpecial;
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

        public class JumpTriggered : Event
        {
            public float initialJumpHeight;
            public float jumpDuration { get; }

            public JumpTriggered(float jumpHeight)
            {
                initialJumpHeight = jumpHeight;
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
    }
}