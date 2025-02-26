using Model;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;
using static Utility.GameConstants;

namespace Menus
{
    public class GameOverMenuManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private Button _tryAgainButton;
        private Button _returnButton;
        private Button _leaderboardButton;

        public UIDocument _leaderboardDocumnet;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _tryAgainButton = _uiDocument.rootVisualElement.Q<Button>("TryAgainButton");
            _returnButton = _uiDocument.rootVisualElement.Q<Button>("ReturnButton");
            _leaderboardButton = _uiDocument.rootVisualElement.Q<Button>("LeaderboardButton");
            
            RegisterButtonCallbacks();
        }

        private void RegisterButtonCallbacks()
        {
            _tryAgainButton?.RegisterCallback<ClickEvent>(OnClickTryAgainButton);
            _returnButton?.RegisterCallback<ClickEvent>(OnClickReturnButton);
            _leaderboardButton?.RegisterCallback<ClickEvent>(OnClickOpenLeaderboard);
        }

        private void OnClickOpenLeaderboard(ClickEvent evt)
        {
            _leaderboardDocumnet.sortingOrder = 1;
        }

        private static void OnClickReturnButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound(Audio.MenuClickSFX);
            SceneLoader.Instance.LoadScene(Scenes.MainMenu);
            EventManager.Trigger(new GameModel.GameStateChanged(GameModel.GameState.Menu));
        }

        private static void OnClickTryAgainButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound(Audio.MenuClickSFX);
            EventManager.Trigger(new GameModel.GameStateChanged(GameModel.GameState.Running));
            SceneLoader.Instance.LoadScene(Scenes.Level);
        }

        private void OnDisable()
        {
            UnregisterButtonCallbacks();
        }

        private void UnregisterButtonCallbacks()
        {
            _tryAgainButton.UnregisterCallback<ClickEvent>(OnClickTryAgainButton);
            _returnButton.UnregisterCallback<ClickEvent>(OnClickReturnButton);
            _leaderboardButton.UnregisterCallback<ClickEvent>(OnClickOpenLeaderboard);
        }
    }
}
