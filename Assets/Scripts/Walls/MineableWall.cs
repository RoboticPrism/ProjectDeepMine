using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


// A wall in a room, lights itself accordingly
public class MineableWall : WallBase {

    public float life = 100f;
    public float lifeMax = 100f;

    public enum wallType { dirt, stone };
    public wallType type;


    public HealthBar healthBarPrefab;
    HealthBar healthBarInstance;
    public Color healthBarColor;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        potentialTasks = new List<Task>
        {
            new MineTask("Schedule Mine", this, Task.priotities.QUEUE),
            new MineTask("Mine Now", this, Task.priotities.QUEUE_NOW),
            new MineTask("Prioritize Mine", this, Task.priotities.REQUEUE_NOW),
        };
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    // FixedUpdate is called once per tick
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void MineWall(float mineSpeed)
    {
        this.life -= mineSpeed;
        if (life < 0)
        {
            DestroySelf();
        }
        UpdateHealthBar();
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
