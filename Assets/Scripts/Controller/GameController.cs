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
        private InputManager playerInput;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                gameModel = new GameModel();
                playerInput = InputManager.Instance;
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
            playerInput.OnEscapeButtonPressed += EscapeAction;
            EventManager.AddListener<GameModel.StateChanged>(OnGameStateChanged);
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
        
        private static void OnGameStateChanged(GameModel.StateChanged evt)
        {
            switch (evt.NewGameState)
            {
                case GameModel.GameState.Menu:
                    SceneLoader.Instance.LoadScene("MainMenu");
                    break;
                case GameModel.GameState.Running:
                    Time.timeScale = 1f;
                    EventManager.Broadcast(new LevelEvent.TogglePauseMenu(false));
                    break;
                case GameModel.GameState.Paused:
                    Time.timeScale = 0f;
                    EventManager.Broadcast(new LevelEvent.TogglePauseMenu(true));
                    break;
                case GameModel.GameState.Loose:
                    SceneLoader.Instance.LoadScene("Game Over");
                    break;
                case GameModel.GameState.Quit:
                    Application.Quit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EscapeAction()
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
            playerInput.OnEscapeButtonPressed -= EscapeAction;
            EventManager.RemoveListener<GameModel.StateChanged>(OnGameStateChanged);
        }
    }
}