using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar {
    Vector3Int start;
    Vector3Int target;
    Tilemap tilemap;

    Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
    Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
    Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();

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

    // Dictionary for a list of directions we need to check when trying to move diagonally
    Dictionary<Vector2Int, List<Vector2Int>> diagonalChecks = new Dictionary<Vector2Int, List<Vector2Int>>
    {
        { Vector2Int.up + Vector2Int.right, new List<Vector2Int> { Vector2Int.up, Vector2Int.right } },
        { Vector2Int.right + Vector2Int.down, new List<Vector2Int> { Vector2Int.right, Vector2Int.down } },
        { Vector2Int.down + Vector2Int.left, new List<Vector2Int> { Vector2Int.down, Vector2Int.left } },
        { Vector2Int.left + Vector2Int.up, new List<Vector2Int> { Vector2Int.left, Vector2Int.up } },
    };

    public AStar(Vector3Int start, Vector3Int target, Tilemap tilemap)
    {
        this.start = start;
        this.target = target;
        this.tilemap = tilemap;
    }

    public List<Vector3Int> Generate ()
    {
        List<Vector3Int> closedList = new List<Vector3Int>();
        PriorityQueue<Vector3Int> queue = new PriorityQueue<Vector3Int>();
        queue.Enqueue(start, 0f);

        gScore[start] = 0f;

        while(queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();
            closedList.Add(current);


            if(current == target)
            {
                break;
            }
            // Check linear directions
            foreach (Vector2Int direction in linearDirections)
            {
                Vector3Int currentNeighbor = current + new Vector3Int(direction.x, direction.y, 0);
                if (closedList.Contains(currentNeighbor))
                {
                    continue;
                }
                GameObject neighborObj = tilemap.GetInstantiatedObject(currentNeighbor);
                // check for wall at the target location
                if (neighborObj && neighborObj.GetComponent<Wall>() && currentNeighbor != target)
                {
                    continue;
                }

                float score = gScore[current] + Vector3.Distance(current, currentNeighbor);

                if (!gScore.ContainsKey(currentNeighbor)) {
                    gScore.Add(currentNeighbor, score);
                    cameFrom.Add(currentNeighbor, current);
                    queue.Enqueue(currentNeighbor, score + Vector3.Distance(currentNeighbor, target));
                }
                else if(score < gScore[currentNeighbor])
                {
                    gScore[currentNeighbor] = score;
                    cameFrom[currentNeighbor] = current;
                    queue.Enqueue(currentNeighbor, score + Vector3.Distance(currentNeighbor, target));
                }
            }
            // Check diagonal moves
            foreach (Vector2Int direction in diagonalDirections)
            {
                Vector3Int currentNeighbor = current + new Vector3Int(direction.x, direction.y, 0);
                GameObject neighborObj = tilemap.GetInstantiatedObject(currentNeighbor);
                // check for a wall at target location
                if (neighborObj && neighborObj.GetComponent<Wall>())
                {
                    continue;
                }
                // check if the diagonal movement is blocked by a wall
                bool blocked = false;
                List<Vector2Int> checkDirections = diagonalChecks[direction];
                foreach(Vector2Int check in checkDirections)
                {
                    Vector3Int checkPoint = current + new Vector3Int(check.x, check.y, 0);
                    GameObject checkObj = tilemap.GetInstantiatedObject(checkPoint);
                    if (checkObj && checkObj.GetComponent<Wall>())
                    {
                        blocked = true;
                        break;
                    }
                }
                if (blocked)
                {
                    break;
                }

                float score = gScore[current] + Vector3.Distance(current, currentNeighbor);

                if (!gScore.ContainsKey(currentNeighbor))
                {
                    gScore.Add(currentNeighbor, score);
                    cameFrom.Add(currentNeighbor, current);
                    queue.Enqueue(currentNeighbor, score + Vector3.Distance(currentNeighbor, target));
                }
                else if (score < gScore[currentNeighbor])
                {
                    gScore[currentNeighbor] = score;
                    cameFrom[currentNeighbor] = current;
                    queue.Enqueue(currentNeighbor, score + Vector3.Distance(currentNeighbor, target));
                }
            }
        }

        List<Vector3Int> path = new List<Vector3Int>();

        Vector3Int nextPath = target;

        while (nextPath != start)
        {
            path.Insert(0, nextPath);
            nextPath = cameFrom[nextPath];
        }
        //path.RemoveAt(path.Count - 1);
        return path;
    }
}
