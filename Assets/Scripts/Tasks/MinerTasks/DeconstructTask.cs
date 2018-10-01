using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeconstructTask : MinerTask {

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {
        BuildingBase targetBuilding = target.GetComponent<BuildingBase>();
        // can't schedule a deconstruct task if the building is being build or is broken
        if (targetBuilding.built && !targetBuilding.broken)
        {
            return base.TaskAvailable();
        }
        else
        {
            return false;
        }
    }

    public static bool TaskAvailable(BuildingBase building)
    {
        return building.built && !building.broken;
    }

    // Starts the associtated coroutine on the miner
    public override void StartTaskCoroutine(Miner miner)
    {
        miner.StartCoroutine(miner.DeconstructTask(this.target.GetComponent<BuildingBase>()));
    }
}
