using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

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
        
        private void RegisterEvents()
        {
            EventManager.Add<LevelEvent.StageChanged>(OnStageChanged);
            EventManager.Add<LevelEvent.TimeElapsed>(OnTimeElapsed);
            EventManager.Add<PlayerEvent.HealthPointsChanged>(OnHealthChanged);
            EventManager.Add<PlayerEvent.ScorePointsChanged>(OnScoreChanged);
            EventManager.Add<PlayerEvent.SpecialPointsChanged>(OnSpecialChanged);
            EventManager.Add<PlayerEvent.SpeedMultiplierChanged>(OnSpeedMultiplierChanged);
            EventManager.Add<PlayerEvent.JumpDurationChanged>(OnJumpDurationChanged);
            EventManager.Add<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
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

        private void OnSpeedMultiplierChanged(PlayerEvent.SpeedMultiplierChanged evt)
        {
            speedCounter.text = evt.NewSpeedMultiplier.ToString();
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
        
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            EventManager.Remove<LevelEvent.StageChanged>(OnStageChanged);
            EventManager.Remove<PlayerEvent.ScorePointsChanged>(OnScoreChanged);
            EventManager.Remove<PlayerEvent.HealthPointsChanged>(OnHealthChanged);
            EventManager.Remove<PlayerEvent.SpecialPointsChanged>(OnSpecialChanged);
            EventManager.Remove<PlayerEvent.SpeedMultiplierChanged>(OnSpeedMultiplierChanged);
            EventManager.Remove<PlayerEvent.JumpDurationChanged>(OnJumpDurationChanged);
            EventManager.Remove<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
        }
    }
}