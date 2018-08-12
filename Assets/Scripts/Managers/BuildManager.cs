using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {
    
    public BuildingBlueprint blueprintPrefab;
    public BuildingBlueprint instantiatedBlueprint;

    public RectTransform menuParent;
    public Button menuItemPrefab;

	// Use this for initialization
	protected void Start () {
        
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
}
