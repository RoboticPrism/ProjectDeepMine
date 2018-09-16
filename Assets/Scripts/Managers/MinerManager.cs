using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerManager : MonoBehaviour {

    List<Miner> minerList = new List<Miner>();

    public static MinerManager instance;

    public List<MinerTask> queuedTaskList = new List<MinerTask>(); // List of tasks to be done
    public List<MinerTask> selectedTaskList = new List<MinerTask>(); // List of tasks currently being done by miners

    // Use this for initialization
    void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddMiner(Miner miner)
    {
        minerList.Add(miner);
    }

    public void RemoveMiner(Miner miner)
    {
        minerList.Remove(miner);
    }

    // Selects the next idle worker available
    Miner NextAvailableMiner()
    {
        foreach(Miner miner in minerList)
        {
            if(miner.currentTask == null)
            {
                return miner;
            }
        }
        return null;
    }

    // Selects the next idle worker available nearest to the location
    Miner NextAvailableMiner(Vector3 location)
    {
        Miner retMiner = null;
        foreach (Miner miner in minerList)
        {
            if (miner.currentTask == null)
            {
                if (retMiner != null)
                {
                    // TODO: experiment with using AStar distance calcs instead
                    if(Vector3.Distance(miner.transform.position, location) < Vector3.Distance(retMiner.transform.position, location))
                    {
                        retMiner = miner;
                    }
                }
                else
                {
                    retMiner = miner;
                }
            }
        }
        return retMiner;
    }

    // Forcibly selects a miner, even if its busy
    Miner NextMiner()
    {
        Miner retMiner;
        retMiner = NextAvailableMiner();
        if(retMiner)
        {
            return retMiner;
        }
        else
        {
            return minerList[0];
        }
    }

    // Forcibly selects the miner closest to a location, even if its busy
    Miner NextMiner(Vector3 location)
    {
        Miner retMiner = null;
        retMiner = NextAvailableMiner(location);
        if (retMiner)
        {
            return retMiner;
        }
        foreach (Miner miner in minerList)
        {
            if (retMiner != null)
            {
                // TODO: experiment with using AStar distance calcs instead
                if (Vector3.Distance(miner.transform.position, location) < Vector3.Distance(retMiner.transform.position, location))
                {
                    retMiner = miner;
                }
            }
            else
            {
                retMiner = miner;
            }
        }
        return retMiner;
    }

    public void AddTaskToQueue(MinerTask task)
    {
        queuedTaskList.Add(task);
        task.Queue();
    }

    public void AddTaskToStartOfQueue(MinerTask task)
    {
        queuedTaskList.Insert(0, task);
        task.Queue();
    }

    public void ForceTask(MinerTask task, Miner miner)
    {
        if(miner.currentTask)
        {
            DeselectTask(task);
        }
        if (!selectedTaskList.Contains(task))
        {
            selectedTaskList.Add(task);
        }
        if(queuedTaskList.Contains(task))
        {
            queuedTaskList.Remove(task);
        }
        task.Queue();
        miner.SwapTask(task);
    }

    public void DeselectTask(MinerTask task)
    {
        if (selectedTaskList.Contains(task))
        {
            selectedTaskList.Remove(task);
        }
        if (!queuedTaskList.Contains(task))
        {
            queuedTaskList.Insert(0, task);
        }
        task.Unqueue();
    }

    // Forcibly schedules a new task and throws the old current task to the front of the queue
    // TODO select a unit to perform the task right now better
    public void DoTaskNow(MinerTask task)
    {
        ForceTask(task, NextMiner(task.TargetLocation()));
    }

    // Forcible removes the task early
    public void CancelTask(MinerTask task)
    {
        if(queuedTaskList.Contains(task))
        {
            queuedTaskList.Remove(task);
            task.Cancel();
        } else if (selectedTaskList.Contains(task))
        {
            task.owner.CompleteTask();
            task.Cancel();
        }
    }

    // Removes the completed task from the selected list
    public void CompleteTask(MinerTask completedTask)
    {
        selectedTaskList.Remove(completedTask);
        completedTask.Complete();
    }

    // Returns the next task and moves it from the queue to selected
    public MinerTask GrabNextTask()
    {
        if (queuedTaskList.Count > 0)
        {
            MinerTask nextTask = queuedTaskList[0];
            queuedTaskList.Remove(nextTask);
            selectedTaskList.Add(nextTask);
            return nextTask;
        }
        else
        {
            return null;
        }
    }
}
