using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour {
    public float moveSpeed = 1f;
    public float mineSpeed = 1f;
    List<Task> taskList = new List<Task>();

	// Use this for initialization
	void Start () {
        FindObjectOfType<MinerManager>().AddMiner(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        DoNextTask();
    }

    public void AddTask(Task task)
    {
        taskList.Add(task);
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
        // TODO: A* once grid is more established
        this.transform.position = Vector2.MoveTowards(this.transform.position, targetLocation, moveSpeed);
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
            mineTask.targetWall.Destroy();
            taskList.Remove(mineTask);
        }
    }
}
