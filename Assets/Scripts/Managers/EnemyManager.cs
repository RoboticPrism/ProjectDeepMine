using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager instance;

    public float waveTimeCurrent = 0;
    public float waveTimeMax = 100;

    public bool enemyWave = false;

    public int enemiesPerWave = 5;
    public int enemiesLeftToSpawn = 0;

    public List<EnemyBase> spawnableEnemies = new List<EnemyBase>();
    public List<EnemyBase> spawnedEnemies = new List<EnemyBase>();

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

    private void FixedUpdate()
    {
        waveTimeCurrent += ResourceManager.instance.seismicActivity/60;

        if (enemyWave)
        {
            if(enemiesLeftToSpawn <= 0 && spawnedEnemies.Count <= 0)
            {
                EndWave();
            }
            if(enemiesLeftToSpawn > 0)
            {
                EnemyBase enemyToSpawn = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
                Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
                EnemyBase enemy = Instantiate( enemyToSpawn, spawnPosition, Quaternion.Euler(Vector3.zero));
                spawnedEnemies.Add(enemy);
                enemiesLeftToSpawn -= 1;
            }
        }
        else
        {
            if(waveTimeCurrent >= waveTimeMax)
            {
                StartWave();
            }
        }
    }

    private void StartWave()
    {
        enemyWave = true;
        enemiesLeftToSpawn = enemiesPerWave;
    }

    private void EndWave()
    {
        enemyWave = false;
        waveTimeCurrent = 0;
    }

    public void RemoveEnemyFromList(EnemyBase enemyBase)
    {
        spawnedEnemies.Remove(enemyBase);
    }

    ////////////////////
    // BUILD TRACKING //
    ////////////////////

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
