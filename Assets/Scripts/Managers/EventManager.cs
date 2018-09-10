using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

    public static EventManager instance;

    public class TileEvent : UnityEvent<TaskableBase> { };

    private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();
    private Dictionary<string, TileEvent> tileEventDictionary = new Dictionary<string, TileEvent>();

    // Use this for initialization
    void Start () {
        instance = this;
	}

    // Adds an event listener
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, UnityAction<TaskableBase> listener)
    {
        TileEvent thisEvent = null;
        if (instance.tileEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TileEvent();
            thisEvent.AddListener(listener);
            instance.tileEventDictionary.Add(eventName, thisEvent);
        }
    }

    // Removes an event listener
    public static void StopListening(string eventName, UnityAction listener)
    {
        if (instance == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void StopListening(string eventName, UnityAction<TaskableBase> listener)
    {
        if (instance == null) return;
        TileEvent thisEvent = null;
        if (instance.tileEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    // Triggers listeners
    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void TriggerEvent(string eventName, TaskableBase location)
    {
        TileEvent thisEvent = null;
        if (instance.tileEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(location);
        }
    }
}
