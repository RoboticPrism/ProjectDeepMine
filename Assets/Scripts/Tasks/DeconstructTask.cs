using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeconstructTask : Task {
    public BuildingBase targetBuilding;

    public DeconstructTask (BuildingBase targetBuilding)
    {
        this.targetBuilding = targetBuilding;
        targetBuilding.deconstructTask = this;
    }
}
