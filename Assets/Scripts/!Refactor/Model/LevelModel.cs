using UnityEngine;

public class LevelModel
{
    private int currentStage;
    private static float stageSpeed;
    private float stageWidth;
    private float obstacleSpawnInterval;
    private float pickUpSpawnInterval;

    public LevelModel()
    {
        currentStage = 1;
        stageSpeed = 5f;
        stageWidth = 10f;
        obstacleSpawnInterval = 0.1f;
        pickUpSpawnInterval = 5f;
    }

    public int GetCurrentStage() => currentStage;

    public void SetCurrentStage(int newLevel) => currentStage = newLevel;

    public float GetStageWidth() => stageWidth;

    public void SetLevelWidth(float width) => stageWidth = width;

    // TODO: implement getters & setters for level height

    public float GetObstacleSpawnInterval() => obstacleSpawnInterval;

    public void SetObstacleSpawnInterval(float interval) => obstacleSpawnInterval = interval;

    public float GetPickUpSpawnInterval() => pickUpSpawnInterval;

    public void SetPickUpSpawnInterval(float interval) => pickUpSpawnInterval = interval;

    public static float GetStageSpeed() => stageSpeed;

    public static void SetStageSpeed(float speed) => stageSpeed = speed;

}