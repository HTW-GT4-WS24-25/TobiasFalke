using UnityEngine;

public class PlayerModel
{
    private float score;
    private Vector2 velocity;
    private int health;
    private int special;
    private float scoreMultiplier;
    private float speedMultiplier;
    private float jumpDurationMultiplier;
    private float speed;
    private float jumpDuration;
    private float jumpHeight;
    private bool isJumping;
    private bool isGrinding;
    private bool isInvincible;
    private float invincibilityDuration;

    public PlayerModel()
    {
        health = 100;
        score = 0;
        speed = 5f;
        jumpDuration = 1f;
        jumpHeight = 1f;
        scoreMultiplier = 1f;
        speedMultiplier = 1f;
        jumpDurationMultiplier = 1f;
        invincibilityDuration = 1f;
    }

    public float GetScore() => score;
    public void SetScore(float newScore) => score = newScore;
    public void IncreaseScore(float points) => score += (int)(points * scoreMultiplier);
    public int GetHealth() => health;
    public void SetHealth(int newHealth) => health = Mathf.Clamp(newHealth, 0, 100);
    public void IncreaseHealth(int amount) => SetHealth(health + amount);
    public int GetSpecial() => special;
    public void SetSpecial(int newSpecial) => special = newSpecial;

    public Vector2 GetVelocity() => velocity;
    public void SetVelocity(Vector2 newVelocity) => velocity = newVelocity;
    public float GetSpeed() => speed;
    public void SetSpeed(float newSpeed) { speed = newSpeed; }
    
    public float GetJumpHeight() => jumpHeight;
    
    public float GetJumpDuration() => jumpDuration;
    public void SetJumpDuration(float newDuration) { jumpDuration = newDuration; }
    public bool GetIsJumping() => isJumping;
    public void SetIsJumping(bool jumping) => isJumping = jumping;
    
    public bool GetIsGrinding() => isGrinding;
    public void SetIsGrinding(bool grinding) => isGrinding = grinding;
    public bool GetIsInvincible() => isInvincible;
    public void SetIsInvincible(bool invincible) => isInvincible = invincible;
    public float GetInvincibilityDuration() => invincibilityDuration;
    public void SetInvincibilityDuration(float newDuration) => invincibilityDuration = newDuration;
}