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

    // Selects the next idle worker available nearest to the location capable of performing the given task
    Miner NextAvailableMinerForTask(Vector3 location, MinerTask task)
    {
        Miner retMiner = null;
        foreach (Miner miner in minerList)
        {
            if (miner.currentTask == null && task.CanMinerDo(miner))
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
    Miner NextMinerForTask(Vector3 location, MinerTask task)
    {
        Miner retMiner = null;
        retMiner = NextAvailableMinerForTask(location, task);
        if (retMiner)
        {
            return retMiner;
        }
        foreach (Miner miner in minerList)
        {
            if (task.CanMinerDo(miner))
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
        if (miner)
        {
            if (miner.currentTask)
            {
                DeselectTask(miner.currentTask);
            }
            task.Queue();
            miner.SwapTask(task);
            if (queuedTaskList.Contains(task))
            {
                queuedTaskList.Remove(task);
            }
            if (!selectedTaskList.Contains(task))
            {
                selectedTaskList.Add(task);
            }
        }
        else
        {
            //TODO: notify user that no workers can do this task
        }
    }

    public void DeselectTask(MinerTask task)
    {
        if (selectedTaskList.Contains(task))
        {
            selectedTaskList.Remove(task);
            queuedTaskList.Add(task);
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
        ForceTask(task, NextMinerForTask(task.TargetLocation(), task));
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
        completedTask.DestroySelf();
    }

    // Returns the next task and moves it from the queue to selected
    public MinerTask GrabNextTask(Miner miner)
    {
        foreach (MinerTask task in queuedTaskList)
        {
            if(task.CanMinerDo(miner))
            {
                MinerTask nextTask = queuedTaskList[0];
                queuedTaskList.Remove(nextTask);
                selectedTaskList.Add(nextTask);
                return nextTask;
            }
        }
        return null;
    }
}
