using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class GameView : MonoBehaviour
    {
        public TextMeshProUGUI levelCounter;
        public TextMeshProUGUI timeCounter;
        public TextMeshProUGUI scoreCounter;
        // TODO: refactor status bars
        [SerializeField] private Slider healthBar;
        [SerializeField] private Gradient healthGradient;
        [SerializeField] private Image healthFill;
        [SerializeField] private Slider specialBar;
        [SerializeField] private Gradient specialGradiant;
        [SerializeField] private Image specialFill;
        public TextMeshProUGUI speedCounter;
        public TextMeshProUGUI jumpDurationCounter;
        public Image specialActionButtonPopUp;

        private void Start()
        {
            RegisterEvents();
        }
        
        private void OnLevelChanged(LevelEvents.StageChanged evt)
        {
            levelCounter.text = evt.NewStage.ToString();
        }
        
        private void OnTimeElapsed(LevelEvents.TimeElapsed evt)
        {
            timeCounter.text = FormatTime(evt.NewTime);
        }

        private void OnScoreChanged(PlayerEvents.ScoreChanged evt)
        {
            scoreCounter.text = ((int)evt.NewScore).ToString();
        }

        private void OnHealthChanged(PlayerEvents.HealthChanged evt)
        {
            healthBar.value = evt.NewHealth / 100;
            healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
        }

        private void OnSpecialChanged(PlayerEvents.SpecialChanged evt)
        {
            specialBar.value = evt.NewSpecial / 100;
            specialFill.color = specialGradiant.Evaluate(specialBar.normalizedValue);
            if (evt.NewSpecial == 100) specialActionButtonPopUp.gameObject.SetActive(true);
        }

        private void OnSpeedChanged(PlayerEvents.SpeedChanged evt)
        {
            speedCounter.text = evt.NewSpeed.ToString();
        }

        private void OnJumpDurationChanged(PlayerEvents.JumpDurationChanged evt)
        {
            jumpDurationCounter.text = evt.NewJumpDuration.ToString();
        }

        private void OnSpecialAction(PlayerEvents.SpecialActionTriggered evt)
        {
            specialActionButtonPopUp.gameObject.SetActive(false);
        }
        
        private string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
            return $"{minutes:0}:{seconds:00}";
        }
        
        private void RegisterEvents()
        {
            EventManager.AddListener<LevelEvents.StageChanged>(OnLevelChanged);
            EventManager.AddListener<LevelEvents.TimeElapsed>(OnTimeElapsed);
            EventManager.AddListener<PlayerEvents.ScoreChanged>(OnScoreChanged);
            EventManager.AddListener<PlayerEvents.HealthChanged>(OnHealthChanged);
            EventManager.AddListener<PlayerEvents.SpecialChanged>(OnSpecialChanged);
            EventManager.AddListener<PlayerEvents.SpeedChanged>(OnSpeedChanged);
            EventManager.AddListener<PlayerEvents.JumpDurationChanged>(OnJumpDurationChanged);
            EventManager.AddListener<PlayerEvents.SpecialActionTriggered>(OnSpecialAction);
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            EventManager.RemoveListener<LevelEvents.StageChanged>(OnLevelChanged);
            EventManager.RemoveListener<PlayerEvents.ScoreChanged>(OnScoreChanged);
            EventManager.RemoveListener<PlayerEvents.HealthChanged>(OnHealthChanged);
            EventManager.RemoveListener<PlayerEvents.SpecialChanged>(OnSpecialChanged);
            EventManager.RemoveListener<PlayerEvents.SpeedChanged>(OnSpeedChanged);
            EventManager.RemoveListener<PlayerEvents.JumpDurationChanged>(OnJumpDurationChanged);
            EventManager.RemoveListener<PlayerEvents.SpecialActionTriggered>(OnSpecialAction);
        }
    }
}