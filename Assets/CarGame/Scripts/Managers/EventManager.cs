using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public static class EventManager
{
    static Dictionary<string, Delegate> events = new Dictionary<string, Delegate>();

    private static bool IsCompatible(Action a1, Action a2) =>
        a1.GetType() == a2.GetType();

    private static bool IsCompatible<T>(Action<T> a1, Action<T> a2) =>
        a1.GetType() == a2.GetType();

    private static bool IsCompatible<T, U>(Action<T, U> a1, Action<T, U> a2) =>
        a1.GetType() == a2.GetType();

    private static bool WasRegistered(string eventName)
    {
        bool result = events.ContainsKey(eventName);
        if(!result)
            Debug.Log($"Key:{eventName} does not exist");

        return result;
    }

    public static void AddListener(string eventName, Action action)
    {
        if (events.ContainsKey(eventName))
        {
            if (!IsCompatible((Action)events[eventName], action))
            {
                Debug.LogError("Incompatible delegate types to combine");
                return;
            }
            events[eventName] = Delegate.Combine(events[eventName], action);
        }
        else
            events[eventName] = action;
    }

    public static void RemoveListener(string eventName, Action action)
    {
        if (!IsCompatible((Action)events[eventName], action))
        {
            Debug.LogError("Incompatible delegate types to remove");
            return;
        }

        events[eventName] = (Action)events[eventName] - action;
    }

    public static void NotifyEvent(string eventName)
    {
        if (!WasRegistered(eventName)) return;

        Delegate[] listeners = events[eventName]?.GetInvocationList();

        if (listeners == null) return;

        foreach (Delegate listener in listeners)
            listener?.DynamicInvoke();
    }

    public static void AddListener<T>(string eventName, Action<T> action)
    {
        if (events.ContainsKey(eventName))
        {
            if (!IsCompatible((Action<T>)events[eventName], action))
            {
                Debug.LogError("Incompatible delegate types to combine");
                return;
            }

            events[eventName] = Delegate.Combine(events[eventName], action);
        }
        else
            events[eventName] = action;
    }

    public static void RemoveListener<T>(string eventName, Action<T> action)
    {
        if (!IsCompatible((Action<T>)events[eventName], action))
        {
            Debug.LogError("Incompatible delegate types to remove");
            return;
        }

        events[eventName] = (Action<T>)events[eventName] - action;
    }

    public static void NotifyEvent<T>(string eventName, T param1)
    {
        if (!WasRegistered(eventName)) return;

        Delegate[] listeners = events[eventName]?.GetInvocationList();

        foreach (Delegate listener in listeners)
            listener?.DynamicInvoke(param1);
    }

    public static void AddListener<T, U>(string eventName, Action<T, U> action)
    {
        if (events.ContainsKey(eventName))
        {
            if (!IsCompatible((Action<T, U>)events[eventName], action))
            {
                Debug.LogError("Incompatible delegate types to combine");
                return;
            }
            events[eventName] = Delegate.Combine(events[eventName], action);
        }
        else
            events[eventName] = action;
    }

    public static void RemoveListener<T, U>(string eventName, Action<T, U> action)
    {
        if (!IsCompatible((Action<T, U>)events[eventName], action))
        {
            Debug.LogError("Incompatible delegate types to remove");
            return;
        }

        events[eventName] = (Action<T, U>)events[eventName] - action;
    }

    public static void NotifyEvent<T, U>(string eventName, T param1, U param2)
    {
        if(!WasRegistered(eventName)) return;

        Delegate[] listeners = events[eventName]?.GetInvocationList();

        foreach (Delegate listener in listeners)
            listener?.DynamicInvoke(param1, param2);
    }
}
