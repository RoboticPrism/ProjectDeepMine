using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : TaskableBase {

    public WallTile tileType;

    [Header("Life Stats")]
    public float life = 100;
    public float lifeMax = 100;
    public float brokenLimit = 50;
    
    [Header("Build Stats")]
    public float buildAmount = 0;
    public float buildMax = 100;

    [Header("States")]
    public bool broken = false;
    public bool built = false;
    
    [Header("Costs")]
    public int oreCost = 1;
    public int powerCost = 1;
    public float seismicOutput = 0f;

    [Header("Health Bars")]
    public HealthBar healthBarPrefab;
    HealthBar healthBarInstance;
    HealthBar buildBarInstance;
    public Color healthBarColor;
    public Color buildBarColor;

    [Header("Tasks")]
    public BuildTask buildTaskPrefab;
    public RepairTask repairTaskPrefab;
    public DeconstructTask deconstructTaskPrefab;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        OnCreate();
        if (buildTaskPrefab)
        {
            CreateTask(buildTaskPrefab);
        }
	}

	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    // FixedUpdate is called once per tick
    protected override void FixedUpdate()
    {
        if(currentTask)
        {
            currentTask.DoTask();
        }
        base.FixedUpdate();
    }

    ////////////////////
    // STATE UPDATERS //
    ////////////////////

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

    public void AddLife(float addAmount)
    {
        if (life == lifeMax && addAmount < 0)
        {
            OnDamaged();
        }
        life += addAmount;
        if (life >= lifeMax)
        {
            life = lifeMax;
            OnFix();
        }
        // a half built building can't stop working
        if (built)
        {
            // Update broken state
            if (life >= brokenLimit && broken)
            {
                OnRepaired();
            }
            else if (life < brokenLimit && !broken)
            {
                OnBreak();
            }
        }
        UpdateHealthBar();
    }

    ////////////////////
    // EVENT TRIGGERS //
    ////////////////////

    // Called when a building has first been put down but is not constructed yet
    public virtual void OnCreate()
    {
        ResourceManager.instance.AddOre(-oreCost, this.transform.position);
        ResourceManager.instance.AddPower(powerCost, this.transform.position + new Vector3(0, -0.5f, 0));
        ResourceManager.instance.AddSeismicActivity(seismicOutput);
        EventManager.TriggerEvent("BuildingCreated", this);
    }

    // Called when a building is entirely deconstructed and then sold
    public virtual void OnSell()
    {
        ResourceManager.instance.AddOre(oreCost, this.transform.position);
        ResourceManager.instance.AddPower(-powerCost, this.transform.position + new Vector3(0, -0.5f, 0));
        ResourceManager.instance.AddSeismicActivity(-seismicOutput);
        EventManager.TriggerEvent("BuildingSold", this);
        DestroySelf();
    }

    // Called when a building is done being constructed
    public virtual void OnBuilt()
    {
        built = true;
        EventManager.TriggerEvent("BuildingBuilt", this);
    }

    // Called when a building begins being deconstructed
    public virtual void OnDeconstruct()
    {
        built = false;
        EventManager.TriggerEvent("BuildingDeconstructing", this);
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

    // Called when a building first takes damage from full health
    public virtual void OnDamaged()
    {
        EventManager.TriggerEvent("BuildingDamaged", this);
    }

    // Called when a building is repaired to full health
    public virtual void OnRepaired()
    {
        EventManager.TriggerEvent("BuildingRepaired", this);
    }

    ////////////////////
    // BUILDING TASKS //
    ////////////////////

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
