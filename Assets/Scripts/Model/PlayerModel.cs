using Events;
using UnityEngine;

public class PlayerModel
{
    // values modified by items
    private float scorePoints;
    private float healthPoints;
    private float specialPoints;
    private float scoreMultiplier;
    private float speedMultiplier;
    
    // movement values
    private Vector2 currentVelocity;
    private float speed;
    private float jumpDuration;
    
    private float grindActionScore;
    private float trickActionScore;
    private float trickActionDuration;
    private float specialActionDuration;
    private float invincibilityDuration;
    
    // player states
    private bool isAboveRail;
    private bool isDoingJumpAction;
    private bool isDoingGrindAction;
    private bool isDoingTrickAction;
    private bool isDoingSpecialAction;
    private bool isInvincible;

    // TODO: put into player config

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
            EventManager.Broadcast(new PlayerEvent.ScorePointsChanged(scorePoints));
        }
    }
    
    public float HealthPoints
    {
        get => healthPoints;
        set
        {
            healthPoints = value;
            EventManager.Broadcast(new PlayerEvent.HealthPointsChanged(healthPoints));
        }
    }
    
    public float SpecialPoints
    {
        get => specialPoints;
        set
        {
            specialPoints = value;
            EventManager.Broadcast(new PlayerEvent.SpecialPointsChanged(specialPoints));
        }
    }

    public float ScoreMultiplier
    {
        get => scoreMultiplier;
        set
        {
            scoreMultiplier = value;
            EventManager.Broadcast(new PlayerEvent.ScoreMultiplierChanged(scoreMultiplier));
        }
    }

    public float SpeedMultiplier
    {
        get => speedMultiplier;
        set
        {
            speedMultiplier = value;
            EventManager.Broadcast(new PlayerEvent.SpeedMultiplierChanged(speedMultiplier));
        }
    }
    
    public Vector2 CurrentVelocity { get; set; }

    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            EventManager.Broadcast(new PlayerEvent.SpeedChanged(speed));
        }
    }

    public float JumpDuration
    {
        get => jumpDuration;
        set
        {
            jumpDuration = value;
            EventManager.Broadcast(new PlayerEvent.JumpDurationChanged(jumpDuration));
        }
    }
    
    public bool IsAboveRail { get; set; }
    
    public bool IsDoingJumpAction
    {
        get => isDoingJumpAction;
        set
        {
            isDoingJumpAction = value;
            EventManager.Broadcast(new PlayerEvent.JumpActionTriggered(jumpDuration, JumpHeight));
        }
    }

    public bool IsDoingGrindAction
    {
        get => isDoingGrindAction;
        set
        {
            isDoingGrindAction = value;
            EventManager.Broadcast(new PlayerEvent.GrindActionTriggered(grindActionScore));
        }
    }

    public bool IsDoingTrickAction
    {
        get => isDoingTrickAction;
        set
        {
            isDoingTrickAction = value;
            EventManager.Broadcast(new PlayerEvent.TrickActionTriggered(trickActionDuration));
        }
    }

    public bool IsDoingSpecialAction
    {
        get => isDoingSpecialAction;
        set
        {
            isDoingSpecialAction = value;
            EventManager.Broadcast(new PlayerEvent.SpecialActionTriggered(specialActionDuration));
        }
    }

    public bool IsInvincible
    {
        get => isInvincible;
        set
        {
            isInvincible = value;
            EventManager.Broadcast(new PlayerEvent.InvincibilityTriggered(invincibilityDuration));
        }
    }
}