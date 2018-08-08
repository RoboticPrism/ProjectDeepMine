using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BuildingBlueprint : MonoBehaviour {
    
    TilemapManager tilemapManager;
    public BuildingBase buildingType;

    public Sprite MiningDrillBlueprint;

    SpriteRenderer spriteRenderer;
    public Color buildColor;
    public Color noBuildColor;

	// Use this for initialization
	void Start () {
        tilemapManager = FindObjectOfType<TilemapManager>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPoint = new Vector3(Mathf.Floor(worldPoint.x), Mathf.Floor(worldPoint.y), -1);
        this.transform.position = worldPoint + new Vector3(0.5f, 0.5f, 0);

        Vector3Int floorPosition = tilemapManager.floorTilemap.WorldToCell(worldPoint);
        GameObject floorObj = tilemapManager.floorTilemap.GetInstantiatedObject(floorPosition);
        FloorBase floor = null;
        if (floorObj)
        {
            floor = floorObj.GetComponent<FloorBase>();
        }

        Vector3Int wallPosition = tilemapManager.floorTilemap.WorldToCell(worldPoint);
        GameObject wallObj = tilemapManager.floorTilemap.GetInstantiatedObject(wallPosition);
        WallBase wall = null;
        if (wallObj) {
            wall = wallObj.GetComponent<WallBase>();
        }
        if (buildingType.CanBuildHere(floor, wall))
        {
            spriteRenderer.color = buildColor;
            if (Input.GetMouseButtonDown(0))
            {
                tilemapManager.wallTilemap.SetTile(wallPosition, buildingType.tileType);
                DestroySelf();
            }
        }
        else
        {
            spriteRenderer.color = noBuildColor;
        }
    }

    public void SetupBlueprint(BuildingBase building)
    {
        this.buildingType = (BuildingBase)building;
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
