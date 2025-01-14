using Events;
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
        EventManager.AddListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        EventManager.AddListener<GameEvents.LevelChangedEvent>(OnLevelChanged);
        EventManager.AddListener<PlayerEvents.ScoreChangedEvent>(OnScoreChanged);
        EventManager.AddListener<PlayerEvents.HealthChangedEvent>(OnHealthChanged);
        EventManager.AddListener<PlayerEvents.SpecialChangedEvent>(OnSpecialChanged);
        EventManager.AddListener<PlayerEvents.SpeedChangedEvent>(OnSpeedChanged);
        EventManager.AddListener<PlayerEvents.JumpDurationChangedEvent>(OnJumpDurationChanged);
        EventManager.AddListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        EventManager.RemoveListener<GameEvents.LevelChangedEvent>(OnLevelChanged);
        EventManager.RemoveListener<PlayerEvents.ScoreChangedEvent>(OnScoreChanged);
        EventManager.RemoveListener<PlayerEvents.HealthChangedEvent>(OnHealthChanged);
        EventManager.RemoveListener<PlayerEvents.SpecialChangedEvent>(OnSpecialChanged);
        EventManager.RemoveListener<PlayerEvents.SpeedChangedEvent>(OnSpeedChanged);
        EventManager.RemoveListener<PlayerEvents.JumpDurationChangedEvent>(OnJumpDurationChanged);
        EventManager.RemoveListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
    }

    private void Update()
    {
        UpdateTimeCounter();
    }

    private void OnGameStateChanged(GameEvents.GameStateChangedEvent evt)
    {
        switch (evt.NewGameState)
        {
            // TODO: potential visual effects on game state changes?
            case GameModel.GameState.Menu:
                break;
            case GameModel.GameState.Running:
                break;
            case GameModel.GameState.Paused:
                break;
            case GameModel.GameState.GameOver:
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
        healthBar.value = evt.NewHealth / 100;
        healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
    }

    private void OnSpecialChanged(PlayerEvents.SpecialChangedEvent evt)
    {
        specialBar.value = evt.NewSpecial / 100;
        specialFill.color = specialGradiant.Evaluate(specialBar.normalizedValue);
        if (evt.NewSpecial == 100)
            specialActionButtonPopUp.gameObject.SetActive(true);
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
        float elapsedTime = GameModel.GetElapsedTime();
        timeCounter.text = FormatTime(elapsedTime);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
        return "{minutes:0}:{seconds:00}";
    }
}