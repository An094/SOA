using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent<T> : ScriptableObject
{
    private List<Action<T>> _listeners = new();
    
    public void Raise(T value)
    {
        for(int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i]?.Invoke(value);
        }
    }

    public void Register(Action<T> callback) => _listeners.Add(callback);
    public void Unregister(Action<T> callback) => _listeners.Remove(callback);
}

[CreateAssetMenu(fileName = "New GameEvent", menuName = "GameEvents/GameEvent")]
public class GameEvent : GameEvent<Unit>
{
    public void Raise() => Raise(Unit.Default);
}

public struct Unit
{
    public static Unit Default = default;
}