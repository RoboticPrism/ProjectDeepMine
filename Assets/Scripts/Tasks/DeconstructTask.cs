using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeconstructTask : Task {

    public BuildingBase targetBuilding;

    public override void Setup(TaskableBase target)
    {
        base.Setup(target);
        this.targetBuilding = target.GetComponent<BuildingBase>();
    }

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {
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

    public bool DoTask(float buildSpeed)
    {
        if(targetBuilding)
        {
            targetBuilding.AddConstruction(-buildSpeed);
            return false;
        }
        else
        {
            return true;
        }
    }
}
