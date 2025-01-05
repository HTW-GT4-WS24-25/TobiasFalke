using UnityEngine;
using Utilities;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private UIManager _ui;
    [SerializeField] private PlayerController _player;
    
    [SerializeField]private float countDownTime = 3f;
    public float scrollSpeed = 10f;
    public bool isPlaying;
    private float playTime;
    private float remainingTime;

    private void Start()
    {
        remainingTime = countDownTime;
        isPlaying = false;
        Time.timeScale = 0f; // Unpause time
        _ui.ToggleCountDownVisibility(true);

    }
    
    private void Update()
    {        
        if (remainingTime > 0)
        {
            remainingTime -= Time.unscaledDeltaTime;
            float remainingSeconds = remainingTime % 60;
            _ui.UpdateCountDown(remainingSeconds);
        } 
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            isPlaying = true;
            _ui.ToggleCountDownVisibility(false);
            Time.timeScale = 1f; // Unpause time
            AudioManager.Instance.PlaySound("openSettings");
        }
        
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