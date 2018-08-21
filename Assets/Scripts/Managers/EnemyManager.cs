using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager instance;
    
    public List<EnemyBase> spawnableEnemies = new List<EnemyBase>();

    List<BuildingBase> buildingList = new List<BuildingBase>();
    public List<EnemySpawner> spawnPoints = new List<EnemySpawner>();

    protected UnityAction<ClickableTileBase> buildingCreatedListener;
    protected UnityAction<ClickableTileBase> buildingRemovedListener;
    protected UnityAction<ClickableTileBase> wallRemovedListener;

    // Use this for initialization
    void Start () {
        instance = this;
        buildingCreatedListener = new UnityAction<ClickableTileBase>(AddBuildingToList);
        EventManager.StartListening("BuildingCreated", buildingCreatedListener);
        buildingRemovedListener = new UnityAction<ClickableTileBase>(RemoveBuildingFromList);
        EventManager.StartListening("BuildingSold", buildingRemovedListener);
        wallRemovedListener = new UnityAction<ClickableTileBase>(CheckForNewSpawner);
        EventManager.StartListening("WallDestroyed", wallRemovedListener);
        foreach(EnemySpawner spawner in FindObjectsOfType<EnemySpawner>())
        {
            if(spawner.CheckIfCovered())
            {
                spawnPoints.Add(spawner);
            }
        }
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

    public void CheckForNewSpawner(ClickableTileBase tileBase)
    {
        Vector3Int location = TilemapManager.instance.wallTilemap.WorldToCell(tileBase.transform.position);
        GameObject locObj = TilemapManager.instance.floorTilemap.GetInstantiatedObject(location);
        if (locObj)
        {
            EnemySpawner enemySpawner = locObj.GetComponent<EnemySpawner>();
            if (enemySpawner)
            {
                spawnPoints.Add(enemySpawner);
            }
        }
    }
}
