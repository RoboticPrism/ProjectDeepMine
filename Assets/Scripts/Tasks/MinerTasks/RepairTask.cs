using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTask : MinerTask {

    public BuildingBase targetBuilding;

    public override void Setup(TaskableBase target)
    {
        base.Setup(target);
        this.targetBuilding = target.GetComponent<BuildingBase>();
    }

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {
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

    public bool DoTask(int repairSpeed)
    {
        if(targetBuilding.life < targetBuilding.lifeMax)
        {
            targetBuilding.AddLife(repairSpeed);
            return false;
        }
        else
        {
            return true;
        }
    }
}
