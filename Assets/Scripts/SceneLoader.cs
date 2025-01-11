using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IScene
{
    public static IScene Instance { get; private set; } // singleton instance for easy access

    public const string MainMenuScene = "Main Menu";
    public const string GameOverScene = "Game Over";
    public const string GameLevelScene = "Main Scene";
    public const string TutorialScene = "Tutorial Scene";
        
    private void Awake()
    { 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void LoadScene(string sceneName)
    {
        AudioManager.Instance.StopTrack();
        // only load if scene is not already loaded
        if (SceneManager.GetActiveScene().name != sceneName) SceneManager.LoadScene(sceneName);
    }
}