using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Miner : MonoBehaviour {
    public float moveSpeed = 0.1f;
    public float mineSpeed = 1f;
    List<Task> taskList = new List<Task>();

    List<Vector3Int> pathToTarget = null;

    Tilemap tileMap;

    List<Vector2Int> neighborDirections = new List<Vector2Int> {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.up + Vector2Int.right,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.left + Vector2Int.up
    };

    // Use this for initialization
    void Start () {
        tileMap = FindObjectOfType<TilemapManager>().wallTilemap;
        FindObjectOfType<MinerManager>().AddMiner(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        DoNextTask();
    }

    // adds a task to the end of the queue
    public void AddTask(Task task)
    {
        taskList.Add(task);
    }

    // adds a task to the front of the queue, interrupting any current tasks
    public void AddTaskNow(Task task)
    {
        taskList.Insert(0, task);
        pathToTarget = null;
    }

    private void DoNextTask()
    {
        if(taskList.Count > 0)
        {
            Task nextTask = taskList[0];
            if ((MineTask)nextTask != null)
            {
                DoMineTask((MineTask)nextTask);
            }
        }
    }

    // Handles pathfinding towards a location
    private void MoveTowards(Vector2 targetLocation)
    {
        // generate new path
        if (pathToTarget == null || pathToTarget.Count == 0)
        {
            pathToTarget = new AStar(tileMap.WorldToCell(this.transform.position), tileMap.WorldToCell(targetLocation), tileMap).Generate();
        }
        // follow path
        else if (pathToTarget.Count > 0)
        {    
            if(Vector3.Distance(this.transform.position, pathToTarget[0]) > 0.1)
            {
                this.transform.position =  Vector2.MoveTowards(this.transform.position, new Vector2(pathToTarget[0].x, pathToTarget[0].y), moveSpeed);
            } else
            {
                pathToTarget.RemoveAt(0);
            }
        }
        
    }

    // Handles moving towards and mining a wall
    private void DoMineTask(MineTask mineTask)
    {
        // move to wall
        if (Vector2.Distance(this.transform.position, mineTask.targetWall.transform.position) > 1)
        {
            MoveTowards(mineTask.targetWall.transform.position);
        }
        // drill wall
        else if (mineTask.targetWall.destroyTime > 0)
        {
            mineTask.targetWall.MineWall(mineSpeed);
        }
        // break wall
        else
        {
            mineTask.targetWall.DestroySelf();
            taskList.Remove(mineTask);
        }
    }
}
