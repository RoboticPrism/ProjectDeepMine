using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerManager : MonoBehaviour {

    List<Miner> minerList = new List<Miner>();
    public TileMenu tileMenuPrefab;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void AddMiner(Miner miner)
    {
        Debug.Log("added miner");
        minerList.Add(miner);
    }

    public void RemoveMiner(Miner miner)
    {
        minerList.Remove(miner);
    }

    Miner NextAvailableMiner()
    {
        // TODO: add multi worker scheduling
        return minerList[0];
    }

    public void CreateTileMenu(ClickableTileBase tile)
    {
        TileMenu tileMenu = Instantiate(tileMenuPrefab, tile.transform.position, Quaternion.Euler(Vector3.zero));
        tileMenu.CreateMenu(tile);
    }

    // Adds a new wall to be mined to the end of the queue
    public void AddWallToQueue(MineableWall wall)
    {
        NextAvailableMiner().AddTask(new MineTask(wall));
    }

    // Adds a new wall to be mined to the front of the queue
    public void AddWallToQueueNow(MineableWall wall)
    {
        NextAvailableMiner().AddTaskNow(new MineTask(wall));
    }

    // Adds a new building to be constructed to the end of the queue
    public void AddBuildingToQueue(BuildingBase buildingBase)
    {
        NextAvailableMiner().AddTask(new BuildTask(buildingBase));
    }

    // Adds a new building to be constructed to the front of the queue
    public void AddBuildingToQueueNow(BuildingBase buildingBase)
    {
        NextAvailableMiner().AddTaskNow(new BuildTask(buildingBase));
    }

    // Adds a new building to be repaired to the end of the queue
    public void AddBuildingRepairToQueue(BuildingBase buildingBase)
    {
        NextAvailableMiner().AddTask(new RepairTask(buildingBase));
    }

    // Adds a new building to be repaired to the front of the queue
    public void AddBuildingRepairToQueueNow(BuildingBase buildingBase)
    {
        NextAvailableMiner().AddTaskNow(new RepairTask(buildingBase));
    }

    // Adds a new building to be deconstructed to the end of the queue
    public void AddBuildingDeconstructToQueue(BuildingBase buildingBase)
    {
        NextAvailableMiner().AddTask(new DeconstructTask(buildingBase));
    }

    // Adds a new building to be deconstructed to the front of the queue
    public void AddBuildingDeconstructToQueueNow(BuildingBase buildingBase)
    {
        NextAvailableMiner().AddTaskNow(new DeconstructTask(buildingBase));
    }
}
