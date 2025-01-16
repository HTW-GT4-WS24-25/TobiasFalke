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
        
        private void OnLevelChanged(LevelEvent.StageChanged evt)
        {
            levelCounter.text = evt.NewStage.ToString();
        }
        
        private void OnTimeElapsed(LevelEvent.TimeElapsed evt)
        {
            timeCounter.text = FormatTime(evt.NewTime);
        }

        private void OnScoreChanged(PlayerEvent.ScoreChanged evt)
        {
            scoreCounter.text = ((int)evt.NewScore).ToString();
        }

        private void OnHealthChanged(PlayerEvent.HealthChanged evt)
        {
            healthBar.value = evt.NewHealth / 100;
            healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
        }

        private void OnSpecialChanged(PlayerEvent.SpecialChanged evt)
        {
            specialBar.value = evt.NewSpecial / 100;
            specialFill.color = specialGradiant.Evaluate(specialBar.normalizedValue);
            if (evt.NewSpecial == 100) specialActionButtonPopUp.gameObject.SetActive(true);
        }

        private void OnSpeedChanged(PlayerEvent.SpeedChanged evt)
        {
            speedCounter.text = evt.NewSpeed.ToString();
        }

        private void OnJumpDurationChanged(PlayerEvent.JumpDurationChanged evt)
        {
            jumpDurationCounter.text = evt.NewJumpDuration.ToString();
        }

        private void OnSpecialAction(PlayerEvent.SpecialActionTriggered evt)
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
            EventManager.AddListener<LevelEvent.StageChanged>(OnLevelChanged);
            EventManager.AddListener<LevelEvent.TimeElapsed>(OnTimeElapsed);
            EventManager.AddListener<PlayerEvent.ScoreChanged>(OnScoreChanged);
            EventManager.AddListener<PlayerEvent.HealthChanged>(OnHealthChanged);
            EventManager.AddListener<PlayerEvent.SpecialChanged>(OnSpecialChanged);
            EventManager.AddListener<PlayerEvent.SpeedChanged>(OnSpeedChanged);
            EventManager.AddListener<PlayerEvent.JumpDurationChanged>(OnJumpDurationChanged);
            EventManager.AddListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            EventManager.RemoveListener<LevelEvent.StageChanged>(OnLevelChanged);
            EventManager.RemoveListener<PlayerEvent.ScoreChanged>(OnScoreChanged);
            EventManager.RemoveListener<PlayerEvent.HealthChanged>(OnHealthChanged);
            EventManager.RemoveListener<PlayerEvent.SpecialChanged>(OnSpecialChanged);
            EventManager.RemoveListener<PlayerEvent.SpeedChanged>(OnSpeedChanged);
            EventManager.RemoveListener<PlayerEvent.JumpDurationChanged>(OnJumpDurationChanged);
            EventManager.RemoveListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
        }
    }
}