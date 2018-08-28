using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

    public string taskName;
    public ClickableTileBase target;
    public Miner owner;
    public bool queued = false;
    public enum priotities { QUEUE, QUEUE_NOW, REQUEUE_NOW, CANCEL }

    public Task (string taskName, ClickableTileBase target)
    {
        this.taskName = taskName;
        this.target = target;
    }

    public Vector3 TargetLocation()
    {
        return target.transform.position;
    }

    // returns true if the current task is feasible
    public virtual bool TaskAvailable()
    {
        return true;
    }

    public virtual bool CanQueue()
    {
        return TaskAvailable() && !queued;
    }

    public virtual bool CanQueueNow()
    {
        return TaskAvailable() && !queued;
    }

    public virtual bool CanRequeueNow()
    {
        return TaskAvailable() && queued;
    }

    public virtual bool CanCancel()
    {
        return TaskAvailable() && queued;
    }
}
