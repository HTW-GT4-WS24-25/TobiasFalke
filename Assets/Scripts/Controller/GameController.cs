using System;
using UnityEngine;
using Model;
using Events;
using Utility;
using static Utility.GameConstants;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        // TODO: make dedicated persistent singleton class? (example in previous branches)
        private static GameController Instance { get; set; }
        private GameModel gameModel;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                gameModel = new GameModel();
            }
            else Destroy(gameObject);
        }

        private void Start()
        {
            RegisterEvents();
            gameModel.CurrentGameState = GameModel.GameState.Menu;
        }
        
        private void RegisterEvents()
        {
            EventManager.Add<GameModel.GameStateChanged>(OnGameStateChanged);
            EventManager.Add<PlayerEvent.GameOverTriggered>(OnGameOverTriggered);
        }
        
        private static void OnGameStateChanged(GameModel.GameStateChanged evt)
        {
            Debug.Log("New game state " + evt.NewGameState);
            switch (evt.NewGameState)
            {
                case GameModel.GameState.Menu:
                    SceneLoader.Instance.LoadScene(Scenes.MainMenu);
                    AudioManager.Instance.PlayTrack(Audio.MainMenuBGM);
                    break;
                case GameModel.GameState.Running:
                    AudioManager.Instance.PlayTrack(Audio.LevelBGM);
                    Time.timeScale = 1f;
                    EventManager.Trigger(new LevelEvent.TogglePauseMenu(false));
                    break;
                case GameModel.GameState.Paused:
                    Time.timeScale = 0f;
                    EventManager.Trigger(new LevelEvent.TogglePauseMenu(true));
                    break;
                case GameModel.GameState.Loose:
                    SceneLoader.Instance.LoadScene(Scenes.GameOver);
                    AudioManager.Instance.PlayTrack(Audio.GameOverBGM);
                    break;
                case GameModel.GameState.Quit:
                    Application.Quit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnGameOverTriggered(PlayerEvent.GameOverTriggered evt)
        {
            gameModel.CurrentGameState = GameModel.GameState.Loose;
            SceneLoader.Instance.LoadScene(Scenes.GameOver);
            Debug.Log("Game Over triggered in game controller");
        }

        private void OnCancel()
        {
            gameModel.CurrentGameState = gameModel.CurrentGameState switch
            {
                GameModel.GameState.Running => GameModel.GameState.Paused,
                GameModel.GameState.Paused => GameModel.GameState.Running,
                GameModel.GameState.Loose => GameModel.GameState.Menu,
                GameModel.GameState.Menu => GameModel.GameState.Quit,
                _ => gameModel.CurrentGameState
            };
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            EventManager.Remove<GameModel.GameStateChanged>(OnGameStateChanged);
            EventManager.Remove<PlayerEvent.GameOverTriggered>(OnGameOverTriggered);
        }
    }
}