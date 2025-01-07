using UnityEngine;
using Utilities;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private BackgroundScroller _backgroundScroller;

    public bool isPlaying = false;
    public float gameSpeed = 10f;
   
    private float playTime;
    private const float countDownTime = 3f;
    private float remainingTime;

    public int levelDuration = 30;
    public int level;

    private void Start()
    {
        remainingTime = countDownTime;
        Time.timeScale = 0f;
        UIManager.Instance.ToggleCountDownVisibility(true);
    }
    
    private void Update()
    {        
        switch (remainingTime)
        {
            case > 0:
            {
                remainingTime -= Time.unscaledDeltaTime;
                float remainingSeconds = remainingTime % 60;
                UIManager.Instance.UpdateCountDown(remainingSeconds);
                break;
            }
            case <= 0:
                isPlaying = true;
                remainingTime = 0;
                UIManager.Instance.ToggleCountDownVisibility(false);
                Time.timeScale = 1f;
                AudioManager.Instance.PlaySound("gameStart");
                break;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePauseMenu(); // open or close pause menu on ESC
        if (!isPlaying) return; // only continue if game is running
        playTime += Time.deltaTime;

        int levelByTime = (int)(playTime / levelDuration);
        if (levelByTime > level) ChangeLevel(levelByTime);
    }

    private void LateUpdate()
    {
        UIManager.Instance.timeCounter.text = Mathf.RoundToInt(playTime).ToString();
    }
    
    private void TogglePauseMenu()
    {
        if (isPlaying)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        UIManager.Instance.SetPauseMenuUIVisibility(false);
        Time.timeScale = 1f; // Unpause time
        isPlaying = true;
    }

    public void PauseGame()
    {
        UIManager.Instance.SetPauseMenuUIVisibility(true);
        Time.timeScale = 0f; // Pause time
        isPlaying = false;
    }

    private void ChangeLevel(int newLevel)
    {
        gameSpeed = 2.5f * newLevel + 10f;
        UIManager.Instance.UpdateLevelCounter(newLevel);
        _backgroundScroller.UpdateLevelBackground(newLevel);
        level = newLevel;
    }
}