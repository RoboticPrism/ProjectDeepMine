﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTask : Task {

    public BuildingBase targetBuilding;

    public RepairTask(string taskName, BuildingBase target, priotities priority) : base(taskName, target, priority)
    {
        this.targetBuilding = target;
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
}
