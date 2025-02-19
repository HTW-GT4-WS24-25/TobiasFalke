using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class Event
    {
    }

    public static class EventManager
    {
        private static readonly Dictionary<Type, Action<Event>> events = new();
        private static readonly Dictionary<Delegate, Action<Event>> eventLookup = new();

        public static void Add<T>(Action<T> listener) where T : Event
        {
            if (eventLookup.ContainsKey(listener)) return;
            Action<Event> newAction = (e) => listener((T)e);
            eventLookup[listener] = newAction;

            if (events.TryGetValue(typeof(T), out Action<Event> internalAction))
            {
                events[typeof(T)] = internalAction + newAction;
            }
            else
            {
                events[typeof(T)] = newAction;
            }
        }

        public static void Remove<T>(Action<T> listener) where T : Event
        {
            if (!eventLookup.TryGetValue(listener, out var action)) return;
            if (events.TryGetValue(typeof(T), out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null) events.Remove(typeof(T));
                else events[typeof(T)] = tempAction;
            }

            eventLookup.Remove(listener);
        }

        public static void Trigger(Event evt)
        {
            if (!events.TryGetValue(evt.GetType(), out var action)) return;
            try
            {
                action.Invoke(evt);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        public static void Clear()
        {
            events.Clear();
            eventLookup.Clear();
        }
    }
}