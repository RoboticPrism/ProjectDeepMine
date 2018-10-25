using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTask : MinerTask {

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {        
        return base.TaskAvailable();
    }

    // Starts the associtated coroutine on the miner
    public override void StartTaskCoroutine(Miner miner)
    {
        miner.StartCoroutine(miner.MineTask(this.target.GetComponent<MineableWall>()));
    }

    public override bool CanMinerDo(Miner miner)
    {
        return miner.mineableWallTypes.Contains(this.target.GetComponent<MineableWall>().type);
    }
}
