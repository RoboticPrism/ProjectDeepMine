using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {
    [Header("Prefab Connections")]
    public BuildingBlueprint blueprintPrefab;
    BuildingBlueprint instantiatedBlueprint;

    [Header("Instance Connections")]
    public List<BuildMenu> buildMenus;

    [HideInInspector]
    public static BuildManager instance;

	// Use this for initialization
	protected void Start () {
        instance = this;
        foreach (BuildMenu menu in buildMenus)
        {
            menu.openButton.onClick.AddListener(() => OpenBuildMenu(menu));
        }

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

    public void OpenBuildMenu(BuildMenu selectedMenu)
    {
        if(selectedMenu.open)
        {
            selectedMenu.CloseMenu();
        }
        else
        {
            foreach (BuildMenu menu in buildMenus)
            {
                if (menu == selectedMenu)
                {
                    menu.OpenMenu();
                }
                else
                {
                    menu.CloseMenu();
                }
            }
        }
    }
}
