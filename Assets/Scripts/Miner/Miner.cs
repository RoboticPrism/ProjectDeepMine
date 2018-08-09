using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Miner : MonoBehaviour {
    public float moveSpeed = 0.1f;
    public float mineSpeed = 1f;
    public float buildSpeed = 1f;
    Task currentTask;
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
        if(taskList.Count > 0 && currentTask == null)
        {
            SelectTask(taskList[0]);
        }
        else if (currentTask != null)
        {
            if (currentTask is MineTask)
            {
                DoMineTask((MineTask)currentTask);
            }
            else if (currentTask is BuildTask)
            {
                DoBuildTask((BuildTask)currentTask);
            }
            else if (currentTask is RepairTask)
            {
                DoRepairTask((RepairTask)currentTask);
            }
            else if (currentTask is DeconstructTask)
            {
                DoDeconstructTask((DeconstructTask)currentTask);
            }
        }
    }

    public void SelectTask(Task newTask)
    {
        currentTask = newTask;
        MakePath(newTask.TargetLocation());
    }

    public void MakePath(Vector2 targetLocation)
    {
        pathToTarget = new AStar(tileMap.WorldToCell(this.transform.position), tileMap.WorldToCell(targetLocation), tileMap).Generate();
        // we dont want to move into the target, just next to it
        pathToTarget.RemoveAt(pathToTarget.Count - 1);
    }

    //  Handles pathfinding a long the current route to a location
    private void MoveTowards(Vector2 targetLocation)
    {  
        if(Vector3.Distance(this.transform.position, pathToTarget[0]) > 0.1)
        {
            this.transform.position =  Vector2.MoveTowards(this.transform.position, new Vector2(pathToTarget[0].x, pathToTarget[0].y), moveSpeed);
        } else
        {
            pathToTarget.RemoveAt(0);
        }
    }

    // Handles moving towards and mining a wall
    private void DoMineTask(MineTask mineTask)
    {
        // move to wall
        if (pathToTarget != null && pathToTarget.Count > 0)
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
            currentTask = null;
            pathToTarget = null;
        }
    }

    // Handles moving towards and building a building
    public void DoBuildTask(BuildTask buildTask)
    {
        // move to building
        if (pathToTarget != null && pathToTarget.Count > 0)
        {
            MoveTowards(buildTask.targetBuilding.transform.position);
        }
        // build building
        else if (buildTask.targetBuilding.buildAmount < buildTask.targetBuilding.buildMax)
        {
            buildTask.targetBuilding.AddConstruction(buildSpeed);
        }
        // end task
        else
        {
            taskList.Remove(buildTask);
            currentTask = null;
            pathToTarget = null;
        }
    }

    // Handles moving towards a building and then repairing it
    public void DoRepairTask(RepairTask repairTask)
    {
        // move to building
        if (pathToTarget != null && pathToTarget.Count > 0)
        {
            MoveTowards(repairTask.targetBuilding.transform.position);
        }
        // fix up building
        else if (repairTask.targetBuilding.life < repairTask.targetBuilding.lifeMax)
        {
            repairTask.targetBuilding.AddLife((int)buildSpeed);
        }
        // end task
        else
        {
            taskList.Remove(repairTask);
            currentTask = null;
            pathToTarget = null;
        }
    }

    // Handles moving towards a building and then deconstructing it
    public void DoDeconstructTask(DeconstructTask deconstructTask)
    {
        // move to building
        if (pathToTarget != null && pathToTarget.Count > 0)
        {
            MoveTowards(deconstructTask.targetBuilding.transform.position);
        }
        // tear down building
        else if (deconstructTask.targetBuilding.buildAmount >= 0)
        {
            deconstructTask.targetBuilding.AddConstruction(-buildSpeed);
        }
        // sell building
        else
        {
            taskList.Remove(deconstructTask);
            currentTask = null;
            pathToTarget = null;
        }
    }
}
