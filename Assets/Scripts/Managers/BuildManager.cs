using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : ClickableTileBase {
    
    public BuildingBlueprint blueprintPrefab;
    public BuildingBlueprint instantiatedBlueprint;

    public RectTransform menuParent;
    public Button menuItemPrefab;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            TryBuild();
        }	
        if(Input.GetMouseButtonDown(1))
        {
            if (instantiatedBlueprint)
            {
                instantiatedBlueprint.DestroySelf();
            }
        }
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

    public void TryBuild()
    {

    }
}
