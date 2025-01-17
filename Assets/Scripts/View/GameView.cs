using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class GameView : MonoBehaviour
    {
        public TextMeshProUGUI stageCounter;
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
        
        private void OnStageChanged(LevelEvent.StageChanged evt)
        {
            stageCounter.text = evt.NewStage.ToString();
        }
        
        private void OnTimeElapsed(LevelEvent.TimeElapsed evt)
        {
            timeCounter.text = FormatTime(evt.NewTime);
        }

        private void OnScoreChanged(PlayerEvent.ScorePointsChanged evt)
        {
            scoreCounter.text = ((int)evt.NewScorePoints).ToString();
        }

        private void OnHealthChanged(PlayerEvent.HealthPointsChanged evt)
        {
            healthBar.value = evt.NewHealthPoints / 100;
            healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
        }

        private void OnSpecialChanged(PlayerEvent.SpecialPointsChanged evt)
        {
            specialBar.value = evt.NewSpecialPoints / 100;
            specialFill.color = specialGradiant.Evaluate(specialBar.normalizedValue);
            if (evt.NewSpecialPoints == 100) specialActionButtonPopUp.gameObject.SetActive(true);
        }

        private void OnSpeedChanged(PlayerEvent.SpeedChanged evt)
        {
            speedCounter.text = evt.NewSpeed + " km/h";
        }

        private void OnJumpDurationChanged(PlayerEvent.JumpDurationChanged evt)
        {
            jumpDurationCounter.text = evt.NewJumpDuration + " sec";
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
            EventManager.AddListener<LevelEvent.StageChanged>(OnStageChanged);
            EventManager.AddListener<LevelEvent.TimeElapsed>(OnTimeElapsed);
            EventManager.AddListener<PlayerEvent.ScorePointsChanged>(OnScoreChanged);
            EventManager.AddListener<PlayerEvent.HealthPointsChanged>(OnHealthChanged);
            EventManager.AddListener<PlayerEvent.SpecialPointsChanged>(OnSpecialChanged);
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
            EventManager.RemoveListener<LevelEvent.StageChanged>(OnStageChanged);
            EventManager.RemoveListener<PlayerEvent.ScorePointsChanged>(OnScoreChanged);
            EventManager.RemoveListener<PlayerEvent.HealthPointsChanged>(OnHealthChanged);
            EventManager.RemoveListener<PlayerEvent.SpecialPointsChanged>(OnSpecialChanged);
            EventManager.RemoveListener<PlayerEvent.SpeedChanged>(OnSpeedChanged);
            EventManager.RemoveListener<PlayerEvent.JumpDurationChanged>(OnJumpDurationChanged);
            EventManager.RemoveListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
        }
    }
}