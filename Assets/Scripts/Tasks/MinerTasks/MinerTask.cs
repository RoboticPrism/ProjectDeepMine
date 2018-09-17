using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerTask : Task {

    [HideInInspector]
    public Miner owner;
    bool queued = false;

    // returns true if the current task is feasible
    public override bool TaskAvailable()
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
        if (spriteRenderer)
        {
            spriteRenderer.enabled = false;
        }
    }
}
