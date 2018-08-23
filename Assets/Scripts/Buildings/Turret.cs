using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : BuildingBase {
    
    public int attackSpeed;
    int attackSpeedCurrent = 0;
    int attackSpeedMax = 60;

    public float rotationSpeed = 1f;

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
        bool _ = RotateTowardsEnemy() && Shoot();
    }

    private bool RotateTowardsEnemy()
    {
        if (targetEnemy != null)
        {
            Vector2 targetLocation = targetEnemy.transform.position;

            Vector2 vectorToTarget = targetLocation - (Vector2)transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 5)
            {
                transform.rotation = targetRotation;
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
            FriendlyProjectileBase projectile = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation);
            return true;
        }
    }
}
