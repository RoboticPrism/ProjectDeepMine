using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTask : MinerTask {

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {        
        return base.TaskAvailable();
    }

    public bool DoTask(float mineSpeed)
    {
        if (target)
        {
            MineableWall targetWall = target.GetComponent<MineableWall>();
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
        else
        {
            return true;
        }
    }

    
}
