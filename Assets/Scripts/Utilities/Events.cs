using UnityEngine;

public static class Events
{
    public static GameOverEvent GameOverEvent = new GameOverEvent();
    public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
    public static PickupEvent PickupEvent = new PickupEvent();
    public static CollisionEvent CollisionEvent = new CollisionEvent();
    public static TrickEvent TrickEvent = new TrickEvent();
}

public class GameOverEvent : GameEvent
{
    public bool Win;
}

public class PlayerDeathEvent : GameEvent { }

public class PickupEvent : GameEvent
{
    public Item Item;
}

public class TrickEvent : GameEvent
{
    public int Points;
}

public class CollisionEvent : GameEvent
{
    public GameObject Sender;
    public int DamageValue;
}