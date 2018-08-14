using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildMenuItem : MonoBehaviour {

    public Text text;
    public Image image;
    public Button button;
    public BuildingBase buildingBase;
    public delegate void Action(BuildingBase buildingBase);

    public Text oreCost;
    public Text powerCost;

    public Color buildColor;
    public Color noBuildColor;

    private UnityAction resourceUpdateListener;

	// Use this for initialization
	void Start () {
        resourceUpdateListener = new UnityAction(UpdateAvailability);
        ResourceManager.StartListening("ResourcesChanged", resourceUpdateListener);
        UpdateAvailability();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetupItem(BuildingBase buildingBase, Action action)
    {
        this.text.text = buildingBase.displayName;
        this.image.sprite = buildingBase.GetComponent<SpriteRenderer>().sprite;
        this.buildingBase = buildingBase;
        this.oreCost.text = buildingBase.oreCost.ToString();
        this.powerCost.text = buildingBase.powerCost.ToString();
        this.button.onClick.AddListener(delegate { action(buildingBase); });
    }

    public void UpdateAvailability()
    {
        bool canBuild = true;
            
        
        if (buildingBase.oreCost <= ResourceManager.instance.oreCount)
        {
            oreCost.color = buildColor;
        }
        else
        {
            oreCost.color = noBuildColor;
            canBuild = false;
        }
        if (buildingBase.powerCost <= ResourceManager.instance.PowerAvailable())
        {
            powerCost.color = buildColor;
        } 
        else
        {
            powerCost.color = noBuildColor;
            canBuild = false;
        }

        button.interactable = canBuild;
    }
}
