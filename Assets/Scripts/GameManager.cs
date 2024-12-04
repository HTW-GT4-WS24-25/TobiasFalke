using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion Singleton

    [SerializeField] private UIManager _ui;
    [SerializeField] private PlayerController _player;
    
    public float scrollSpeed = 10f;
    public bool isPlaying = true;
    private float playTime;

    private void Start()
    {
        _ui.SetMaxHealth(_player._maxHealth);
        _ui.SetMaxSpecial(_player._maxSpecial);
    }
    
    private void Update()
    {
        if (isPlaying)
        {
            playTime += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        _ui.timeUI.text = UpdateTimer().ToString();
    }

    public void TriggerGameOver()
    {
        isPlaying = false;
    }

    private int UpdateTimer()
    {
        return Mathf.RoundToInt(playTime);
    }
}