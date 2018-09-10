using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour {

    public string taskName;
    public TaskableBase target;
    public Miner owner;
    bool queued = false;
    public enum priotities { QUEUE, QUEUE_NOW, REQUEUE_NOW, CANCEL }
    public SpriteRenderer spriteRenderer;

    public virtual void Setup (TaskableBase target)
    {
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

    public void Queue()
    {
        queued = true;
        ShowIcon();
    }

    public void Complete()
    {
        HideIcon();
    }

    public void Unqueue()
    {
        queued = false;
        owner = null;
    }

    public void Cancel()
    {
        queued = false;
        owner = null;
        HideIcon();
    }

    void ShowIcon()
    {
        if (spriteRenderer)
        {
            spriteRenderer.enabled = true;
        }
    }

    void HideIcon()
    {
        Debug.Log("hide icon");
        if (spriteRenderer)
        {
            spriteRenderer.enabled = false;
        }
    }
}
