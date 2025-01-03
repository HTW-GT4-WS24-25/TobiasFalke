using UnityEngine;
using Utilities;

// Oversees game states, flow, and manages score and high scores.

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private UIManager _ui;
    [SerializeField] private PlayerController _player;
    
    public float scrollSpeed = 10f;
    public bool isPlaying;
    private float playTime;

    private void Start()
    {
        isPlaying = true;
      
    }
    
    private void Update()
    {
        if (isPlaying)
        {
            playTime += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }

    private void LateUpdate()
    {
        _ui.timeUI.text = UpdateTimer().ToString();
    }

    public void TriggerGameOver()
    {
        isPlaying = false;
    }

    public void TogglePauseGame()
    {
        if (isPlaying)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Resume()
    {
        // pauseMenuUI.SetActive(true);
        Time.timeScale = 1f; // Unpause time
        isPlaying = true;
    }

    private void Pause()
    {
        // pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause time
        isPlaying = false;
    }

    private int UpdateTimer()
    {
        return Mathf.RoundToInt(playTime);
    }
}