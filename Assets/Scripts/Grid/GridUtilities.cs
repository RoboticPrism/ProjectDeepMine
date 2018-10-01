using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridUtilities {

    Tilemap tilemap;

    List<Vector2Int> linearDirections = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    List<Vector2Int> diagonalDirections = new List<Vector2Int> {
        Vector2Int.up + Vector2Int.right,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.left + Vector2Int.up
    };

    public GridUtilities(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    public static Vector3Int WorldToCell(Tilemap grid, Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

    public List<Vector3Int> GetEmptyNeighbors(GameObject gameObject, bool includeDiagonals = false)
    {
        return GetEmptyNeighbors(tilemap.WorldToCell(gameObject.transform.position), includeDiagonals);
    }

    public List<Vector3Int> GetEmptyNeighbors(Vector3Int position, bool includeDiagonals = false)
    {
        List<Vector3Int> retList = new List<Vector3Int>();
        foreach (Vector2Int direction in linearDirections)
        {
            Vector3Int currentNeighborPosition = position + new Vector3Int(direction.x, direction.y, 0);
            GameObject neighborObj = tilemap.GetInstantiatedObject(currentNeighborPosition);
            if(neighborObj == null)
            {
                retList.Add(currentNeighborPosition);
            }
        }
        if(includeDiagonals)
        {
            foreach (Vector2Int direction in diagonalDirections)
            {
                Vector3Int currentNeighborPosition = position + new Vector3Int(direction.x, direction.y, 0);
                GameObject neighborObj = tilemap.GetInstantiatedObject(currentNeighborPosition);
                if (neighborObj == null)
                {
                    retList.Add(currentNeighborPosition);
                }
            }
        }

        return retList;
    }
}
