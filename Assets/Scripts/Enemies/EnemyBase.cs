using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MoveableBase {
    public int life;
    public int lifeMax;

    public int damage = 1;
    public float attackSpeed = 1f;

    public HealthBar healthBarPrefab;
    HealthBar healthBarInstance;

    public Color healthBarColor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Turret turret = collision.GetComponent<Turret>();
        if (turret && turret.targetEnemy == null)
        {
            turret.targetEnemy = this;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Turret turret = collision.GetComponent<Turret>();
        if (turret && turret.targetEnemy == null)
        {
            turret.targetEnemy = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Turret turret = collision.GetComponent<Turret>();
        if (collision.GetComponent<EnemyBase>() == turret.targetEnemy)
        {
            turret.targetEnemy = null;
        }
    }

    public void TakeDamage(int damageTaken)
    {
        life -= damageTaken;
        if (life <= 0)
        {
            DestroySelf();
        }
        UpdateHealthBar();
    }

    public void DestroySelf()
    {
        EnemyManager.instance.RemoveEnemyFromList(this);
        Destroy(this.gameObject);
    }

    // Updates the health bar, builds on if needed, or destroys health bar if unit is at max
    private void UpdateHealthBar()
    {
        if (life < lifeMax)
        {
            if (healthBarInstance == null)
            {
                healthBarInstance = Instantiate(healthBarPrefab, transform);
                healthBarInstance.transform.position += new Vector3(0, -0.4f, 0);
                healthBarInstance.UpdateColor(healthBarColor);
            }
            healthBarInstance.UpdateBar((float)life / lifeMax);
        }
        else
        {
            if (healthBarInstance)
            {
                Destroy(healthBarInstance.gameObject);
            }
        }
    }
}
