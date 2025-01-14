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

    public Image specialActionButtonPopUp;
    
    private void Awake()
    {
        EventManagerR.AddListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        EventManagerR.AddListener<GameEvents.LevelChangedEvent>(OnLevelChanged);
        EventManagerR.AddListener<PlayerEvents.ScoreChangedEvent>(OnScoreChanged);
        EventManagerR.AddListener<PlayerEvents.HealthChangedEvent>(OnHealthChanged);
        EventManagerR.AddListener<PlayerEvents.SpecialChangedEvent>(OnSpecialChanged);
        EventManagerR.AddListener<PlayerEvents.SpeedChangedEvent>(OnSpeedChanged);
        EventManagerR.AddListener<PlayerEvents.JumpDurationChangedEvent>(OnJumpDurationChanged);
        EventManagerR.AddListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
    }

    private void OnDestroy()
    {
        EventManagerR.RemoveListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        EventManagerR.RemoveListener<GameEvents.LevelChangedEvent>(OnLevelChanged);
        EventManagerR.RemoveListener<PlayerEvents.ScoreChangedEvent>(OnScoreChanged);
        EventManagerR.RemoveListener<PlayerEvents.HealthChangedEvent>(OnHealthChanged);
        EventManagerR.RemoveListener<PlayerEvents.SpecialChangedEvent>(OnSpecialChanged);
        EventManagerR.RemoveListener<PlayerEvents.SpeedChangedEvent>(OnSpeedChanged);
        EventManagerR.RemoveListener<PlayerEvents.JumpDurationChangedEvent>(OnJumpDurationChanged);
        EventManagerR.RemoveListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
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
    
    private void OnScoreChanged(PlayerEvents.ScoreChangedEvent evt)
    {
        scoreCounter.text = ((int)evt.NewScore).ToString();
    }
    
    private void OnHealthChanged(PlayerEvents.HealthChangedEvent evt)
    {
        healthBar.value = evt.NewHealth;
        healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
    }
    
    private void OnSpecialChanged(PlayerEvents.SpecialChangedEvent evt)
    {
        specialBar.value = evt.NewSpecial;
        specialFill.color = specialGradiant.Evaluate(specialBar.normalizedValue);
        if (evt.NewSpecial == 100) specialActionButtonPopUp.gameObject.SetActive(true);
    }
    
    private void OnSpeedChanged(PlayerEvents.SpeedChangedEvent evt)
    {
        speedCounter.text = evt.NewSpeed.ToString();
    }
    
    private void OnJumpDurationChanged(PlayerEvents.JumpDurationChangedEvent evt)
    {
        jumpDurationCounter.text = evt.NewJumpDuration.ToString();
    }

    private void OnSpecialAction(PlayerEvents.SpecialActionEvent evt)
    {
        specialActionButtonPopUp.gameObject.SetActive(false);
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
        return $"{minutes:0}:{seconds:00}";
    }

    public void ToggleCountDown(bool setActive)
    {
        EventManagerR.Broadcast(new GameEvents.ToggleCountDownEvent(setActive));
    }
}