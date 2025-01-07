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
        Time.timeScale = 0f; // Pause at the start
        UIManager.Instance.ToggleCountDownVisibility(true);
    }

    private void Update()
    {
        // Handle countdown
        if (remainingTime > 0)
        {
            remainingTime -= Time.unscaledDeltaTime; // Use unscaled time
            float remainingSeconds = Mathf.Ceil(remainingTime);
            
            // Update countdown only if changed to reduce UI updates
            UIManager.Instance.UpdateCountDown(remainingSeconds);

            if (remainingTime <= 0)
            {
                StartGame();
            }
        }

        // Handle pause toggle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
            return;
        }
        
        // Only proceed if the game is playing
        if (!isPlaying) return;

        // Game running logic
        playTime += Time.deltaTime;
        int levelByTime = (int)(playTime / levelDuration);
        if (levelByTime > level) ChangeLevel(levelByTime);
    }

    private void StartGame()
    {
        isPlaying = true;
        remainingTime = 0;
        UIManager.Instance.ToggleCountDownVisibility(false);
        Time.timeScale = 1f; // Begin gameplay
        AudioManager.Instance.PlaySound("gameStart");
    }

    private void LateUpdate()
    {
        // Update time counter UI if game is playing
        if (isPlaying)
        {
            UIManager.Instance.timeCounter.text = Mathf.RoundToInt(playTime).ToString();
        }
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
        Time.timeScale = 1f; // Resume time
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