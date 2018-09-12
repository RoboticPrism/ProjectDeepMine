using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTask : MinerTask {

    public MineableWall targetWall;

    public override void Setup(TaskableBase target) {
        base.Setup(target);
        this.targetWall = target.GetComponent<MineableWall>();
    }

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {        
        return base.TaskAvailable();
    }

    public bool DoTask(float mineSpeed)
    {
        if (targetWall)
        {
            targetWall.MineWall(mineSpeed);
            return false;
        }
        else
        {
            return true;
        }
    }

    
}
