﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiningDrill : BuildingBase {

    ResourceManager resourceManager;
    Tilemap floorTilemap;
    FloorBase floorBase;
    public float mineSpeed = 1f;
    public float miningNeeded = 100f;
    public float currentMining = 0f;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        floorTilemap = FindObjectOfType<TilemapManager>().floorTilemap;
        Vector3Int tileLoc = floorTilemap.WorldToCell(this.transform.position);
        floorBase = floorTilemap.GetInstantiatedObject(tileLoc).GetComponent<FloorBase>();
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
        if (!broken && built && floorBase && floorBase.resourceCount > 0)
        {
            if(currentMining >= miningNeeded)
            {
                currentMining = 0;
                if (floorBase.resourceType == FloorBase.resourceTypes.ORE) {
                    resourceManager.AddOre(floorBase.MineResources());
                }
            }
            else
            {
                currentMining += mineSpeed;
            }
        }
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
