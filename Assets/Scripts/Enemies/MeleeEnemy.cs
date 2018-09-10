using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyBase {

    public AttackTask attackTaskPrefab;
    AttackTask attackTask = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        DoNextAction();
    }

    void DoNextAction()
    {
        if(attackTask != null)
        {
            if(AttackTask())
            {
                attackTask = null;
                
            }
        }
        else
        {
            BuildingBase targetBuidling = EnemyManager.instance.GetNearestBuilding(this.transform.position);
            if (targetBuidling)
            {
                attackTask = Instantiate(attackTaskPrefab, transform);
                attackTask.Setup(targetBuidling);
                MakePath(attackTask.TargetLocation());
            }
        }
    }

    bool AttackTask()
    {
        return
             MoveAlongPathBehavior() &&
             RotateTowardsTargetBehavior(attackTask) &&
             attackTask.DoTask(damage, attackSpeed);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
