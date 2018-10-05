using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour {

    public int oreCount = 0;
    public int powerCount = 0;
    public int powerMax = 3;

    public int blueGemsCount = 0;

    public float seismicActivity = 0; // This effects how fast waves occur

    public Text oreText;
    public Text powerText;

    public static ResourceManager instance;

    public PopupText textPopupPrefab;
    public Sprite oreSprite;
    public Sprite powerSprite;
    public Sprite blueGemSprite;

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
        EventManager.TriggerEvent("ResourcesChanged");
    }

    public void AddOre(int newOre, Vector3 location)
    {
        oreCount += newOre;
        oreText.text = oreCount.ToString();
        EventManager.TriggerEvent("ResourcesChanged");
        CreatePopupText(location, newOre, oreSprite);
    }

    public void SetPower(int newPowerCount)
    {
        powerCount = newPowerCount;
        powerText.text = powerCount + " / " + powerMax;
        EventManager.TriggerEvent("ResourcesChanged");
    }

    public void AddPower(int newPower, Vector3 location)
    {
        powerCount += newPower;
        powerText.text = powerCount + " / " + powerMax;
        EventManager.TriggerEvent("ResourcesChanged");
        CreatePopupText(location, newPower, powerSprite);
    }

    public void SetPowerMax(int newPowerCount)
    {
        powerMax = newPowerCount;
        powerText.text = powerCount + " / " + powerMax;
        EventManager.TriggerEvent("ResourcesChanged");
    }

    public void AddPowerMax(int newPower, Vector3 location)
    {
        powerMax += newPower;
        powerText.text = powerCount + " / " + powerMax;
        EventManager.TriggerEvent("ResourcesChanged");
        CreatePopupText(location, newPower, powerSprite);
    }

    public int PowerAvailable()
    {
        return powerMax - powerCount;
    }

    public void AddBlueGems(int newGems, Vector3 location)
    {
        blueGemsCount += newGems;
        EventManager.TriggerEvent("ResourcesChanged");
        CreatePopupText(location, newGems, blueGemSprite);
    }

    public void AddSeismicActivity(float newSeismic)
    {
        seismicActivity += newSeismic;
        EventManager.TriggerEvent("ResourcesChanged");
    }

    public void CreatePopupText(Vector3 location, float amount, Sprite sprite)
    {
        if (amount != 0)
        {
            string text = amount.ToString();
            if (amount > 0)
            {
                text = "+" + text;
            }
            Instantiate(textPopupPrefab, location, Quaternion.Euler(Vector3.zero)).Setup(text, sprite);
        }
    }
}
