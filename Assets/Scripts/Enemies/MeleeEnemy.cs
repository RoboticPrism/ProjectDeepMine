using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyBase {

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
        if (needsNewTask)
        {
            StartCoroutine(MeleeAttackTask(EnemyManager.instance.GetNearestBuilding(this.transform.position)));
        }
    }

    public IEnumerator MeleeAttackTask(BuildingBase building)
    {
        needsNewTask = false;
        Vector3Int gridPos = GridUtilities.WorldToCell(TilemapManager.instance.wallTilemap, building.transform.position);
        yield return StartCoroutine(MoveTo(gridPos));
        yield return StartCoroutine(RotateTowards(building.transform.position));
        yield return StartCoroutine(MeleeAttack(building));
        needsNewTask = true;
    }

    public IEnumerator MeleeAttack(BuildingBase building)
    {
        while (building.life > 0)
        {
            if (attackCooldown >= attackCooldownMax)
            {
                building.AddLife(-damage);
                attackCooldown = 0;
            }
            else
            {
                attackCooldown += attackSpeed;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
