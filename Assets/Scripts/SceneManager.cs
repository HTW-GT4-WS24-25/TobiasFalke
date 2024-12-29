using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
public class SceneManager : PersistentSingleton<SceneManager>
{
    private string _mainMenuScene = "Main Menu";
    private string _settingsMenuScene = "Settings";
    private string _gameScene = "Main Scene";
        
    public void LoadScene(string sceneName)
    {
        // Logic to load a scene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != sceneName)
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }


    public void LoadMainMenu()
    {
        LoadScene(_mainMenuScene);
    }

    public void LoadSettingsMenu()
    {
        LoadScene(_settingsMenuScene);
    }

    public void LoadGame()
    {
        LoadScene(_gameScene);
    }
}