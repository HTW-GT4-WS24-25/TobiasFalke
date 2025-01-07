using UnityEngine;
using Utilities;

public class GameModel : PersistentSingleton<GameModel>
{
    public float CountDownTime
    {
        get { return countDownTime; } 
        set {countDownTime = value; }
        
    }
    public float ScrollSpeed
    {
        get { return scrollSpeed; }
        set { scrollSpeed = value; }
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

    private float playTime;
    private bool isPlaying;
    private float scrollSpeed = 10f;
    private float countDownTime = 3f;
    
}