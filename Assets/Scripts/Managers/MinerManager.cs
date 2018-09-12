using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerManager : MonoBehaviour {

    List<Miner> minerList = new List<Miner>();
    public TileMenu tileMenuPrefab;

    public static MinerManager instance;

    List<MinerTask> queuedTaskList = new List<MinerTask>(); // List of tasks to be done
    List<MinerTask> selectedTaskList = new List<MinerTask>(); // List of tasks currently being done by miners

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

    Miner NextAvailableMiner()
    {
        // TODO: add multi worker scheduling
        return minerList[0];
    }

    // TODO: this probably should get decoupled from miner manager
    public void CreateTileMenu(TaskableBase tile)
    {
        TileMenu tileMenu = Instantiate(tileMenuPrefab, tile.transform.position, Quaternion.Euler(Vector3.zero));
        tileMenu.CreateMenu(tile);
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

    // Forcibly schedules a new task and throws the old current task to the front of the queue
    // TODO select a unit to perform the task right now better
    public void DoTaskNow(MinerTask task)
    {
        MinerTask oldTask = minerList[0].ReplaceTask(task);
        if (oldTask != null)
        {
            AddTaskToStartOfQueue(oldTask);
        }
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
