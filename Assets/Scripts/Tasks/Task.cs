using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

    public string taskName;
    public ClickableTileBase target;
    public enum priotities { QUEUE, QUEUE_NOW, REQUEUE_NOW }
    public priotities priority;

    public Task (string taskName, ClickableTileBase target, priotities priority)
    {
        this.taskName = taskName;
        this.target = target;
        this.priority = priority;
    }

    public Vector3 TargetLocation()
    {
        return target.transform.position;
    }

    public void SetTaskOwner(Miner miner)
    {
        this.target.taskOwner = miner;
    }

    // returns true if the current task is feasible
    public virtual bool TaskAvailable()
    {
        // can only schedule a new task if this task isn't schedules
        if (priority == priotities.QUEUE || priority == priotities.QUEUE_NOW)
        {
            return (this.target.taskOwner == null);
        }
        // can only boost priority if task has already been scheduled
        else
        {
            return (this.target.taskOwner != null);
        }
    }
}
