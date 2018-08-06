using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {

    public int oreCount = 0;
    public int powerCount = 0;
    public int powerAvailable = 3;

    public Text oreText;
    public Text powerText;

	// Use this for initialization
	void Start () {
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
    }

    public void AddOre(int newOre)
    {
        oreCount += newOre;
        oreText.text = oreCount.ToString();
    }

    public void SetPower(int newPowerCount)
    {
        powerCount = newPowerCount;
        powerText.text = powerCount + " / " + powerAvailable;
    }

    public void AddPower(int newPower)
    {
        powerCount = newPower;
        powerText.text = powerCount + " / " + powerAvailable;
    }

    public void SetPowerAvailable(int newPowerCount)
    {
        powerAvailable = newPowerCount;
        powerText.text = powerCount + " / " + powerAvailable;
    }

    public void AddPowerAvailable(int newPower)
    {
        powerAvailable = newPower;
        powerText.text = powerCount + " / " + powerAvailable;
    }
}
