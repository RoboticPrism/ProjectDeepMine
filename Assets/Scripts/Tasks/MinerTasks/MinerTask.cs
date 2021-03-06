﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerTask : Task {
    
    public Miner owner;
    bool queued = false;

    // returns true if the current task is feasible
    public override bool TaskAvailable()
    {
        return true;
    }

    // Starts the associtated coroutine on the miner
    public virtual void StartTaskCoroutine(Miner miner)
    {
        
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
        DestroySelf();
    }

    public virtual bool CanMinerDo(Miner miner)
    {
        return true;
    }
}
