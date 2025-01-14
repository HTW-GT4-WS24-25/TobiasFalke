using UnityEngine;

public class PlayerModel
{
    private float score;
    private float health = 100;
    private float special;
    // movement
    private Vector2 velocity;
    private float speed = 5f;
    private float jumpHeight = 2f;
    private float jumpDuration = 1f;
    // multipliers
    private float scoreMultiplier = 1f;
    private float speedMultiplier = 1f;
    private float jumpDurationMultiplier = 1f;
    // states
    private bool isJumping;
    private bool isGrinding;
    private bool isInvincible;
    private float invincibilityDuration = 1f;
    
    public float GetScore() => score;
    public void SetScore(float newScore) => score = newScore;
    public void IncreaseScore(float points) => score += (int)(points * scoreMultiplier);
    public float GetHealth() => health;
    public void SetHealth(float newHealth) => health = Mathf.Clamp(newHealth, 0, 100);
    public void IncreaseHealth(int amount) => SetHealth(health + amount);
    public float GetSpecial() => special;
    public void SetSpecial(float newSpecial) => special = newSpecial;
    
    public Vector2 GetVelocity() => velocity;
    public void SetVelocity(Vector2 newVelocity) => velocity = newVelocity;
    public float GetSpeed() => speed * speedMultiplier;
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed * speedMultiplier;
    }
    
    public float GetJumpHeight() => jumpHeight;
    
    public float GetJumpDuration() => jumpDuration;
    
    public void SetJumpDuration(float newJumpDuration)
    {
        jumpDuration = newJumpDuration * jumpDurationMultiplier;
    }
    public bool GetIsJumping() => isJumping;
    public void SetIsJumping(bool jumping) => isJumping = jumping;
    public bool GetIsGrinding() => isGrinding;
    public void SetIsGrinding(bool grinding) => isGrinding = grinding;
    public bool GetIsInvincible() => isInvincible;
    public void SetIsInvincible(bool invincible) => isInvincible = invincible;
    public float GetInvincibilityDuration() => invincibilityDuration;
    public void SetInvincibilityDuration(float newDuration) => invincibilityDuration = newDuration;
    
    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        EventManagerR.AddListener<PlayerEvents.HealthChangedEventR>(OnHealthChanged);
        EventManagerR.AddListener<PlayerEvents.SpecialChangedEventR>(OnSpecialChanged);
        EventManagerR.AddListener<PlayerEvents.ScoreChangedEventR>(OnScoreChanged);
        EventManagerR.AddListener<PlayerEvents.SpeedChangedEventR>(OnSpeedChanged);
        EventManagerR.AddListener<PlayerEvents.JumpDurationChangedEventR>(OnJumpDurationChanged);
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        EventManagerR.RemoveListener<PlayerEvents.HealthChangedEventR>(OnHealthChanged);
        EventManagerR.RemoveListener<PlayerEvents.SpecialChangedEventR>(OnSpecialChanged);
        EventManagerR.RemoveListener<PlayerEvents.ScoreChangedEventR>(OnScoreChanged);
        EventManagerR.RemoveListener<PlayerEvents.SpeedChangedEventR>(OnSpeedChanged);
        EventManagerR.RemoveListener<PlayerEvents.JumpDurationChangedEventR>(OnJumpDurationChanged);
    }

    private void OnJumpDurationChanged(PlayerEvents.JumpDurationChangedEventR obj)
    {
        SetJumpDuration(jumpDuration + obj.NewJumpDuration);
    }

    private void OnSpeedChanged(PlayerEvents.SpeedChangedEventR obj)
    {
        SetSpeed(speed + obj.NewSpeed);
    }

    private void OnScoreChanged(PlayerEvents.ScoreChangedEventR obj)
    {
        SetScore(score + obj.NewScore);
    }

    private void OnSpecialChanged(PlayerEvents.SpecialChangedEventR obj)
    {
        SetSpecial(special + obj.NewSpecial);
    }

    private void OnHealthChanged(PlayerEvents.HealthChangedEventR obj)
    {
        SetHealth(health + obj.NewHealth);
    }
}