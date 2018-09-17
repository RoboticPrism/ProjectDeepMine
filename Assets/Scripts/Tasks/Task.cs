using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour {

    public string taskName;
    [HideInInspector]
    public TaskableBase target;

    [Header("Instance Connections")]
    public SpriteRenderer spriteRenderer;

    public virtual void Setup(TaskableBase target)
    {
        this.target = target;
        this.transform.position = target.transform.position;
        EventManager.TriggerEvent("TaskCreated", target);
    }

    public virtual void Complete()
    {
        target.currentTask = null;
        EventManager.TriggerEvent("TaskDestroyed", target);
        DestroySelf();
    }

    public virtual bool TaskAvailable()
    {
        return true;
    }

    public virtual void DoTask()
    {

    }

    public Vector3 TargetLocation()
    {
        return target.transform.position;
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
