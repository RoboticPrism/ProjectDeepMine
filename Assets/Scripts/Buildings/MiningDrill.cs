using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiningDrill : BuildingBase {
    
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
                    ResourceManager.instance.AddOre(floorBase.MineResources(), floorBase.transform.position);
                }
            }
            else
            {
                currentMining += mineSpeed;
            }
        }
    }

    public override bool CanBuildHere(GameObject floorObj, GameObject wallObj)
    {
        if(wallObj == null &&
            floorObj && 
            floorObj.GetComponent<FloorBase>() && 
            floorObj.GetComponent<FloorBase>().resourceType != FloorBase.resourceTypes.NONE)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
