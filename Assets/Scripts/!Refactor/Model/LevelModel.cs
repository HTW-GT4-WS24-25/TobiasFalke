using UnityEngine;

public class LevelModel
{
    private float obstacleSpawnInterval;
    private float pickUpSpawnInterval;
    private static float levelSpeed;
    private float levelWidth;

    public LevelModel()
    {
        obstacleSpawnInterval = 0.5f;
        pickUpSpawnInterval = 5f;
        levelSpeed = 1f;
        levelWidth = 10f;
    }

    public float GetObstacleSpawnInterval()
    {
        return obstacleSpawnInterval;
    }

    public void SetObstacleSpawnInterval(float interval)
    {
        obstacleSpawnInterval = interval;
    }

    public float GetPickUpSpawnInterval()
    {
        return pickUpSpawnInterval;
    }

    public void SetPickUpSpawnInterval(float interval)
    {
        pickUpSpawnInterval = interval;
    }

    public static float GetLevelSpeed()
    {
        return levelSpeed;
    }

    public static void SetLevelSpeed(float speed)
    {
        levelSpeed = speed;
    }

    public float GetLevelWidth()
    {
        return levelWidth;
    }

    public void SetLevelWidth(float width)
    {
        levelWidth = width;
    }
}