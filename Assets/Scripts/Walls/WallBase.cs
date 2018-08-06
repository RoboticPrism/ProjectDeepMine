using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Base class that all wall pieces extend from
public abstract class WallBase : MonoBehaviour {

    public string displayName;
    public Tilemap tileMap;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;

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

    public bool blocksLighting = true;

    public bool isAlive = true;

    // Use this for initialization
    protected virtual void Start () {
        wallTypes = new Dictionary<List<bool>, Sprite>
        {
            { new List<bool> { true, true, true, true }, unknownSprite },
            { new List<bool> { true, false, true, false }, wallSprite },
            { new List<bool> { true, true, true, false }, wallSprite },
            { new List<bool> { false, true, true, false }, innerCornerSprite },
            { new List<bool> { false, false, false, false }, pillarSprite },
        };
        tileMap = FindObjectOfType<TilemapManager>().wallTilemap;
        boxCollider = this.GetComponent<BoxCollider2D>();
        CheckNeighbors();
        UpdateShading();
    }
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    // FixedUpdate is called once per tick
    protected virtual void FixedUpdate()
    {

    }

    // Check all neighbors and establish and remember what is around this block
    public virtual void CheckNeighbors()
    {
        int i = 0;
        foreach (Vector2 direction in neighborDirections)
        {
            Vector3Int cell = tileMap.WorldToCell(this.transform.position + (Vector3)direction);
            GameObject cellObj = tileMap.GetInstantiatedObject(cell);
            if (cellObj && 
                cellObj.GetComponent<WallBase>() && 
                cellObj.GetComponent<WallBase>().isAlive &&
                cellObj.GetComponent<WallBase>().blocksLighting)
            {
                hasNeighbors[i] = true;
            }
            else
            {
                hasNeighbors[i] = false;
            }

            i++;
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
        }
        else
        {
            spriteRenderer.sprite = wallSprite;
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
            Vector3Int cell = tileMap.WorldToCell(this.transform.position + (Vector3)direction);
            GameObject cellObj = tileMap.GetInstantiatedObject(cell);
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
        isAlive = false;
        UpdateNeighbors();
        Destroy(this.gameObject);
    }

    // On mouse down, pop menu
    private void OnMouseDown()
    {
        UIHoverListener uhl = FindObjectOfType<UIHoverListener>();
        if (!IsSurrounded(false) && (uhl == null || !uhl.isUIOverride))
        {
            FindObjectOfType<MinerManager>().CreateWallMenu(this);
        }
    }
}
