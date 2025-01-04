using UnityEngine.SceneManagement;
using Utilities;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    private string _mainMenuScene = "Main Menu";
    private string _settingsMenuScene = "Settings";
    private string _gameScene = "Main Scene";
        
    public void LoadGameScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name != sceneName) SceneManager.LoadScene(sceneName);
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