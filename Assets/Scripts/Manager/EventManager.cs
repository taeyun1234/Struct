using System;
using System.Collections.Generic;

public enum GameEventType
{
    GameStart,
    GameOver,
    PlayerDead,
    ScoreUpdated,
    LevelUp
}

public class EventManager
{
    private Dictionary<GameEventType, Action<object>> _eventDictionary = new Dictionary<GameEventType, Action<object>>();

    public void Subscribe(GameEventType eventType, Action<object> listener)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType] += listener;
        }
        else
        {
            _eventDictionary.Add(eventType, listener);
        }
    }

    public void Unsubscribe(GameEventType eventType, Action<object> listener)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType] -= listener;
        }
    }

    public void Trigger(GameEventType eventType, object param = null)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType]?.Invoke(param);
        }
    }

    public void Clear()
    {
        _eventDictionary.Clear();
    }
}