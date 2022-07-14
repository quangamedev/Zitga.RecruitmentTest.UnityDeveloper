using System.Collections;
using System.Collections.Generic;
using DesignPatterns.Observer.ScriptableObjectsEventSystem.Advanced;
using UnityEngine;

/// <summary>
/// Use this SO to replace normal floats when usage and references of a primitive type is in high demand.
/// </summary>
public class ScriptableObjectVariable<T> : ScriptableObject
{
    [Tooltip("Value can be modified during runtime")]
    [SerializeField] private T _value;
    
    [SerializeField] private VoidGameEvent[] OnValueChangeEvents;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            RaiseEvents();
        }
    }

    protected virtual void RaiseEvents()
    {
        if (OnValueChangeEvents == null || OnValueChangeEvents.Length == 0) return;
        
        foreach (var voidGameEvent in OnValueChangeEvents)
        {
            voidGameEvent.Raise();
        }
    }
}