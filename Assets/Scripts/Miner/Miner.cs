using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Miner : MoveableBase {
    public float mineSpeed = 1f;
    public float buildSpeed = 1f;

    public Task currentTask;

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

    private void DoNextTask()
    {
        if(currentTask == null)
        {
            SelectTask(MinerManager.instance.GrabNextTask());
        }
        else if (currentTask != null)
        {
            if (currentTask is MineTask)
            {
                if (DoMineTask((MineTask)currentTask))
                {
                    CompleteTask();
                }
            }
            else if (currentTask is BuildTask)
            {
                if (DoBuildTask((BuildTask)currentTask))
                {
                    CompleteTask();
                }
            }
            else if (currentTask is RepairTask)
            {
                if (DoRepairTask((RepairTask)currentTask))
                {
                    CompleteTask();
                }
            }
            else if (currentTask is DeconstructTask)
            {
                if (DoDeconstructTask((DeconstructTask)currentTask))
                {
                    CompleteTask();
                }
            }
        }
    }

    public void SelectTask(Task newTask)
    {
        if (newTask != null)
        {
            if (newTask.target && MakePath(newTask.TargetLocation()))
            {
                currentTask = newTask;
                currentTask.owner = this;
            }
            else
            {
                CompleteTask();
            }
        }
    }

    public Task ReplaceTask(Task newTask)
    {
        Task oldTask = currentTask;
        if (newTask != null)
        {
            if (newTask.target && MakePath(newTask.TargetLocation()))
            {
                UnqueueTask(oldTask);
                SelectTask(newTask);
                return oldTask;
            }
            else
            {
                CompleteTask();
            }
        }
        return null;
    }

    public void UnqueueTask(Task task)
    {
        if (task != null)
        {
            task.owner = null;
            task.queued = false;
        }
        RemovePath();
    }

    public void CompleteTask()
    {
        MinerManager.instance.CompleteTask(currentTask);
        currentTask = null;
        RemovePath();
    }

    ///////////////////
    // TASK HANDLING //
    ///////////////////

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
