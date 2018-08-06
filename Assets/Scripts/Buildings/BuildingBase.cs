using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : WallBase {

    public int life = 100;
    public int lifeMax = 100;

    public int buildAmount = 0;
    public int buildMax = 100;

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

    public void OnCreate()
    {
        resourceManager.AddOre(-oreCost);
        resourceManager.AddPower(-powerCost);
    }

    public void OnBuilt()
    {

    }

    public void OnSell()
    {
        resourceManager.AddOre(oreCost);
        resourceManager.AddPower(powerCost);
    }

    public void OnBreak()
    {
        
    }

    public void OnFix()
    {
        
    }
}
