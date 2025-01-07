using UnityEngine;
using Utilities;

public class GameModel : PersistentSingleton<GameModel>
{
    public float CountDownTime
    {
        get { return countDownTime; } 
        set {countDownTime = value; }
        
    }
    public bool IsPlaying
    {
        get { return isPlaying; } 
        set{ isPlaying = value?true:false; }
    }
    public float PlayTime
    {
        get { return playTime; }
        set{ playTime = value; } 
    }
    public float GameSpeed
    {
        get { return gameSpeed; }
        set { gameSpeed = value; }
    }
    
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public int LevelDuration
    {
        get { return levelDuration; }
        set { levelDuration = value; }
    }
    

    private float playTime;
    private float gameSpeed = 10f;
    private bool isPlaying;
    private float countDownTime = 3f;
    private int level;
    private int levelDuration;

}