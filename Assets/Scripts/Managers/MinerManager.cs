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

    public void AddTaskToQueue(Task task)
    {
        if (task.priority == Task.priotities.QUEUE)
        {
            NextAvailableMiner().AddTask(task);
        }
        else if (task.priority == Task.priotities.QUEUE_NOW)
        {
            NextAvailableMiner().AddTaskNow(task);
        }
        else
        {
            NextAvailableMiner().PrioritizeTask(task);
        }
    }
}
