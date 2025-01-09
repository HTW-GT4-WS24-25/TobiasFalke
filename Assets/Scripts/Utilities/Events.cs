using UnityEngine;

public static class Events
{
    public static GameOverEvent GameOverEvent = new GameOverEvent();
    public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
    public static PickupEvent PickupEvent = new PickupEvent();
    public static ObstacleCollisionEvent ObstacleCollisionEvent = new ObstacleCollisionEvent();
    public static ObstacleCollisionExitEvent ObstacleCollisionExitEvent = new ObstacleCollisionExitEvent();
    public static TrickEvent TrickEvent = new TrickEvent();
}

public class GameOverEvent : GameEvent
{
    public bool Win;
}

public class PlayerDeathEvent : GameEvent { }

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