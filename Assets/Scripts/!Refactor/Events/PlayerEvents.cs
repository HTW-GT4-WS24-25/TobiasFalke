using UnityEngine;

public static class PlayerEvents
{
    public class ObstacleCollisionEventR : GameEvent
    {
        public GameObject Obstacle { get; }

        public ObstacleCollisionEventR(GameObject obstacle)
        {
            Obstacle = obstacle;
        }
    }

    public class ObstacleCollisionExitEventR : GameEvent
    {
        public GameObject Obstacle { get; }

        public ObstacleCollisionExitEventR(GameObject obstacle)
        {
            Obstacle = obstacle;
        }
    }

    public class PickupCollisionEventR : GameEvent
    {
        public GameObject Pickup { get; }

        public PickupCollisionEventR(GameObject pickup)
        {
            Pickup = pickup;
        }
    }
    public class ScoreChangedEventR : GameEvent
    {
        public float NewScore { get; }

        public ScoreChangedEventR(float newScore)
        {
            NewScore = newScore;
        }
    }

    public class HealthChangedEventR : GameEvent
    {
        public float NewHealth { get; private set; }

        public HealthChangedEventR(float newHealth)
        {
            NewHealth = newHealth;
        }
    }

    public class SpecialChangedEventR : GameEvent
    {
        public float NewSpecial { get; private set; }

        public SpecialChangedEventR(float newSpecial)
        {
            NewSpecial = newSpecial;
        }
    }

    public class SpeedChangedEventR : GameEvent
    {
        public float NewSpeed { get; private set; }

        public SpeedChangedEventR(float newSpeed)
        {
            NewSpeed = newSpeed;
        }
    }

    public class JumpDurationChangedEventR : GameEvent
    {
        public float NewJumpDuration { get; private set; }

        public JumpDurationChangedEventR(float newJumpDuration)
        {
            NewJumpDuration = newJumpDuration;
        }
    }

    public class JumpEventR : GameEvent
    {
        public float initialJumpHeight;
        public float jumpDuration { get; }

        public JumpEventR(float jumpHeight)
        {
            initialJumpHeight = jumpHeight;
        }
    }
    
    public class TrickActionEventR : GameEvent
    {
        public int Points;
        public string TrickName { get; }

        public TrickActionEventR(string trickName)
        {
            TrickName = trickName;
        }

        public TrickActionEventR()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SpecialActionEventR : GameEvent
    {
        public string SpecialAction { get; }

        public SpecialActionEventR(string specialAction)
        {
            SpecialAction = specialAction;
        }

        public SpecialActionEventR()
        {
            throw new System.NotImplementedException();
        }
    }
}