using UnityEngine;

public class PlayerModel
{
    private float score;
    private Vector2 velocity;
    private int health;
    private int special;
    private float scoreMultiplier = 1f;
    private float speedMultiplier = 1f;
    private float jumpDurationMultiplier;
    private float speed = 5f;
    private float jumpDuration = 5f;
    private bool isInvincible;
    private float invincibilityDuration = 1f;

    public PlayerModel()
    {
        health = 100;
        score = 0;
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

    public float GetJumpDuration() => jumpDuration;
    public void SetJumpDuration(float newDuration) { jumpDuration = newDuration; }
    public bool GetIsInvincible() => isInvincible;
    public void SetIsInvincible(bool invincible) => isInvincible = invincible;
    public float GetInvincibilityDuration() => invincibilityDuration;
    public void SetInvincibilityDuration(float newDuration) => invincibilityDuration = newDuration;
}