using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
public class GameSceneManager : PersistentSingleton<GameSceneManager>
{
    private string _mainMenuScene = "Main Menu";
    private string _settingsMenuScene = "Settings";
    private string _gameScene = "Main Scene";
        
    public void LoadGameScene(string sceneName)
    {
        // Logic to load a scene
        if (SceneManager.GetActiveScene().name != sceneName)
            
            SceneManager.LoadScene(sceneName);
    }


    public void LoadMainMenu()
    {
        LoadGameScene(_mainMenuScene);
    }

    public void LoadSettingsMenu()
    {
        LoadGameScene(_settingsMenuScene);
    }

    public void LoadGame()
    {
        LoadGameScene(_gameScene);
    }
}