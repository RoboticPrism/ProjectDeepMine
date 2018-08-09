using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiningDrill : BuildingBase {

    ResourceManager resourceManager;
    Tilemap floorTilemap;
    FloorBase floorBase;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        floorTilemap = FindObjectOfType<TilemapManager>().floorTilemap;
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

    public override bool CanBuildHere(FloorBase floorTile, WallBase wallTile)
    {
        if(wallTile == null && floorTile && floorTile.resourceType != FloorBase.resourceTypes.NONE)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
