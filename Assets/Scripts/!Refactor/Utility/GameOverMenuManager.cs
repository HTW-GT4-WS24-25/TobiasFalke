using UnityEngine;
using UnityEngine.UIElements;

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

    private void OnClickReturnButton(ClickEvent evt)
    {
        AudioManagerR.Instance.PlaySound("clickPlay");
        SceneLoader.Instance.LoadScene(SceneLoader.mainMenu);
    }

    private void OnClickTryAgainButton(ClickEvent evt)
    {
        AudioManagerR.Instance.PlaySound("clickPlay");
        SceneLoader.Instance.LoadScene(SceneLoader.gameLevel);
    }

    private void OnDisable()
    {
        _tryAgainButton.UnregisterCallback<ClickEvent>(OnClickTryAgainButton);
        _returnButton.UnregisterCallback<ClickEvent>(OnClickReturnButton);
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
    }
}
