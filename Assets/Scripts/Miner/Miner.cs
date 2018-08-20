using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Miner : MonoBehaviour {
    public float moveSpeed = 0.1f;
    public float rotationSpeed = 1f;
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
                if (DoMineTask((MineTask)currentTask))
                {
                    EndTask(currentTask);
                }
            }
            else if (currentTask is BuildTask)
            {
                if (DoBuildTask((BuildTask)currentTask))
                {
                    EndTask(currentTask);
                }
            }
            else if (currentTask is RepairTask)
            {
                if (DoRepairTask((RepairTask)currentTask))
                {
                    EndTask(currentTask);
                }
            }
            else if (currentTask is DeconstructTask)
            {
                if (DoDeconstructTask((DeconstructTask)currentTask))
                {
                    EndTask(currentTask);
                }
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
            EndTask(newTask);
        }
    }

    public void EndTask(Task task)
    {
        currentTask = null;
        taskList.Remove(task);
    }

    // Generates a new path to a target or alerts of a failure to make path
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

    // For now just triggers a full recheck of the path, could make this smarter though
    public void UpdatePath(ClickableTileBase tileBase)
    {
        MakePath(tileBase.transform.position);
    }

    // Handles moving along a set path towards a target
    private bool MoveAlongPathBehavior()
    {
        if (pathToTarget != null && pathToTarget.Count > 0)
        {
            // Account for grid offset
            Vector2 targetLocation = new Vector2(pathToTarget[0].x + 0.5f, pathToTarget[0].y + 0.5f);
            if (Vector3.Distance(this.transform.position, targetLocation) > 0.1)
            {
                MoveTowardsTarget(targetLocation);
                RotateAlongPath();
            } else
            {
                pathToTarget.RemoveAt(0);
            }
            return false;
        }
        else
        {
            pathToTarget = null;
            return true;
        }
    }

    // Handles moving towards the given target
    private void MoveTowardsTarget(Vector2 targetLocation)
    {
        this.transform.position =  Vector2.MoveTowards(this.transform.position, targetLocation, moveSpeed);
    }

    // Handles rotating towards the next node on the path
    private void RotateAlongPath()
    {
        if (pathToTarget != null && pathToTarget.Count > 0)
        {
            Vector2 targetLocation = new Vector2(pathToTarget[0].x + 0.5f, pathToTarget[0].y + 0.5f);

            Vector2 vectorToTarget = targetLocation - (Vector2)transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 5)
            {
                transform.rotation = targetRotation;
            }
        }
    }

    // Handles rotating towards the task's target
    private bool RotateTowardsTargetBehavior(Task task)
    {
        if (task.target != null)
        {
            Vector2 targetLocation = task.TargetLocation();

            Vector2 vectorToTarget = targetLocation - (Vector2)transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 5)
            {
                transform.rotation = targetRotation;
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    // Handles moving towards and mining a wall
    private bool DoMineTask(MineTask mineTask)
    {
        return
             MoveAlongPathBehavior() &&
             RotateTowardsTargetBehavior(mineTask) &&
             mineTask.DoTask(mineSpeed);
    }

    // Handles moving towards and building a building
    public bool DoBuildTask(BuildTask buildTask)
    {
        return
            MoveAlongPathBehavior() &&
            RotateTowardsTargetBehavior(buildTask) &&
            buildTask.DoTask(buildSpeed);
    }

    // Handles moving towards a building and then repairing it
    public bool DoRepairTask(RepairTask repairTask)
    {
        return
            MoveAlongPathBehavior() &&
            RotateTowardsTargetBehavior(repairTask) &&
            repairTask.DoTask((int)buildSpeed);
    }

    // Handles moving towards a building and then deconstructing it
    public bool DoDeconstructTask(DeconstructTask deconstructTask)
    {
        return
            MoveAlongPathBehavior() &&
            RotateTowardsTargetBehavior(deconstructTask) &&
            deconstructTask.DoTask(buildSpeed);
    }
}
