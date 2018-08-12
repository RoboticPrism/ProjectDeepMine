using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The core of the base, game ends when destroyed
public class Core : BuildingBase {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        // Core can't be build or deconstructed
        potentialTasks = new List<Task>
        {
            new RepairTask("Schedule Repair", this, Task.priotities.QUEUE),
            new RepairTask("Repair Now", this, Task.priotities.QUEUE_NOW),
            new RepairTask("Prioritize Repair", this, Task.priotities.REQUEUE_NOW)
        };
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    // Called when a building has been damaged enough to no longer function
    public override void OnBreak()
    {
        // trigger gameover
    }
}
