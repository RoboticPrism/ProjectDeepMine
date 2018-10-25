using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveableBase : MonoBehaviour {
    public float moveSpeed = 0.1f;
    public float rotationSpeed = 1f;

    bool triggerPathRefresh = false; 

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
      
    public IEnumerator MoveTo(Vector3Int targetLocation, bool directlyOnTarget = false)
    {
        pathToTarget = new AStar(TilemapManager.instance.wallTilemap.WorldToCell(transform.position), targetLocation, TilemapManager.instance.wallTilemap).Generate();
        // remove the last point so we move next to the target, not on the target
        if (!directlyOnTarget)
        {
            pathToTarget.RemoveAt(pathToTarget.Count - 1);
        }
        while (pathToTarget.Count > 0)
        {
            // Handle calculating a new route when the map changes
            if(triggerPathRefresh)
            {
                pathToTarget = new AStar(TilemapManager.instance.wallTilemap.WorldToCell(transform.position), targetLocation, TilemapManager.instance.wallTilemap).Generate();
                // remove the last point so we move next to the target, not on the target
                if (!directlyOnTarget)
                {
                    pathToTarget.RemoveAt(pathToTarget.Count - 1);
                }
                triggerPathRefresh = false;
            }

            Vector2 pathTargetLocation = new Vector2(pathToTarget[0].x + 0.5f, pathToTarget[0].y + 0.5f);

            while (Vector2.Distance(pathTargetLocation, transform.position) > 0.1f)
            {
                // Move towards
                transform.position = Vector2.MoveTowards(transform.position, pathTargetLocation, moveSpeed);

                // Rotate towards
                Vector2 vectorToTarget = pathTargetLocation - (Vector2)transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                moveableBody.transform.rotation = Quaternion.Slerp(moveableBody.transform.rotation, targetRotation, rotationSpeed);
                if (Quaternion.Angle(moveableBody.transform.rotation, targetRotation) < 5)
                {
                    moveableBody.transform.rotation = targetRotation;
                }

                yield return new WaitForFixedUpdate();
            }
            transform.position = pathTargetLocation;
            pathToTarget.RemoveAt(0);
        }
    }

    public IEnumerator RotateTowards(Vector2 targetLocation)
    {
        Vector2 vectorToTarget = targetLocation - (Vector2)transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        while (Quaternion.Angle(moveableBody.transform.rotation, targetRotation) > 5) {
            // Update target rotation in the event the target is moving
            vectorToTarget = targetLocation - (Vector2)transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            moveableBody.transform.rotation = Quaternion.Slerp(moveableBody.transform.rotation, targetRotation, rotationSpeed);
            yield return new WaitForFixedUpdate();
        }
        moveableBody.transform.rotation = targetRotation;
    }

    // For now just triggers a full recheck of the path, could make this smarter though
    public void UpdatePath(TaskableBase tileBase)
    {
        triggerPathRefresh = true;
    }
}
