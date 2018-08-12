using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {
    
    public BuildingBlueprint blueprintPrefab;
    public BuildingBlueprint instantiatedBlueprint;

    public RectTransform menuParent;
    public BuildMenuItem menuItemPrefab;

    public List<BuildingBase> buildings;

	// Use this for initialization
	protected void Start () {
        CreateMenuItems();
	}
	
	// Update is called once per frame
	protected void Update () {
        
	}

    public void SelectBuild(BuildingBase building)
    {
        if(instantiatedBlueprint)
        {
            instantiatedBlueprint.DestroySelf();
        }
        instantiatedBlueprint = Instantiate(blueprintPrefab);
        instantiatedBlueprint.SetupBlueprint(building);
    }

    // creates a new menu item
    public BuildMenuItem CreateMenuItem (BuildingBase buildingBase)
    {
        BuildMenuItem item = Instantiate(menuItemPrefab, menuParent);
        item.SetupItem(buildingBase, SelectBuild);
        return item;
    }

    public void CreateMenuItems ()
    {
        int i = 0;
        foreach (BuildingBase buildingBase in buildings)
        {
            RectTransform rt = CreateMenuItem(buildingBase).GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -90 - (i * 170));
            i++;
        }
        menuParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20 + (i * 170));
    }
}
