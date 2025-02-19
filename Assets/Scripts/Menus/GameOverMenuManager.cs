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

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _tryAgainButton = _uiDocument.rootVisualElement.Q("TryAgainButton") as Button;
            _returnButton = _uiDocument.rootVisualElement.Q("ReturnButton") as Button;
            _tryAgainButton?.RegisterCallback<ClickEvent>(OnClickTryAgainButton);
            _returnButton?.RegisterCallback<ClickEvent>(OnClickReturnButton);
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
            _tryAgainButton.UnregisterCallback<ClickEvent>(OnClickTryAgainButton);
            _returnButton.UnregisterCallback<ClickEvent>(OnClickReturnButton);
        }
    }
}
