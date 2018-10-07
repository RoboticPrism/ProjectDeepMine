using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The core of the base, game ends when destroyed
public class Core : BuildingBase {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        OnCreate();
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    // Called when a building has been damaged enough to no longer function
    public override void OnBreak()
    {
        VictoryManager.instance.TriggerDefeat();
    }
}
