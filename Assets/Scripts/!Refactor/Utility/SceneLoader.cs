using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IScene
{
    public static IScene Instance { get; private set; } // singleton instance for easy access

    public const string mainMenu = "Main Menu";
    public const string gameOver = "Game Over";
    public const string gameLevel = "Main Scene";
        
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
        AudioManagerR.Instance.StopTrack();
        // only load if scene is not already loaded
        if (SceneManager.GetActiveScene().name != sceneName) SceneManager.LoadScene(sceneName);
    }
}