using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerManager : MonoBehaviour {

    List<Miner> minerList = new List<Miner>();
    public TileMenu tileMenuPrefab;

    public static MinerManager instance;

    List<Task> queuedTaskList = new List<Task>(); // List of tasks to be done
    List<Task> selectedTaskList = new List<Task>(); // List of tasks currently being done by miners

    // Use this for initialization
    void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddMiner(Miner miner)
    {
        Debug.Log("added miner");
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

    public void CreateTileMenu(TaskableBase tile)
    {
        TileMenu tileMenu = Instantiate(tileMenuPrefab, tile.transform.position, Quaternion.Euler(Vector3.zero));
        tileMenu.CreateMenu(tile);
    }

    public void AddTaskToQueue(Task task)
    {
        queuedTaskList.Add(task);
        task.Queue();
    }

    public void AddTaskToStartOfQueue(Task task)
    {
        queuedTaskList.Insert(0, task);
        task.Queue();
    }

    // Forcibly schedules a new task and throws the old current task to the front of the queue
    // TODO select a unit to perform the task right now better
    public void DoTaskNow(Task task)
    {
        Task oldTask = minerList[0].ReplaceTask(task);
        if (oldTask != null)
        {
            AddTaskToStartOfQueue(oldTask);
        }
    }

    // Forcible removes the task early
    public void CancelTask(Task task)
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
    public void CompleteTask(Task completedTask)
    {
        selectedTaskList.Remove(completedTask);
        completedTask.Complete();
    }

    // Returns the next task and moves it from the queue to selected
    public Task GrabNextTask()
    {
        if (queuedTaskList.Count > 0)
        {
            Task nextTask = queuedTaskList[0];
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
