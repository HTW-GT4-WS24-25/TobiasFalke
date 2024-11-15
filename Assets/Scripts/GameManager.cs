using UnityEngine;

// TODO: Ausbauen

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

    public float currentScore;
    public float scrollSpeed = 10f;
    public bool isPlaying = true;

    private void Update()
    {
        if (isPlaying)
        {
            currentScore += Time.deltaTime;
        }
            
    }

    public void GameOver()
    {
        currentScore = 0;
        isPlaying = false;
    }
        
    public int UpdateScore()
    {
        return Mathf.RoundToInt(currentScore);
    }
}