using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTask : MinerTask {

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {
        BuildingBase targetBuilding = target.GetComponent<BuildingBase>();
        // can't schedule a build task if the building is already built
        if (!targetBuilding.built)
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
        return !building.built;
    }

    // Starts the associtated coroutine on the miner
    public override void StartTaskCoroutine(Miner miner)
    {
        miner.StartCoroutine(miner.BuildTask(this.target.GetComponent<BuildingBase>()));
    }
}
