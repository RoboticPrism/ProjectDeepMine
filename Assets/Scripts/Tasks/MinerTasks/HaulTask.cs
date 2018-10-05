using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulTask : MinerTask {

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {
        return base.TaskAvailable() && target.transform.parent == null;
    }

    public static bool TaskAvailable(HaulableBase haulable)
    {
        return haulable.transform.parent == null;
    }

    // Starts the associtated coroutine on the miner
    public override void StartTaskCoroutine(Miner miner)
    {
        miner.StartCoroutine(miner.HaulTask(this.target.GetComponent<HaulableBase>()));
    }
}
