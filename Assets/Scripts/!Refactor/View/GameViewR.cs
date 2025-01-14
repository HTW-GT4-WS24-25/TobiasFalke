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
        EventManagerR.AddListener<GameEvents.GameStateChangedEventR>(OnGameStateChanged);
        EventManagerR.AddListener<GameEvents.LevelChangedEventR>(OnLevelChanged);
        EventManagerR.AddListener<PlayerEvents.ScoreChangedEventR>(OnScoreChanged);
        EventManagerR.AddListener<PlayerEvents.HealthChangedEventR>(OnHealthChanged);
        EventManagerR.AddListener<PlayerEvents.SpecialChangedEventR>(OnSpecialChanged);
        EventManagerR.AddListener<PlayerEvents.SpeedChangedEventR>(OnSpeedChanged);
        EventManagerR.AddListener<PlayerEvents.JumpDurationChangedEventR>(OnJumpDurationChanged);
        EventManagerR.AddListener<PlayerEvents.SpecialActionEventR>(OnSpecialAction);
    }

    private void OnDestroy()
    {
        EventManagerR.RemoveListener<GameEvents.GameStateChangedEventR>(OnGameStateChanged);
        EventManagerR.RemoveListener<GameEvents.LevelChangedEventR>(OnLevelChanged);
        EventManagerR.RemoveListener<PlayerEvents.ScoreChangedEventR>(OnScoreChanged);
        EventManagerR.RemoveListener<PlayerEvents.HealthChangedEventR>(OnHealthChanged);
        EventManagerR.RemoveListener<PlayerEvents.SpecialChangedEventR>(OnSpecialChanged);
        EventManagerR.RemoveListener<PlayerEvents.SpeedChangedEventR>(OnSpeedChanged);
        EventManagerR.RemoveListener<PlayerEvents.JumpDurationChangedEventR>(OnJumpDurationChanged);
        EventManagerR.RemoveListener<PlayerEvents.SpecialActionEventR>(OnSpecialAction);
    }
    
    private void Update()
    {
        UpdateTimeCounter();
    }
    
    private void OnGameStateChanged(GameEvents.GameStateChangedEventR evt)
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
    
    private void OnLevelChanged(GameEvents.LevelChangedEventR evt)
    {
        levelCounter.text = evt.NewLevel.ToString();
    }
    
    private void OnScoreChanged(PlayerEvents.ScoreChangedEventR evt)
    {
        scoreCounter.text = ((int)evt.NewScore).ToString();
    }
    
    private void OnHealthChanged(PlayerEvents.HealthChangedEventR evt)
    {
        healthBar.value = evt.NewHealth;
        healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
    }
    
    private void OnSpecialChanged(PlayerEvents.SpecialChangedEventR evt)
    {
        specialBar.value = evt.NewSpecial;
        specialFill.color = specialGradiant.Evaluate(specialBar.normalizedValue);
        if (evt.NewSpecial == 100) specialActionButtonPopUp.gameObject.SetActive(true);
    }
    
    private void OnSpeedChanged(PlayerEvents.SpeedChangedEventR evt)
    {
        speedCounter.text = evt.NewSpeed.ToString();
    }
    
    private void OnJumpDurationChanged(PlayerEvents.JumpDurationChangedEventR evt)
    {
        jumpDurationCounter.text = evt.NewJumpDuration.ToString();
    }

    private void OnSpecialAction(PlayerEvents.SpecialActionEventR evt)
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
        EventManagerR.Broadcast(new GameEvents.ToggleCountDownEventR(setActive));
    }
}