using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : ClickableTileBase {

    public WallTile tileType;

    public float life = 100;
    public float lifeMax = 100;
    public float brokenLimit = 50;
    public bool broken = false;

    public float buildAmount = 0;
    public float buildMax = 100;
    public bool built = false;

    public int oreCost = 1;
    public int powerCost = 1;

    public HealthBar healthBarPrefab;
    HealthBar healthBarInstance;
    HealthBar buildBarInstance;
    public Color healthBarColor;
    public Color buildBarColor;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        potentialTasks = new List<Task>
        {
            new BuildTask("Schedule Build", this, Task.priotities.QUEUE),
            new BuildTask("Build Now", this, Task.priotities.QUEUE_NOW),
            new BuildTask("Prioritize Build", this, Task.priotities.REQUEUE_NOW),
            new RepairTask("Schedule Repair", this, Task.priotities.QUEUE),
            new RepairTask("Repair Now", this, Task.priotities.QUEUE_NOW),
            new RepairTask("Prioritize Repair", this, Task.priotities.REQUEUE_NOW),
            new DeconstructTask("Schedule Deconstruct", this, Task.priotities.QUEUE),
            new DeconstructTask("Deconstruct Now", this, Task.priotities.QUEUE_NOW),
            new DeconstructTask("Prioritize Deconstruct", this, Task.priotities.REQUEUE_NOW),
        };
        OnCreate();
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

    public void AddConstruction(float addAmount)
    {
        buildAmount += addAmount;
        // Update build state
        if (buildAmount >= buildMax && !built)
        {
            OnBuilt();
        }
        else if (buildAmount < buildMax && built)
        {

            OnDeconstruct();
        }
        else if (buildAmount <= 0)
        {
            Debug.Log("sell");
            OnSell();
        }
        UpdateBuildBar();
    }

    public void AddLife(int addAmount)
    {
        life += addAmount;
        if (life > lifeMax)
        {
            life = lifeMax;
        }
        // a half built building can't stop working
        if (built)
        {
            // Update broken state
            if (life >= brokenLimit && broken)
            {
                OnFix();
            }
            else if (life < brokenLimit && !broken)
            {
                OnBreak();
            }
        }
        UpdateHealthBar();
    }

    // Called when a building has first been put down but is not constructed yet
    public virtual void OnCreate()
    {
        ResourceManager.instance.AddOre(-oreCost);
        ResourceManager.instance.AddPower(powerCost);
        EventManager.TriggerEvent("BuildingCreated", this);
    }

    // Called when a building is entirely deconstructed and then sold
    public virtual void OnSell()
    {
        ResourceManager.instance.AddOre(oreCost);
        ResourceManager.instance.AddPower(-powerCost);
        EventManager.TriggerEvent("BuildingSold", this);
        DestroySelf();
    }

    // Called when a building is done being constructed
    public virtual void OnBuilt()
    {
        built = true;
    }

    // Called when a building begins being deconstructed
    public virtual void OnDeconstruct()
    {
        built = false;
    }

    // Called when a building has been damaged enough to no longer function
    public virtual void OnBreak()
    {
        broken = true;
    }

    // Called when a building has been fixed enough to function
    public virtual void OnFix()
    {
        broken = false;
    }

    public void DestroySelf()
    {
        Vector3Int tileLoc = TilemapManager.instance.wallTilemap.WorldToCell(this.transform.position);
        TilemapManager.instance.wallTilemap.SetTile(tileLoc, null);
    }

    public virtual bool CanBuildHere(GameObject floorObj, GameObject wallObj)
    {
        if (wallObj)
        {
            return false;
        }
        else
        {
            if (floorObj && floorObj.GetComponent<EnemySpawner>())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
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
            if(healthBarInstance)
            {
                Destroy(healthBarInstance.gameObject);
            }
        }
    }

    // Updates the build progress bar, builds on if needed, or destroys health bar if unit is at max
    private void UpdateBuildBar()
    {
        if (buildAmount < buildMax)
        {
            if (buildBarInstance == null)
            {
                buildBarInstance = Instantiate(healthBarPrefab, transform);
                buildBarInstance.transform.position += new Vector3(0, -0.2f, 0);
                buildBarInstance.UpdateColor(buildBarColor);
            }
            buildBarInstance.UpdateBar(buildAmount / buildMax);
        }
        else
        {
            if(buildBarInstance)
            {
                Destroy(buildBarInstance.gameObject);
            }
        }
    }
}
