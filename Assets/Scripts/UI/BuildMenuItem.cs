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
    public Text crystalCost;
    public Text powerCost;

    public Color buildColor;
    public Color noBuildColor;

    private UnityAction resourceUpdateListener;

	// Use this for initialization
	void Start () {
        resourceUpdateListener = new UnityAction(UpdateAvailability);
        EventManager.StartListening("ResourcesChanged", resourceUpdateListener);
        UpdateAvailability();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetupItem(BuildingBase buildingBase, Action action)
    {
        this.text.text = buildingBase.GetComponent<HoverInfo>().displayName;
        this.image.sprite = buildingBase.GetComponent<SpriteRenderer>().sprite;
        this.buildingBase = buildingBase;
        if (buildingBase.oreCost == 0)
        {
            this.oreCost.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            this.oreCost.text = buildingBase.oreCost.ToString();
        }
        if (buildingBase.crystalCost == 0)
        {
            this.crystalCost.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            this.crystalCost.text = buildingBase.crystalCost.ToString();
        }
        if (buildingBase.powerCost == 0)
        {
            this.powerCost.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            this.powerCost.text = buildingBase.powerCost.ToString();
        }
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
