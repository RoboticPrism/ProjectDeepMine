using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Miner : MonoBehaviour {
    public float moveSpeed = 0.1f;
    public float mineSpeed = 1f;
    public float buildSpeed = 1f;
    Task currentTask;
    List<Task> taskList = new List<Task>();

    List<Vector3Int> pathToTarget = null;
    Vector3Int target;

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

    private UnityAction<ClickableTileBase> wallDestroyedListener;
    private UnityAction<ClickableTileBase> buildingCreatedListener;

    // Use this for initialization
    void Start () {
        tileMap = FindObjectOfType<TilemapManager>().wallTilemap;
        FindObjectOfType<MinerManager>().AddMiner(this);
        wallDestroyedListener = new UnityAction<ClickableTileBase>(UpdatePath);
        EventManager.StartListening("WallDestroyed", wallDestroyedListener);
        buildingCreatedListener = new UnityAction<ClickableTileBase>(UpdatePath);
        EventManager.StartListening("BuildingCreated", buildingCreatedListener);
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
        task.SetTaskOwner(this);
    }

    public void RemoveTask(Task task)
    {
        task.SetTaskOwner(null);
        taskList.Remove(task);
    }

    // adds a task to the front of the queue, interrupting any current tasks
    public void AddTaskNow(Task task)
    {
        taskList.Insert(0, task);
        task.SetTaskOwner(this);
        SelectTask(task);
    }

    // removes a task from further down the queue and places it on top of the queue
    public void PrioritizeTask(Task task)
    {
        RemoveTask(task);
        AddTaskNow(task);
        SelectTask(task);
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
        if (newTask.target)
        {
            currentTask = newTask;
            MakePath(newTask.TargetLocation());
        } else
        {
            RemoveTask(newTask);
        }
    }

    public void MakePath(Vector3Int targetLocation)
    {
        target = targetLocation;
        pathToTarget = new AStar(tileMap.WorldToCell(this.transform.position), targetLocation, tileMap).Generate();
        if (pathToTarget.Count > 0)
        {
            // we dont want to move into the target, just next to it
            pathToTarget.Remove(target);
        }
        else
        {
            // TODO: make a UI alert popup here
            Debug.Log("cant complete task");
            taskList.Remove(currentTask);
            currentTask = null;
        }
    }

    public void MakePath(Vector2 targetLocation)
    {
        MakePath(tileMap.WorldToCell(targetLocation));
    }

    public void UpdatePath(ClickableTileBase tileBase)
    {
        MakePath(tileBase.transform.position);
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
            MoveTowards(mineTask.TargetLocation());
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
            MoveTowards(buildTask.TargetLocation());
        }
        // build building
        else if (buildTask.targetBuilding.buildAmount < buildTask.targetBuilding.buildMax)
        {
            buildTask.targetBuilding.AddConstruction(buildSpeed);
        }
        // end task
        else
        {
            RemoveTask(buildTask);
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
            MoveTowards(repairTask.TargetLocation());
        }
        // fix up building
        else if (repairTask.targetBuilding.life < repairTask.targetBuilding.lifeMax)
        {
            repairTask.targetBuilding.AddLife((int)buildSpeed);
        }
        // end task
        else
        {
            RemoveTask(repairTask);
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
            MoveTowards(deconstructTask.TargetLocation());
        }
        // tear down building
        else if (deconstructTask.targetBuilding.buildAmount >= 0)
        {
            deconstructTask.targetBuilding.AddConstruction(-buildSpeed);
        }
        // sell building
        else
        {
            RemoveTask(deconstructTask);
            currentTask = null;
            pathToTarget = null;
        }
    }
}
