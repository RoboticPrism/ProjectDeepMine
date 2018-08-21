using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Miner : MoveableBase {
    public float mineSpeed = 1f;
    public float buildSpeed = 1f;

    Task currentTask;
    List<Task> taskList = new List<Task>();

    // Use this for initialization
    void Start () {
        MinerManager.instance.AddMiner(this);
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
        if (newTask.target && MakePath(newTask.TargetLocation()))
        {
            currentTask = newTask;
        }
        else
        {
            EndTask(newTask);
        }
    }

    public void EndTask(Task task)
    {
        currentTask = null;
        taskList.Remove(task);
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
