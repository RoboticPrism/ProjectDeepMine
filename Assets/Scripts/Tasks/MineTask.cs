using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTask : Task {

    public MineableWall targetWall;

    public MineTask(MineableWall targetWall)
    {
        this.targetWall = targetWall;
        targetWall.task = this;
    }
}
