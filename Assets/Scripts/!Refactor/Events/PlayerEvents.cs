using UnityEngine;

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
        public GameObject Pickup { get; }

        public PickupCollisionEvent(GameObject pickup)
        {
            Pickup = pickup;
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
        public int Points;
        public string TrickName { get; }

        public TrickActionEvent(string trickName)
        {
            TrickName = trickName;
        }

        public TrickActionEvent()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SpecialActionEvent : GameEvent
    {
        public string SpecialAction { get; }

        public SpecialActionEvent(string specialAction)
        {
            SpecialAction = specialAction;
        }

        public SpecialActionEvent()
        {
            throw new System.NotImplementedException();
        }
    }
}