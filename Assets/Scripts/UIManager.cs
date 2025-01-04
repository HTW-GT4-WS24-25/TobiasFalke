using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class UIManager : Singleton<UIManager>
{
    
    [SerializeField] private Slider healthBar;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthFill;
    
    [SerializeField] private Slider specialBar;
    [SerializeField] private Gradient specialGradiant;
    [SerializeField] private Image specialFill;
    
    [SerializeField] private UIDocument _pauseUI;

    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI timeUI;
    
    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health; 
        healthFill.color = healthGradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
        healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
    }

    public void SetMaxSpecial(int special)
    {
        specialBar.maxValue = special;
        specialBar.value = special; 
        specialFill.color = specialGradiant.Evaluate(1f);
    }
    
    public void SetSpecial(int special)
    {
        specialBar.value = special;
        specialFill.color = specialGradiant.Evaluate(specialBar.normalizedValue);
    }

    public void SetScore(float score)
    {
        scoreUI.text = ((int)score).ToString();
    }

    public void SetPauseMenuUIVisibility(bool state)
    {
        _pauseUI.gameObject.SetActive(state);
    }
    
}