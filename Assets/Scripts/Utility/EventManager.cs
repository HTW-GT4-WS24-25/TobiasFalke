using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
}

public static class EventManager
{
    private static readonly Dictionary<Type, Action<GameEvent>> events = new Dictionary<Type, Action<GameEvent>>();
    private static readonly Dictionary<Delegate, Action<GameEvent>> eventLookup = new Dictionary<Delegate, Action<GameEvent>>();

    public static void AddListener<T>(Action<T> listener) where T : GameEvent
    {
        if (!eventLookup.ContainsKey(listener))
        {
            Action<GameEvent> newAction = (e) => listener((T)e);
            eventLookup[listener] = newAction;

            if (events.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
            {
                events[typeof(T)] = internalAction += newAction;
            }
            else
            {
                events[typeof(T)] = newAction;
            }
        }
    }

    public static void RemoveListener<T>(Action<T> listener) where T : GameEvent
    {
        if (eventLookup.TryGetValue(listener, out var action))
        {
            if (events.TryGetValue(typeof(T), out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                {
                    events.Remove(typeof(T));
                }
                else
                {
                    events[typeof(T)] = tempAction;
                }
            }

            eventLookup.Remove(listener);
        }
    }

    public static void Broadcast(GameEvent evt)
    {
        if (events.TryGetValue(evt.GetType(), out var action))
        {
            try
            {
                action.Invoke(evt);
            }
            catch (Exception ex) {
              Debug.Log(ex);
            }
        }
    }

    public static void Clear()
    {
        events.Clear();
        eventLookup.Clear();
    }
}