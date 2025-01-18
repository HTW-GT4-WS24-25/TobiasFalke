using Events;
using Model;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;
using static Utility.GameConstants;

namespace Menus
{
    public class PauseMenuManager : MonoBehaviour
    {
        public UIDocument pauseUI;
        private Button continueButton;
        private Button quitButton;

        private void Awake()
        {
            pauseUI.rootVisualElement.style.display = DisplayStyle.None;
        }

        private void OnEnable()
        {
            quitButton = pauseUI.rootVisualElement.Q<Button>("QuitButton");
            continueButton = pauseUI.rootVisualElement.Q<Button>("ContinueButton");

            quitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
            continueButton?.RegisterCallback<ClickEvent>(OnClickContinueButton);
            EventManager.Add<LevelEvent.TogglePauseMenu>(OnTogglePauseMenu);
        }

        private void OnDisable()
        {
            continueButton?.UnregisterCallback<ClickEvent>(OnClickContinueButton);
            quitButton?.UnregisterCallback<ClickEvent>(OnClickExitButton);
            EventManager.Remove<LevelEvent.TogglePauseMenu>(OnTogglePauseMenu);
        }

        private static void OnClickContinueButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound(Audio.MenuClickSFX);
            EventManager.Trigger(new GameModel.GameStateChanged(GameModel.GameState.Running));
            SceneLoader.Instance.LoadScene(Scenes.Level);
        }

        private static void OnClickExitButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound(Audio.MenuClickSFX);
            Application.Quit();
        }

        private void OnTogglePauseMenu(LevelEvent.TogglePauseMenu evt)
        {
            pauseUI.rootVisualElement.style.display = evt.IsPaused ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}