using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// An object that sits in the grid and can be clicked to select it
public class TaskableBase : MonoBehaviour {
    public bool clickable = true;
    public List<Task> potentialTaskPrefabs = new List<Task>();
    public List<Task> potentialTasks = new List<Task>();

    // Use this for initialization
    protected virtual void Start () {
        foreach (Task taskPrefab in potentialTaskPrefabs)
        {
            Task newTask = Instantiate(taskPrefab, transform);
            newTask.Setup(this);
            potentialTasks.Add(newTask);
        }
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    // FixedUpdate is called once per tick
    protected virtual void FixedUpdate()
    {

    }

    // On mouse down, pop menu
    private void OnMouseDown()
    {
        UIHoverListener uhl = FindObjectOfType<UIHoverListener>();
        if (clickable && (uhl == null || !uhl.isUIOverride))
        {
            FindObjectOfType<MinerManager>().CreateTileMenu(this);
        }
    }
}
