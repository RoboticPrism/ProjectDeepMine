using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTask : Task {

    public MineableWall targetWall;

    public MineTask(string taskName, MineableWall target, priotities priority) : base(taskName, target, priority) {
        this.targetWall = target;
    }

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {        
        return base.TaskAvailable();
    }
}
