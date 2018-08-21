using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager instance;

    List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public List<EnemyBase> spawnableEnemies = new List<EnemyBase>();

    List<BuildingBase> buildingList = new List<BuildingBase>();

    protected UnityAction<ClickableTileBase> buildingCreatedListener;
    protected UnityAction<ClickableTileBase> buildingRemovedListener;

    // Use this for initialization
    void Start () {
        instance = this;
        buildingCreatedListener = new UnityAction<ClickableTileBase>(AddBuildingToList);
        EventManager.StartListening("BuildingCreated", buildingCreatedListener);
        buildingRemovedListener = new UnityAction<ClickableTileBase>(RemoveBuildingFromList);
        EventManager.StartListening("BuildingSold", buildingRemovedListener);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddBuildingToList(ClickableTileBase newBuilding)
    {
        buildingList.Add(newBuilding.GetComponent<BuildingBase>());
    }

    void RemoveBuildingFromList(ClickableTileBase oldBuilding)
    {
        buildingList.Remove(oldBuilding.GetComponent<BuildingBase>());
    }

    // Hands out the nearest attackable building relative to a given location
    public BuildingBase GetNearestBuilding(Vector3 location)
    {
        BuildingBase nearest = null;
        foreach (BuildingBase building in buildingList)
        {
            if (nearest == null)
            {
                nearest = building;
            }
            // TODO: see if we can make this smarter with AStar without being too costly
            else if (Vector3.Distance(building.transform.position, location) < Vector3.Distance(nearest.transform.position, location) &&
                !building.broken)
            {
                nearest = building;
            }
        }
        return nearest;
    }
}
