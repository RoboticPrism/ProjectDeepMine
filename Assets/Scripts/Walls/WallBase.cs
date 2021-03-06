﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Base class that all wall pieces extend from
public abstract class WallBase : TaskableBase {

    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;

    public MineTask mineTaskPrefab;

    private HoverInfo hoverInfo;

    List<Vector2> neighborDirections = new List<Vector2> {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left,
        Vector2.up + Vector2.right,
        Vector2.right + Vector2.down,
        Vector2.down + Vector2.left,
        Vector2.left + Vector2.up
    };
    List<bool> hasNeighbors = new List<bool> { false, false, false, false, false, false, false, false };

    public Sprite unknownSprite = null;
    public Sprite innerCornerSprite;
    public Sprite wallSprite;
    public Sprite outerCornerSprite;
    public Sprite pillarSprite;

    Dictionary<List<bool>, Sprite> wallTypes;

    public bool isAlive = true;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        wallTypes = new Dictionary<List<bool>, Sprite>
        {
            { new List<bool> { true, true, true, true }, unknownSprite },
            { new List<bool> { true, false, true, false }, wallSprite },
            { new List<bool> { true, true, true, false }, wallSprite },
            { new List<bool> { false, true, true, false }, innerCornerSprite },
            { new List<bool> { false, false, false, false }, pillarSprite },
        };
        TilemapManager.instance.wallTilemap = FindObjectOfType<TilemapManager>().wallTilemap;
        boxCollider = GetComponent<BoxCollider2D>();
        hoverInfo = GetComponent<HoverInfo>();
        clickable = false;
        CheckNeighbors();
        UpdateShading();
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

    // Check all neighbors and establish and remember what is around this block
    public virtual void CheckNeighbors()
    {
        int i = 0;
        foreach (Vector2 direction in neighborDirections)
        {
            Vector3Int cell = TilemapManager.instance.wallTilemap.WorldToCell(this.transform.position + (Vector3)direction);
            GameObject cellObj = TilemapManager.instance.wallTilemap.GetInstantiatedObject(cell);
            if (cellObj && 
                cellObj.GetComponent<WallBase>() && 
                cellObj.GetComponent<WallBase>().isAlive)
            {
                hasNeighbors[i] = true;
            }
            else
            {
                hasNeighbors[i] = false;
            }

            i++;
        }
        clickable = !IsSurrounded(false);
        if (hoverInfo)
        {
            hoverInfo.visible = !IsSurrounded(true);
        }
    }

    // Updates the shading on the block relative to its neighbors
    public virtual void UpdateShading()
    {
        // Uncomment this when I have a solid plan for the art direction
        /*
        List<bool> basicDirections = hasNeighbors.GetRange(0, 4);
        int spriteRotation = 0;
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(basicDirections[0]+""+ basicDirections[1]+""+ basicDirections[2]+""+ basicDirections[3]);
            foreach (List<bool> b in wallTypes.Keys)
            {
                Debug.Log("ba"+basicDirections[0] + "" + basicDirections[1] + "" + basicDirections[2] + "" + basicDirections[3]);
                Debug.Log("b"+b[0] + "" + b[1] + "" + b[2] + "" + b[3]);
                if (b.SequenceEqual(basicDirections))
                {
                    Debug.Log("match");
                    this.transform.rotation = Quaternion.Euler(0, 0, spriteRotation);
                    this.spriteRenderer.sprite = wallTypes[b];
                    break;
                }
            }
            i++;
            // rotate the check 90 degrees
            spriteRotation = i * 90;
            bool temp = basicDirections[basicDirections.Count - 1];
            basicDirections.RemoveAt(basicDirections.Count - 1);
            basicDirections.Insert(0, temp);
        }
        */
        if (IsSurrounded(true))
        {
            spriteRenderer.sprite = unknownSprite;
            spriteRenderer.sortingLayerName = "Hidden";
        }
        else
        {
            spriteRenderer.sprite = wallSprite;
            spriteRenderer.sortingLayerName = "Wall";
        }
    }

    // Returns true if surrounded from all angles
    public virtual bool IsSurrounded(bool includeDiagonals)
    {
        List<bool> checkList = hasNeighbors;
        if (!includeDiagonals)
        {
            checkList = hasNeighbors.GetRange(0, 4);
        }
        bool ret = true;
        foreach (bool b in checkList)
        {
            ret = ret && b;
        }
        return ret;
    }

    // Tells all neighbors to check their surroundings and update their shading
    public virtual void UpdateNeighbors()
    {
        foreach (Vector2 direction in neighborDirections)
        {
            Vector3Int cell = TilemapManager.instance.wallTilemap.WorldToCell(this.transform.position + (Vector3)direction);
            GameObject cellObj = TilemapManager.instance.wallTilemap.GetInstantiatedObject(cell);
            if (cellObj)
            {
                WallBase neighborWall = cellObj.GetComponent<WallBase>();
                if (neighborWall && neighborWall.isAlive)
                {
                    neighborWall.CheckNeighbors();
                    neighborWall.UpdateShading();
                }
            }
        }
    }

    // Destroys this wall and updates its neighbors
    public virtual void DestroySelf()
    {
        EventManager.TriggerEvent("WallDestroyed", this);
        isAlive = false;
        UpdateNeighbors();
        Vector3Int tileLoc = TilemapManager.instance.wallTilemap.WorldToCell(this.transform.position);
        TilemapManager.instance.wallTilemap.SetTile(tileLoc, null);
    }
}
