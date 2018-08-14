using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour {

    public int oreCount = 0;
    public int powerCount = 0;
    public int powerMax = 3;

    public Text oreText;
    public Text powerText;

    public static ResourceManager instance;

    private Dictionary<string, UnityEvent> eventDictionary  = new Dictionary<string, UnityEvent>();

    // Use this for initialization
    void Start () {
        instance = this;
        SetOre(oreCount);
        SetPower(powerCount);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetOre(int newOreCount)
    {
        oreCount = newOreCount;
        oreText.text = oreCount.ToString();
        TriggerEvent("ResourcesChanged");
    }

    public void AddOre(int newOre)
    {
        oreCount += newOre;
        oreText.text = oreCount.ToString();
        TriggerEvent("ResourcesChanged");
    }

    public void SetPower(int newPowerCount)
    {
        powerCount = newPowerCount;
        powerText.text = powerCount + " / " + powerMax;
        TriggerEvent("ResourcesChanged");
    }

    public void AddPower(int newPower)
    {
        powerCount += newPower;
        powerText.text = powerCount + " / " + powerMax;
        TriggerEvent("ResourcesChanged");
    }

    public void SetPowerMax(int newPowerCount)
    {
        powerMax = newPowerCount;
        powerText.text = powerCount + " / " + powerMax;
        TriggerEvent("ResourcesChanged");
    }

    public void AddPowerMax(int newPower)
    {
        powerMax += newPower;
        powerText.text = powerCount + " / " + powerMax;
        TriggerEvent("ResourcesChanged");
    }

    public int PowerAvailable()
    {
        return powerMax - powerCount;
    }

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

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (instance == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
