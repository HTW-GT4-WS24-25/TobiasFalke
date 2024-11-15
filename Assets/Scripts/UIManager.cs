using TMPro;
using UnityEngine;

// TODO: Ausbauen, health und SP Bar hinzufügen

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;

    private GameManager _gm;

    private void Start()
    {
        _gm = GameManager.Instance;
    }

    private void OnGUI()
    {
        scoreUI.text = _gm.UpdateScore().ToString();
    }
}