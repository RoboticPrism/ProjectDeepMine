using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : BuildingBase {
    
    public int attackSpeed;
    int attackSpeedCurrent = 0;
    int attackSpeedMax = 60;

    public float rotationSpeed = 1f;

    public GameObject turretHead;

    public EnemyBase targetEnemy;

    public FriendlyProjectileBase projectilePrefab;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (built && !broken)
        {
            bool _ = RotateTowardsEnemy() && Shoot();
        }
    }

    private bool RotateTowardsEnemy()
    {
        if (targetEnemy != null)
        {
            Vector2 targetLocation = targetEnemy.transform.position;

            Vector2 vectorToTarget = targetLocation - (Vector2)transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            turretHead.transform.rotation = Quaternion.Slerp(turretHead.transform.rotation, targetRotation, rotationSpeed);
            if (Quaternion.Angle(turretHead.transform.rotation, targetRotation) < 5)
            {
                turretHead.transform.rotation = targetRotation;
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    private bool Shoot()
    {
        if (attackSpeedCurrent < attackSpeedMax)
        {
            attackSpeedCurrent += attackSpeed;
            return false;
        }
        else
        {
            attackSpeedCurrent = 0;
            FriendlyProjectileBase projectile = Instantiate(projectilePrefab, this.transform.position, turretHead.transform.rotation);
            return true;
        }
    }
}
