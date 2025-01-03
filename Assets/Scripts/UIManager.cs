using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

// manages UI elements and handles game state transitions in the UI

public class UIManager : Singleton<UIManager>
{
    // health bar
    [SerializeField] private Slider healthBar;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthFill;
    // special bar
    [SerializeField] private Slider specialBar;
    [SerializeField] private Gradient specialGradiant;
    [SerializeField] private Image specialFill;
    // multipliers
    public TextMeshProUGUI speedMultiplier;
    public TextMeshProUGUI jumpMultiplier;
    public TextMeshProUGUI scoreMultiplier;
    // counters
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI timeUI;
    // pop-ups
    [SerializeField] private Image specialActionButton;

    public void UpdateHealthBar(int health)
    {
        healthBar.value = health;
        healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
    }
    
    public void UpdateSpecialBar(int special)
    {
        specialBar.value = special;
        specialFill.color = specialGradiant.Evaluate(specialBar.normalizedValue);
    }
    public void UpdateSpeedMultiplier(float newSpeedMultiplier)
    {
        speedMultiplier.text = newSpeedMultiplier + "x";
    }
    
    public void UpdateJumpMultiplier(float newJumpMultiplier)
    {
        jumpMultiplier.text = newJumpMultiplier + "x";
    }
    
    public void UpdateScoreMultiplier(float newScoreMultiplier)
    {
        scoreMultiplier.text = newScoreMultiplier + "x";
    }

    public void UpdateScoreCounter(float score)
    {
        scoreUI.text = ((int)score).ToString();
    }

    public void ToggleSpecialActionButton(bool onOrOff)
    {
        specialActionButton.gameObject.SetActive(onOrOff);
    }
    
    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health; 
        healthFill.color = healthGradient.Evaluate(1f);
    }

    public void SetMaxSpecial(int special)
    {
        specialBar.maxValue = special;
        specialBar.value = special; 
        specialFill.color = specialGradiant.Evaluate(1f);
    }
}