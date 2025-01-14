using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
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
        EventManager.AddListener<GameEvents.GameStateChangedEventR>(OnGameStateChanged);
        EventManager.AddListener<GameEvents.LevelChangedEventR>(OnLevelChanged);
        EventManager.AddListener<PlayerEvents.ScoreChangedEventR>(OnScoreChanged);
        EventManager.AddListener<PlayerEvents.HealthChangedEventR>(OnHealthChanged);
        EventManager.AddListener<PlayerEvents.SpecialChangedEventR>(OnSpecialChanged);
        EventManager.AddListener<PlayerEvents.SpeedChangedEventR>(OnSpeedChanged);
        EventManager.AddListener<PlayerEvents.JumpDurationChangedEventR>(OnJumpDurationChanged);
        EventManager.AddListener<PlayerEvents.SpecialActionEventR>(OnSpecialAction);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameEvents.GameStateChangedEventR>(OnGameStateChanged);
        EventManager.RemoveListener<GameEvents.LevelChangedEventR>(OnLevelChanged);
        EventManager.RemoveListener<PlayerEvents.ScoreChangedEventR>(OnScoreChanged);
        EventManager.RemoveListener<PlayerEvents.HealthChangedEventR>(OnHealthChanged);
        EventManager.RemoveListener<PlayerEvents.SpecialChangedEventR>(OnSpecialChanged);
        EventManager.RemoveListener<PlayerEvents.SpeedChangedEventR>(OnSpeedChanged);
        EventManager.RemoveListener<PlayerEvents.JumpDurationChangedEventR>(OnJumpDurationChanged);
        EventManager.RemoveListener<PlayerEvents.SpecialActionEventR>(OnSpecialAction);
    }
    
    private void Update()
    {
        UpdateTimeCounter();
    }
    
    private void OnGameStateChanged(GameEvents.GameStateChangedEventR evt)
    {
        switch (evt.NewGameState)
        {
            case GameModel.GameState.Menu:
                // Handle other UI elements as needed
                break;
            case GameModel.GameState.Running:
                break;
            case GameModel.GameState.Paused:
                break;
            case GameModel.GameState.GameOver:
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
        float elapsedTime = GameModel.GetElapsedTime();
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
        EventManager.Broadcast(new GameEvents.ToggleCountDownEventR(setActive));
    }
}