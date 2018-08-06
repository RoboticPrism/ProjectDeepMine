using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// A room hidden to the player that is shown when accessed
public class HiddenRoom : WallBase {

    // Use this for initialization
    protected override void Start () {
        base.Start();
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

    public override void UpdateShading()
    {
        if (!IsSurrounded(true))
        {
            this.DestroySelf();   
        }
    }
}
