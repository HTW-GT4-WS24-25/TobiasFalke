using UnityEngine;

public class PlayerStats
{
    public int _health;
    public int _special;
    public float _score;
        
    internal void ChangeSpeed(int amount)
    {
        Debug.Log("Score was changed by " + amount);
        // TODO: logic to change player speed by value
        // TODO: make speed upgrade/downgrade disappear after x time
        // TODO: show speed upgrade/downgrade icon in UI 
    }

    internal void ChangeJumpDuration(double time)
    {
        Debug.Log("Jump duration was changed by " + time);
        // TODO: logic to change player jump duration by value
        // TODO: make jump upgrade/downgrade disappear after x time
        // TODO: show jump upgrade/downgrade icon in UI 
    }

    internal void ChangeHealth(int amount)
    {
        Debug.Log("Score was changed by " + amount);
        _health += amount;
        UIManager.Instance.SetHealth(_health);
    }
    
    internal void ChangeSpecial(int amount)
    {
        Debug.Log("Special was changed by " + amount);
        _health += amount;
        UIManager.Instance.SetSpecial(_special);
    }
    
    internal void ChangeScore(int amount)
    {
        Debug.Log("Score was changed by " + amount);
        _score += amount;
        UIManager.Instance.SetScore(_score);
    }

    internal void MultiplyScore(double factor)
    {
        Debug.Log("Score was multiplied by " + factor);
        _score *= (int)factor;
    }
}