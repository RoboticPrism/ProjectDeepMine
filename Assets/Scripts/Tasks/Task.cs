using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour {

    public string taskName;
    public TaskableBase target;
    public SpriteRenderer spriteRenderer;

    public virtual void Setup(TaskableBase target)
    {
        this.target = target;
    }

    public virtual bool TaskAvailable()
    {
        return true;
    }

    public Vector3 TargetLocation()
    {
        return target.transform.position;
    }
}
