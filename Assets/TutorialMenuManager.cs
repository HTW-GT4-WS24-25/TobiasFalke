using System;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialMenuManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _returnButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _returnButton = _uiDocument.rootVisualElement.Q<Button>("ReturnButton");
        
        _returnButton?.RegisterCallback<ClickEvent>(OnClickReturnButton);
    }

    private void OnClickReturnButton(ClickEvent evt)
    {
        AudioManager.Instance.PlaySound("clickPlay");
        SceneLoader.Instance.LoadScene(SceneLoader.MainMenuScene);
    }

    private void OnDisable()
    {
        _returnButton?.UnregisterCallback<ClickEvent>(OnClickReturnButton);
    }
}
