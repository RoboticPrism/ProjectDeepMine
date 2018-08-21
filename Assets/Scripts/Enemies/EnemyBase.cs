using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MoveableBase {

    public int life;
    public int maxLife;

    public int damage = 1;
    public float attackSpeed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("turret");
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


}
