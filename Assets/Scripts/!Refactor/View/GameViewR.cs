using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameViewR : MonoBehaviour
{
    public TextMeshProUGUI levelCounter;
    public TextMeshProUGUI timeCounter;
    public TextMeshProUGUI scoreCounter;
    
    [SerializeField] private Slider healthBar;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthFill;
    [SerializeField] private Slider specialBar;
    [SerializeField] private Gradient specialGradiant;
    [SerializeField] private Image specialFill;
    
    public TextMeshProUGUI speedCounter;
    public TextMeshProUGUI jumpDurationCounter;
    
    private void Awake()
    {
        EventManagerR.AddListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        EventManagerR.AddListener<GameEvents.LevelChangedEvent>(OnLevelChanged);
        EventManagerR.AddListener<GameEvents.ScoreChangedEvent>(OnScoreChanged);
        EventManagerR.AddListener<GameEvents.HealthChangedEvent>(OnHealthChanged);
        EventManagerR.AddListener<GameEvents.SpecialChangedEvent>(OnSpecialChanged);
        EventManagerR.AddListener<GameEvents.SpeedChangedEvent>(OnSpeedChanged);
        EventManagerR.AddListener<GameEvents.JumpDurationChangedEvent>(OnJumpDurationChanged);
    }

    private void OnDestroy()
    {
        EventManagerR.RemoveListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        EventManagerR.RemoveListener<GameEvents.LevelChangedEvent>(OnLevelChanged);
        EventManagerR.RemoveListener<GameEvents.ScoreChangedEvent>(OnScoreChanged);
        EventManagerR.RemoveListener<GameEvents.HealthChangedEvent>(OnHealthChanged);
        EventManagerR.RemoveListener<GameEvents.SpecialChangedEvent>(OnSpecialChanged);
        EventManagerR.RemoveListener<GameEvents.SpeedChangedEvent>(OnSpeedChanged);
        EventManagerR.RemoveListener<GameEvents.JumpDurationChangedEvent>(OnJumpDurationChanged);
    }
    
    private void Update()
    {
        UpdateTimeCounter();
    }
    
    private void OnGameStateChanged(GameEvents.GameStateChangedEvent evt)
    {
        switch (evt.NewGameState)
        {
            case GameModelR.GameState.Menu:
                // Handle other UI elements as needed
                break;
            case GameModelR.GameState.Running:
                break;
            case GameModelR.GameState.Paused:
                break;
            case GameModelR.GameState.GameOver:
                break;
        }
    }
    
    private void OnLevelChanged(GameEvents.LevelChangedEvent evt)
    {
        levelCounter.text = evt.NewLevel.ToString();
    }
    
    private void OnScoreChanged(GameEvents.ScoreChangedEvent evt)
    {
        scoreCounter.text = ((int)evt.NewScore).ToString();
    }
    
    private void OnHealthChanged(GameEvents.HealthChangedEvent evt)
    {
        healthBar.value = evt.NewHealth;
        healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
    }
    
    private void OnSpecialChanged(GameEvents.SpecialChangedEvent evt)
    {
        specialBar.value = evt.NewSpecial;
        specialFill.color = specialGradiant.Evaluate(specialBar.normalizedValue);
    }
    
    private void OnSpeedChanged(GameEvents.SpeedChangedEvent evt)
    {
        speedCounter.text = evt.NewSpeed.ToString();
    }
    
    private void OnJumpDurationChanged(GameEvents.JumpDurationChangedEvent evt)
    {
        jumpDurationCounter.text = evt.NewJumpDuration.ToString();
    }

    private void UpdateTimeCounter()
    {
        float elapsedTime = GameModelR.GetElapsedTime();
        timeCounter.text = FormatTime(elapsedTime);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void ToggleCountDown(bool setActive)
    {
        EventManagerR.Broadcast(new GameEvents.ToggleCountDownEvent(setActive));
    }
    
}