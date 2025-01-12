using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class GameView : Utilities.Singleton<GameView>
{
    [SerializeField] private UIDocument _pauseUI;

    // health bar
    [SerializeField] private Slider healthBar;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthFill;

    // special bar
    [SerializeField] private Slider specialBar;
    [SerializeField] private Gradient specialGradiant;
    [SerializeField] private Image specialFill;
    
    // movement
    public TextMeshProUGUI speedCounter;
    public TextMeshProUGUI speedMultiplier;
    public TextMeshProUGUI jumpCounter;
    public TextMeshProUGUI jumpMultiplier;

    // score & time
    public TextMeshProUGUI levelCounter;
    public TextMeshProUGUI scoreCounter;
    public TextMeshProUGUI timeCounter;
    public TextMeshProUGUI scoreMultiplier;
    
    // pop-ups
    [SerializeField] private Image specialActionButton;
    public TextMeshProUGUI countDownUI;
    public Image specialActionFlashImage;
    public PlayerStats stats;
    
    //TODO connect event actions to update functions 
    
    private void OnEnable()
    {
        if (stats == null) InitializeStats();
       
        stats.HealthChanged += UpdateHealthBar;
        stats.SpecialChanged += UpdateSpecialBar;
        stats.SpeedMultiplierChanged += UpdateSpeedMultiplier;
        stats.JumpDurationChanged += UpdateJumpMultiplier;
        stats.ScoreChanged += UpdateScoreCounter;
        stats.ScoreMultiplierChanged += UpdateScoreMultiplier;
    }

    private void OnDisable()
    {
        if (stats == null) return;

        stats.HealthChanged -= UpdateHealthBar;
        stats.SpecialChanged -= UpdateSpecialBar;
        stats.SpeedMultiplierChanged -= UpdateSpeedMultiplier;
        stats.JumpDurationChanged -= UpdateJumpMultiplier;
        stats.ScoreChanged -= UpdateScoreCounter;
        stats.ScoreMultiplierChanged -= UpdateScoreMultiplier;
    }

    private void InitializeStats()
    {
        stats = FindObjectOfType<PlayerStats>(); // Or another method of getting a reference
        if (stats != null) InitializeStatusBars(stats);
    }
    
    public void InitializeStatusBars(PlayerStats stats)
    {
        // Initialize player's max health & special bars on UI.
        SetMaxHealth(stats._maxHealth);
        SetMaxSpecial(stats._maxSpecial);
        UpdateHealthBar(stats._health);
        UpdateSpecialBar(stats._special);
        UpdateSpeedMultiplier(stats._speedMultiplier);
        UpdateJumpMultiplier(stats._speedMultiplier);
    }

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
    
    public void UpdateSpeedCounter(float currentSpeed)
    {
        speedCounter.text = currentSpeed + " km/h";
    }
    
    public void UpdateSpeedMultiplier(float newSpeedMultiplier)
    {
        speedMultiplier.text = "x" + newSpeedMultiplier;
    }
    
    public void UpdateJumpCounter(float currentJumpLength)
    {
        jumpCounter.text = currentJumpLength + " sec";
    }
    
    public void UpdateJumpMultiplier(float newJumpMultiplier)
    {
        jumpMultiplier.text = "x" + newJumpMultiplier;
    }
    
    public void UpdateScoreMultiplier(float newScoreMultiplier)
    {
        scoreMultiplier.text = "x" + newScoreMultiplier;
    }

    public void UpdateScoreCounter(float score)
    {
        scoreCounter.text = ((int)score).ToString();
    }
    
    public void UpdateLevelCounter(int level)
    {
        levelCounter.text = (level + 1).ToString();
    }
    
    public void UpdateCountDown(float remainingTime)
    {
        countDownUI.text = (((int)remainingTime).ToString());
    }

    public void ToggleCountDownVisibility(bool isActive)
    {
        countDownUI.gameObject.SetActive(isActive);
    }

    public void ToggleSpecialActionButton(bool isActive)
    {
        specialActionButton.gameObject.SetActive(isActive);
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
    
    public void SetPauseMenuUIVisibility(bool isActive)
    {
        _pauseUI.gameObject.SetActive(isActive);
    }
    
    public void PlayScreenFlash(float time)
    {
        StartCoroutine(ScreenFlashCoroutine(time));
    }

    private IEnumerator ScreenFlashCoroutine(float time)
    {
        specialActionFlashImage.gameObject.SetActive(true);

        float duration = 0.1f;
        int flashCount = (int)(time * 5);
        Color originalColor = specialActionFlashImage.color;

        for (int i = 0; i < flashCount; i++)
        {
            // Flash on
            originalColor.a = 0.5f;
            specialActionFlashImage.color = originalColor;
            yield return new WaitForSeconds(duration);

            // Flash off
            originalColor.a = 0f;
            specialActionFlashImage.color = originalColor;
            yield return new WaitForSeconds(duration);
        }
        specialActionFlashImage.gameObject.SetActive(false);
    }
}