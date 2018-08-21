using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BuildingBlueprint : MonoBehaviour {
    
    public BuildingBase buildingType;

    public Sprite MiningDrillBlueprint;

    SpriteRenderer spriteRenderer;
    public Color buildColor;
    public Color noBuildColor;

	// Use this for initialization
	void Start () {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPoint = new Vector3(Mathf.Floor(worldPoint.x), Mathf.Floor(worldPoint.y), -1);
        this.transform.position = worldPoint + new Vector3(0.5f, 0.5f, 0);

        Vector3Int floorPosition = TilemapManager.instance.floorTilemap.WorldToCell(worldPoint);
        GameObject floor = TilemapManager.instance.floorTilemap.GetInstantiatedObject(floorPosition);

        Vector3Int wallPosition = TilemapManager.instance.wallTilemap.WorldToCell(worldPoint);
        GameObject wall = TilemapManager.instance.wallTilemap.GetInstantiatedObject(wallPosition);
        if (buildingType.CanBuildHere(floor, wall))
        {
            spriteRenderer.color = buildColor;
            if (Input.GetMouseButtonDown(0))
            {
                TilemapManager.instance.wallTilemap.SetTile(wallPosition, buildingType.tileType);
                BuildingBase newBuilding = TilemapManager.instance.wallTilemap.GetInstantiatedObject(wallPosition).GetComponent<BuildingBase>();
                MinerManager.instance.AddTaskToQueue(new BuildTask("", newBuilding, Task.priotities.QUEUE));
                DestroySelf();
            }
        }
        else
        {
            spriteRenderer.color = noBuildColor;
        }
        if(Input.GetMouseButtonDown(1))
        {
            DestroySelf();
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
