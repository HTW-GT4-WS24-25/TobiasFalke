using UnityEngine;

public static class Events
{
    public static GameOverEvent GameOverEvent = new GameOverEvent();
    public static PickupEvent PickupEvent = new PickupEvent();
    public static ObstacleCollisionEvent ObstacleCollisionEvent = new ObstacleCollisionEvent();
    public static ObstacleCollisionExitEvent ObstacleCollisionExitEvent = new ObstacleCollisionExitEvent();
    public static TrickEvent TrickEvent = new TrickEvent();
}

public class GameOverEvent : GameEvent
{
    public float Score;
}

public class GameStartEvent : GameEvent {}

public class PickupEvent : GameEvent
{
    public ItemType ItemType;
}

public class TrickEvent : GameEvent
{
    public int Points;
}

public class ObstacleCollisionEvent : GameEvent
{
    public Obstacle Obstacle;
    public int DamageValue;
}

public class ObstacleCollisionExitEvent : GameEvent
{
    public Obstacle Obstacle;
}