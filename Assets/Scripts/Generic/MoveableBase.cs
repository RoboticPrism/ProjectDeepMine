using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveableBase : MonoBehaviour {
    public float moveSpeed = 0.1f;
    public float rotationSpeed = 1f;

    protected List<Vector3Int> pathToTarget = null;
    protected Vector3Int target;

    protected UnityAction<TaskableBase> wallDestroyedListener;
    protected UnityAction<TaskableBase> buildingCreatedListener;

    public GameObject moveableBody;

    // Use this for initialization
    void Start () {
        wallDestroyedListener = new UnityAction<TaskableBase>(UpdatePath);
        EventManager.StartListening("WallDestroyed", wallDestroyedListener);
        buildingCreatedListener = new UnityAction<TaskableBase>(UpdatePath);
        EventManager.StartListening("BuildingCreated", buildingCreatedListener);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Generates a new path to a target or alerts of a failure to make path
    public bool MakePath(Vector3Int targetLocation)
    {
        target = targetLocation;
        pathToTarget = new AStar(TilemapManager.instance.wallTilemap.WorldToCell(this.transform.position), targetLocation, TilemapManager.instance.wallTilemap).Generate();
        if (pathToTarget.Count > 0)
        {
            // we dont want to move into the target, just next to it
            pathToTarget.Remove(target);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Generates a new path to the target location
    public bool MakePath(Vector2 targetLocation)
    {
        return MakePath(TilemapManager.instance.wallTilemap.WorldToCell(targetLocation));
    }

    // For now just triggers a full recheck of the path, could make this smarter though
    public void UpdatePath(TaskableBase tileBase)
    {
        MakePath(tileBase.transform.position);
    }

    public void RemovePath()
    {
        pathToTarget = null;
    }

    // Handles moving along a set path towards a target
    protected bool MoveAlongPathBehavior()
    {
        if (pathToTarget != null && pathToTarget.Count > 0)
        {
            // Account for grid offset
            Vector2 targetLocation = new Vector2(pathToTarget[0].x + 0.5f, pathToTarget[0].y + 0.5f);
            if (Vector3.Distance(this.transform.position, targetLocation) > 0.1)
            {
                MoveTowardsTarget(targetLocation);
                RotateAlongPath();
            }
            else
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
    protected void MoveTowardsTarget(Vector2 targetLocation)
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, targetLocation, moveSpeed);
    }

    // Handles rotating towards the next node on the path
    protected void RotateAlongPath()
    {
        if (pathToTarget != null && pathToTarget.Count > 0)
        {
            Vector2 targetLocation = new Vector2(pathToTarget[0].x + 0.5f, pathToTarget[0].y + 0.5f);

            Vector2 vectorToTarget = targetLocation - (Vector2)transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            moveableBody.transform.rotation = Quaternion.Slerp(moveableBody.transform.rotation, targetRotation, rotationSpeed);
            if (Quaternion.Angle(moveableBody.transform.rotation, targetRotation) < 5)
            {
                moveableBody.transform.rotation = targetRotation;
            }
        }
    }

    // Handles rotating towards the task's target
    protected bool RotateTowardsTargetBehavior(Task task)
    {
        if (task.target != null)
        {
            Vector2 targetLocation = task.TargetLocation();

            Vector2 vectorToTarget = targetLocation - (Vector2)transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            moveableBody.transform.rotation = Quaternion.Slerp(moveableBody.transform.rotation, targetRotation, rotationSpeed);
            if (Quaternion.Angle(moveableBody.transform.rotation, targetRotation) < 5)
            {
                moveableBody.transform.rotation = targetRotation;
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }
}
