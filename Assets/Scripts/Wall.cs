using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// A wall in a room, lights itself accordingly
public class Wall : MonoBehaviour {

    public Tilemap tileMap;
    public float destroyTime = 100f;
    public SpriteRenderer wallShadow;
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

    public bool isAlive = true;

	// Use this for initialization
	void Start () {
        tileMap = FindObjectOfType<Tilemap>();
        boxCollider = this.GetComponent<BoxCollider2D>();
        CheckNeighbors();
        UpdateShading();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MineWall(float mineSpeed)
    {
        this.destroyTime -= mineSpeed;
    }

    public void CheckNeighbors()
    {
        int i = 0;
        foreach (Vector2 direction in neighborDirections)
        {
            Vector3Int cell = tileMap.WorldToCell(this.transform.position + (Vector3)direction);
            GameObject cellObj = tileMap.GetInstantiatedObject(cell);
            if (cellObj)
            {
                Wall neighborWall = cellObj.GetComponent<Wall>();
                HiddenRoom neighborHidden = cellObj.GetComponent<HiddenRoom>();
                if (neighborWall && neighborWall.isAlive)
                {
                    hasNeighbors[i] = true;
                }
                else if (neighborHidden && neighborHidden.isAlive)
                {
                    hasNeighbors[i] = true;
                }
                else
                {
                    hasNeighbors[i] = false;
                }
            } else
            {
                hasNeighbors[i] = false;
            }

            i++;
        }
    }

    public void UpdateShading()
    {
        // TODO: make shading more variable than on/off
        if (IsSurrounded())
        {
            wallShadow.gameObject.SetActive(true);
            boxCollider.enabled = false;
        }
        else
        {
            wallShadow.gameObject.SetActive(false);
            boxCollider.enabled = true;
        }
    }

    public bool IsSurrounded()
    {
        bool ret = true;
        foreach (bool b in hasNeighbors)
        {
            ret = ret && b;
        }
        return ret;
    }

    public void UpdateNeighbors()
    {
        foreach (Vector2 direction in neighborDirections)
        {
            Vector3Int cell = tileMap.WorldToCell(this.transform.position + (Vector3)direction);
            GameObject cellObj = tileMap.GetInstantiatedObject(cell);
            if (cellObj)
            {
                Wall neighborWall = cellObj.GetComponent<Wall>();
                HiddenRoom neighborHidden = cellObj.GetComponent<HiddenRoom>();
                if (neighborWall && neighborWall.isAlive)
                {
                    neighborWall.CheckNeighbors();
                    neighborWall.UpdateShading();
                }
                else if (neighborHidden && neighborHidden.isAlive)
                {
                    neighborHidden.CheckNeighbors();
                    neighborHidden.UpdateShading();
                }
            }
        }
    }

    public void Destroy()
    {
        isAlive = false;
        UpdateNeighbors();
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        FindObjectOfType<MinerManager>().AddWallToQueue(this);
    }
}
