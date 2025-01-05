using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class UIManager : Utilities.Singleton<UIManager>
{
    // health bar
    [SerializeField] private Slider healthBar;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthFill;
    // special bar
    [SerializeField] private Slider specialBar;
    [SerializeField] private Gradient specialGradiant;
    [SerializeField] private Image specialFill;
    
    [SerializeField] private UIDocument _pauseUI;
    // multipliers
    public TextMeshProUGUI speedMultiplier;
    public TextMeshProUGUI jumpMultiplier;
    public TextMeshProUGUI scoreMultiplier;
    // counters
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI timeUI;

    public TextMeshProUGUI countDownUI;
    // pop-ups
    [SerializeField] private Image specialActionButton;
    public Image screenFlashImage;


    private void Start()
    {
        if (screenFlashImage != null)
        {
            // Initialize the screen flash image to be fully transparent
            Color color = screenFlashImage.color;
            color.a = 0f;
            screenFlashImage.color = color;
        }
    }

    public void InitializeStatusBars(PlayerStats stats)
    {
        // Initialize player's max health & special bars on UI.
        SetMaxHealth(stats._maxHealth);
        SetMaxSpecial(stats._maxSpecial);
        UpdateHealthBar(stats._health);
        UpdateSpecialBar(stats._special);
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

    public void UpdateCountDown(float remainingTime)
    {
        countDownUI.text = (((int)remainingTime).ToString());
    }

    public void ToggleCountDownVisibility(bool isActive)
    {
        countDownUI.gameObject.SetActive(isActive);
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
    
    public void SetPauseMenuUIVisibility(bool state)
    {
        _pauseUI.gameObject.SetActive(state);
    }
    
    public void PlayScreenFlash(float time)
    {
        StartCoroutine(ScreenFlashCoroutine(time));
    }

    private IEnumerator ScreenFlashCoroutine(float time)
    {
        if (screenFlashImage == null)
        {
            Debug.LogError("Screen flash image is not assigned.");
            yield break;
        }
        screenFlashImage.gameObject.SetActive(true);

        float duration = 0.1f;
        int flashCount = (int)(time * 5);
        Color originalColor = screenFlashImage.color;

        for (int i = 0; i < flashCount; i++)
        {
            // Flash on
            originalColor.a = 0.5f; // Adjust the alpha value as needed
            screenFlashImage.color = originalColor;
            yield return new WaitForSeconds(duration);

            // Flash off
            originalColor.a = 0f;
            screenFlashImage.color = originalColor;
            yield return new WaitForSeconds(duration);
        }
        screenFlashImage.gameObject.SetActive(false);
    }
}