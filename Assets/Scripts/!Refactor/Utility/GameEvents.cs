using UnityEngine;

public class GameEvents
{
    public class GameStateChangedEvent : GameEvent
    {
        public GameModelR.GameState NewGameState { get; }

        public GameStateChangedEvent(GameModelR.GameState newGameState)
        {
            NewGameState = newGameState;
        }
    }
    
    public class ToggleCountDownEvent : GameEvent
    {
        public bool SetActive { get; }

        public ToggleCountDownEvent(bool setActive)
        {
            SetActive = setActive;
        }
    }

    public class TogglePauseMenuEvent : GameEvent
    {
        public bool IsPaused { get; }

        public TogglePauseMenuEvent(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
    
    public class LevelChangedEvent : GameEvent
    {
        public int NewLevel { get; }

        public LevelChangedEvent(int newLevel)
        {
            NewLevel = newLevel;
        }
    }
    
    public class LevelSpeedChangedEvent : GameEvent
    {
        public float LevelSpeed { get; private set; }

        public LevelSpeedChangedEvent(float levelSpeed)
        {
            LevelSpeed = levelSpeed;
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
    
    public class TrickActionEvent : GameEvent
    {
        public int Points;
        public string TrickName { get; }

        public TrickActionEvent(string trickName)
        {
            TrickName = trickName;
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
    
    public class SpecialActionEvent : GameEvent
    {
        public string SpecialAction { get; }

        public SpecialActionEvent(string specialAction)
        {
            SpecialAction = specialAction;
        }
    }
    
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

    public class UpgradeCollisionEvent : GameEvent
    {
        public GameObject Upgrade { get; }

        public UpgradeCollisionEvent(GameObject upgrade)
        {
            Upgrade = upgrade;
        }
    }
}