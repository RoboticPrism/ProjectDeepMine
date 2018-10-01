using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTask : MinerTask {

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {
        BuildingBase targetBuilding = target.GetComponent<BuildingBase>();
        // can't schedule a repair task if the building is at max hp
        if (targetBuilding.life < targetBuilding.lifeMax)
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
        return building.life < building.lifeMax;
    }

    // Starts the associtated coroutine on the miner
    public override void StartTaskCoroutine(Miner miner)
    {
        miner.StartCoroutine(miner.RepairTask(this.target.GetComponent<BuildingBase>()));
    }
}
