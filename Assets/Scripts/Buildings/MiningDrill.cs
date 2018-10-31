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

    [Header("Prefab Connections")]
    [SerializeField]
    OutOfResources outOfResourcesPrefab;
    OutOfResources outOfResourcesInstance;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        floorTilemap = FindObjectOfType<TilemapManager>().floorTilemap;
        Vector3Int tileLoc = floorTilemap.WorldToCell(this.transform.position);
        floorBase = floorTilemap.GetInstantiatedObject(tileLoc).GetComponent<FloorBase>();
        GetComponent<ResourcesHoverInfo>().resourceHolder = floorBase.gameObject;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    // FixedUpdate is called once per tick
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!broken && built && floorBase)
        {
            if (floorBase.HasResources())
            {
                if (currentMining >= miningNeeded)
                {
                    currentMining = 0;
                    if (floorBase.resourceType == FloorBase.resourceTypes.ORE)
                    {
                        ResourceManager.instance.AddOre(floorBase.MineResources(), floorBase.transform.position);
                    }
                    else if (floorBase.resourceType == FloorBase.resourceTypes.ENERGY_CRYSTAL)
                    {
                        ResourceManager.instance.AddEnergyCrystals(floorBase.MineResources(), floorBase.transform.position);
                    }
                }
                else
                {
                    currentMining += mineSpeed;
                }
            }
            else
            {
                if(outOfResourcesInstance == null)
                {
                    outOfResourcesInstance = Instantiate(outOfResourcesPrefab, transform);
                    outOfResourcesInstance.Setup(floorBase.resourceIcon);
                }
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

    public override void OnDeconstruct()
    {
        base.OnDeconstruct();
        outOfResourcesInstance.gameObject.SetActive(false);
    }
}
