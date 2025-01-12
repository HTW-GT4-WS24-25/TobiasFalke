
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameViewR : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject countDownText;
    
    private void Awake()
    {
        EventManagerR.AddListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
    }

    private void OnDestroy()
    {
        EventManagerR.RemoveListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameEvents.GameStateChangedEvent evt)
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

    public void ToggleCountDown(bool setActive)
    {
        EventManagerR.Broadcast(new GameEvents.ToggleCountDownEvent(setActive));
    }
    
    public void ShowPauseMenu()
    {
    }

    public void HidePauseMenu()
    {
    }
}