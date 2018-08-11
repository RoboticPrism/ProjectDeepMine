﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTask : Task {

    public BuildingBase targetBuilding;

    public BuildTask(string taskName, BuildingBase target, priotities priority) : base(taskName, target, priority) {
        this.targetBuilding = target;
    }

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {
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

}
