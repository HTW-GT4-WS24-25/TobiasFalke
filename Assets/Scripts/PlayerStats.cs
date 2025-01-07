using System.Collections;
using UnityEngine;

public class PlayerStats: MonoBehaviour
{
    // Values changing during gameplay.
    public int _health = 100;
    public int _special = 0;
    public float _speed;
    public float _speedMultiplier = 1;
    public float _jumpLength;
    public float _jumpMultiplier = 1;
    public float _score = 0;
    public float _scoreMultiplier = 1;
    
    // Ceiling values.
    public int _maxHealth = 100;
    public int _maxSpecial = 100;
    public float _maxSpeedMultiplier = 5;
    public float _maxJumpMultiplier = 5;
    public float _maxScoreMultiplier = 5;
    
    // For invincibility frames (after being hit).
    public bool isInvincible = false;
    public float invincibilityDuration = 1.2f;
    
    internal void UpdateHealth(int amount)
    {
        _health = Mathf.Clamp(_health + amount, 0, _maxHealth);
        UIManager.Instance.UpdateHealthBar(_health);
        Debug.Log("Health was in/decreased by " + amount + " and is now " + _health);
    }

    internal void SetHealth(int newHealth)
    {
        _health = Mathf.Clamp(newHealth, 0, _maxHealth);
        UIManager.Instance.UpdateHealthBar(_health);
        Debug.Log("Health was set to " + _health);
    }

    internal void UpdateSpecial(int amount)
    {
        _special = Mathf.Clamp(_special + amount, 0, _maxSpecial);
        UIManager.Instance.UpdateSpecialBar(_special);
        Debug.Log("Special was in/decreased by " + amount + " and is now " + _special);
    }

    internal void SetSpecial(int newSpecial)
    {
        _special = Mathf.Clamp(newSpecial, 0, _maxSpecial);
        UIManager.Instance.UpdateSpecialBar(_special);
        Debug.Log("Special was set to " + _special);
    }

    internal void UpdateSpeedMultiplier(float percent)
    {
        _speedMultiplier = Mathf.Clamp(_speedMultiplier + percent / 100, 0, _maxSpeedMultiplier);
        UIManager.Instance.UpdateSpeedMultiplier(_speedMultiplier);
        Debug.Log("Speed multiplier in/decreased by " + percent + "% and is now " + _speedMultiplier);
    }

    internal void SetSpeedMultiplier(float newSpeedMultiplier)
    {
        _speedMultiplier = Mathf.Clamp(newSpeedMultiplier, 0, _maxSpeedMultiplier);
        UIManager.Instance.UpdateSpeedMultiplier(_speedMultiplier);
        Debug.Log("Speed was set to " + _speedMultiplier);
    }

    internal void UpdateJumpDuration(float percent)
    {
        _jumpMultiplier = Mathf.Clamp(_jumpMultiplier + percent / 100, 0, _maxJumpMultiplier);
        UIManager.Instance.UpdateJumpMultiplier(_jumpMultiplier);
        Debug.Log("Jump duration was in/decreased by " + percent + "% and is now " + _jumpMultiplier);
    }

    internal void SetJumpMultiplier(float newJumpMultiplier)
    {
        _jumpMultiplier = Mathf.Clamp(newJumpMultiplier, 0, _maxJumpMultiplier);
        UIManager.Instance.UpdateJumpMultiplier(_jumpMultiplier);
        Debug.Log("Jump duration multiplier was set to " + _jumpMultiplier);
    }

    internal void UpdateScore(int amount)
    {
        _score += amount;
        UIManager.Instance.UpdateScoreCounter(_score);
        Debug.Log("Score was changed by " + amount + " and is now " + _score);
    }

    internal void UpdateScoreMultiplier(float percent)
    {
        _scoreMultiplier = Mathf.Clamp(_scoreMultiplier + percent / 100, 0, _maxScoreMultiplier);
        UIManager.Instance.UpdateScoreMultiplier(_scoreMultiplier);
        Debug.Log("Score multiplication was in/decreased by " + percent + "% and is now " + _scoreMultiplier);
    }

    internal void SetScoreMultiplier(float multiplier)
    {
        _scoreMultiplier = Mathf.Clamp(multiplier, 0, _maxScoreMultiplier);
        UIManager.Instance.UpdateScoreMultiplier(_scoreMultiplier);
        Debug.Log("Score multiplier was set to " + _scoreMultiplier);
    }

    internal void TriggerSpecialAction()
    {
        SetHealth(100);
        UpdateScoreMultiplier(100);
        UpdateSpeedMultiplier(50);
        UpdateJumpDuration(50);
        // Reset special points to 0.
        SetSpecial(0);
    }
}