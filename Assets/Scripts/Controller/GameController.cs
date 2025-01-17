using System;
using UnityEngine;
using Model;
using Events;

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
            DetermineInitialGameState();
        }
        
        private void RegisterEvents()
        {
            EventManager.AddListener<GameModel.GameStateChanged>(OnGameStateChanged);
            EventManager.AddListener<PlayerEvent.GameOverTriggered>(OnGameOverTriggered);
        }
        
        private void DetermineInitialGameState()
        {
            if (CompareTag("MainMenu"))
            {
                gameModel.CurrentGameState = GameModel.GameState.Menu;
            }
            else if (CompareTag("Level"))
            {
                gameModel.CurrentGameState = GameModel.GameState.Running;
            }
            else if (CompareTag("GameOver"))
            {
                gameModel.CurrentGameState = GameModel.GameState.Loose;
            }
        }
        
        private static void OnGameStateChanged(GameModel.GameStateChanged evt)
        {
            switch (evt.NewGameState)
            {
                case GameModel.GameState.Menu:
                    SceneLoader.Instance.LoadScene("MainMenu");
                    AudioManager.Instance.PlayTrack("MainMenuMusic");
                    break;
                case GameModel.GameState.Running:
                    Time.timeScale = 1f;
                    EventManager.Broadcast(new LevelEvent.TogglePauseMenu(false));
                    AudioManager.Instance.PlayTrack("MainSceneMusic");
                    break;
                case GameModel.GameState.Paused:
                    Time.timeScale = 0f;
                    EventManager.Broadcast(new LevelEvent.TogglePauseMenu(true));
                    break;
                case GameModel.GameState.Loose:
                    SceneLoader.Instance.LoadScene("Game Over");
                    AudioManager.Instance.PlayTrack("GameOverMusic");
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
            SceneLoader.Instance.LoadScene("Game Over");
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
            EventManager.RemoveListener<GameModel.GameStateChanged>(OnGameStateChanged);
            EventManager.RemoveListener<PlayerEvent.GameOverTriggered>(OnGameOverTriggered);
        }
    }
}