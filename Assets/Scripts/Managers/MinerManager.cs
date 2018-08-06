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

    public void CreateTileMenu(ClickableTileBase tile)
    {
        TileMenu tileMenu = Instantiate(tileMenuPrefab, tile.transform.position, Quaternion.Euler(Vector3.zero));
        tileMenu.CreateMenu(tile);
    }

    public void AddWallToQueue(MineableWall wall)
    {
        // TODO: multiple worker scheduling
        Debug.Log("added wall");
        minerList[0].AddTask(new MineTask(wall));
    }

    public void AddWallToQueueNow(MineableWall wall)
    {
        // TODO: multiple worker scheduling
        Debug.Log("added wall urgently");
        minerList[0].AddTaskNow(new MineTask(wall));
    }
}
