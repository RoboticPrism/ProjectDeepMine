using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTask : Task {

    public Wall targetWall;

    public MineTask(Wall targetWall)
    {
        this.targetWall = targetWall;
    }
}
