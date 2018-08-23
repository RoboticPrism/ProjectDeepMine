using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyProjectileBase : MonoBehaviour {

    public int damage = 1;
    public float speed = 1f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += transform.up * speed * Time.deltaTime;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.TakeDamage(damage);
            DestroySelf();
        }

        if(collision.gameObject.GetComponent<WallBase>())
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
