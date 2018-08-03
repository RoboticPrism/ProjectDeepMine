using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerManager : MonoBehaviour {

    List<Miner> minerList = new List<Miner>();

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

    public void AddWallToQueue(Wall wall)
    {
        Debug.Log("added wall");
        minerList[0].AddTask(new MineTask(wall));
    }
}
