using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask : MinerTask {

    float attackCooldown = 60;
    float attackCooldownMax = 60;

    public BuildingBase targetBuilding;

    public override void Setup(TaskableBase target)
    {
        base.Setup(target);
        this.targetBuilding = target.GetComponent<BuildingBase>();
    }

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {
        // can't schedule an attack task if the building is already broken
        if (!targetBuilding.broken)
        {
            return base.TaskAvailable();
        }
        else
        {
            return false;
        }
    }

    public bool DoTask(int damage, float attackSpeed)
    {
        if (targetBuilding.life > 0)
        {
            if (attackCooldown >= attackCooldownMax)
            {
                targetBuilding.AddLife(-damage);
                attackCooldown = 0;
            }
            else
            {
                attackCooldown += attackSpeed;
            }
            return false;
        }
        else
        {
            targetBuilding.life = 0;
            return true;
        }
    }
}
