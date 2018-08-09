using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTask : Task {
    public BuildingBase targetBuilding;

    public RepairTask(BuildingBase targetBuilding)
    {
        this.targetBuilding = targetBuilding;
        targetBuilding.repairTask = this;
    }
}
