using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// An object that sits in the grid and can be clicked to select it
public class TaskableBase : MonoBehaviour {
    public bool clickable = true;
    public Task currentTask;

    // Use this for initialization
    protected virtual void Start () {
        
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    // FixedUpdate is called once per tick
    protected virtual void FixedUpdate()
    {

    }

    // Instantiates a task
    public virtual Task CreateTask(Task task)
    {
        
        currentTask = Instantiate(task);
        currentTask.Setup(this);
        if (currentTask.GetComponent<MinerTask>())
        {
            MinerManager.instance.AddTaskToQueue(currentTask.GetComponent<MinerTask>());
        }
        return currentTask;
    }

    // On mouse down, pop menu
    private void OnMouseDown()
    {
        UIHoverListener uhl = FindObjectOfType<UIHoverListener>();
        if (clickable && (uhl == null || !uhl.isUIOverride))
        {
            FindObjectOfType<HoverManager>().SelectTile(this);
        }
    }
}
