using UnityEngine;
using Utilities;

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

    private void TogglePauseGame()
    {
        if (isPlaying)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        _ui.SetPauseMenuUIVisibility(false);
        Time.timeScale = 1f; // Unpause time
        isPlaying = true;
    }

    public void Pause()
    {
        _ui.SetPauseMenuUIVisibility(true);
        Time.timeScale = 0f; // Pause time
        isPlaying = false;
    }

    private int UpdateTimer()
    {
        return Mathf.RoundToInt(playTime);
    }
}