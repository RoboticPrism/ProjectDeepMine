﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : BuildingBase {

    public int powerProvided = 3;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    // Called when a building is done being constructed
    public override void OnBuilt()
    {
        base.OnBuilt();
        ResourceManager.instance.AddPowerMax(powerProvided, this.transform.position);
    }

    // Called when a building begins being deconstructed
    public override void OnDeconstruct()
    {
        base.OnDeconstruct();
        ResourceManager.instance.AddPowerMax(-powerProvided, this.transform.position);
    }

    // Called when a building has been damaged enough to no longer function
    public override void OnBreak()
    {
        base.OnBreak();
        ResourceManager.instance.AddPowerMax(-powerProvided, this.transform.position);
    }

    // Called when a building has been fixed enough to function
    public override void OnFix()
    {
        base.OnFix();
        ResourceManager.instance.AddPowerMax(powerProvided, this.transform.position);
    }
}
