using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : ClickableTileBase {

    public WallTile tileType;

    public int life = 100;
    public int lifeMax = 100;
    public int brokenLimit = 50;
    public bool broken = false;

    public int buildAmount = 0;
    public int buildMax = 100;
    public bool built = false;

    public int oreCost = 1;
    public int powerCost = 1;

    ResourceManager resourceManager;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        resourceManager = FindObjectOfType<ResourceManager>();
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

    public void AddConstruction(int addAmount)
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
        else if (buildAmount < 0)
        {
            OnSell();
        }
    }

    public void AddLife(int addAmount)
    {
        life += addAmount;
        if (life > lifeMax)
        {
            life = lifeMax;
        }
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

    // Called when a building has first been put down but is not constructed yet
    public void OnCreate()
    {
        resourceManager.AddOre(-oreCost);
        resourceManager.AddPower(-powerCost);
    }

    // Called when a building is done being constructed
    public void OnBuilt()
    {
        built = true;
        
    }

    // Called when a building begins being deconstructed
    public void OnDeconstruct()
    {
        built = false;
    }

    // Called when a building is entirely deconstructed and then sold
    public void OnSell()
    {
        resourceManager.AddOre(oreCost);
        resourceManager.AddPower(powerCost);
        DestroySelf();
    }

    // Called when a building has been damaged enough to no longer function
    public void OnBreak()
    {
        broken = true;
    }

    // Called when a building has been fixed enough to function
    public void OnFix()
    {
        broken = false;
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public virtual bool CanBuildHere(FloorBase floorTile, WallBase wallTile)
    {
        if(wallTile)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
