using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTask : Task {

    public BuildingBase targetBuilding;

	public BuildTask(BuildingBase targetBuilding)
    {
        this.targetBuilding = targetBuilding;
        targetBuilding.buildTask = this;
    }

    public override Vector3 TargetLocation()
    {
        return targetBuilding.transform.position;
    }
}
