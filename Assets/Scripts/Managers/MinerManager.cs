using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerManager : MonoBehaviour {

    List<Miner> minerList = new List<Miner>();
    public WallMenu wallMenuPrefab;

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

    public void CreateWallMenu(Wall wall)
    {
        WallMenu wallMenu = Instantiate(wallMenuPrefab, wall.transform.position, Quaternion.Euler(Vector3.zero));
        wallMenu.CreateMenu(wall);
    }

    public void AddWallToQueue(Wall wall)
    {
        // TODO: multiple worker scheduling
        Debug.Log("added wall");
        minerList[0].AddTask(new MineTask(wall));
    }

    public void AddWallToQueueNow(Wall wall)
    {
        // TODO: multiple worker scheduling
        Debug.Log("added wall urgently");
        minerList[0].AddTaskNow(new MineTask(wall));
    }
}
