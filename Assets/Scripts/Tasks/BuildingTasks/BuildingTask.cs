using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTask : Task {
    
    public BuildingBase owner;
    public float taskSpeed = 0.05f;
    public float taskCurrentTime = 0f;
    public float taskMaxTime = 100f;
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

    public void DoTask()
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
            CompleteTask();
        }
    }

    protected virtual void CompleteTask()
    {
        Destroy(healthBarInstance.gameObject);
        taskCurrentTime = 0;
        owner.EndBuildingTask(this);   
    }
}
