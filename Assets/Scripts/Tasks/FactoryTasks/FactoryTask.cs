using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryTask : Task {
    
    [HideInInspector]
    public BuildingBase owner;

    [Header("Build Stats")]
    public float taskSpeed = 0.05f;
    public float taskCurrentTime = 0f;
    public float taskMaxTime = 100f;

    [Header("Health Bar")]
    public HealthBar healthBarPrefab;
    HealthBar healthBarInstance;
    public Color barColor;

    public override void Setup(TaskableBase target)
    {
        base.Setup(target);
        owner = target.GetComponent<BuildingBase>();
    }

    public override bool TaskAvailable()
    {
        return owner.built && !owner.broken;
    }

    protected virtual void StartTask()
    {
        healthBarInstance = Instantiate(healthBarPrefab, transform);
        healthBarInstance.transform.position += new Vector3(0, -0.4f, 0);
        healthBarInstance.UpdateColor(barColor);
    }

    public override void DoTask()
    {
        if(taskCurrentTime == 0)
        {
            StartTask();
        }
        if (taskCurrentTime < taskMaxTime)
        {
            healthBarInstance.UpdateBar(taskCurrentTime / taskMaxTime);
            taskCurrentTime += taskSpeed;
        }
        else
        {
            Complete();
        }
    }

    public virtual void Complete()
    {
        DestroySelf();
    }

    public virtual void Cancel()
    {
        DestroySelf();
    }

    public override void DestroySelf()
    {
        Destroy(healthBarInstance.gameObject);
        taskCurrentTime = 0;
        base.DestroySelf();
    }
}
