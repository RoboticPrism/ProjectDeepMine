using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// A room hidden to the player that is shown when accessed
public class HiddenRoom : MonoBehaviour {

    public Tilemap tileMap;

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
        CheckNeighbors();
        UpdateShading();
    }
	
	// Update is called once per frame
	void Update () {
		
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
            }
            else
            {
                hasNeighbors[i] = false;
            }

            i++;
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

    public void UpdateShading()
    {
        if (!IsSurrounded())
        {
            this.Destroy();   
        }
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
}
