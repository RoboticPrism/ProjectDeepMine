using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskActionBar : MonoBehaviour {

    public Image taskImage;
    public Text taskName;

    public Button prioritizeButton;
    public Button cancelButton;
    public TaskableBase taskable;

    public void Setup(TaskableBase taskable)
    {
        this.taskable = taskable;
        prioritizeButton.onClick.AddListener(() => PrioritizeAction());
        cancelButton.onClick.AddListener(() => CancelAction());

        RefreshUI();
    }

    public void PrioritizeAction()
    {
        MinerTask minerTask = taskable.currentTask.GetComponent<MinerTask>();
        if (minerTask)
        {
            MinerManager.instance.DoTaskNow(minerTask);
        }
        // Factory tasks can't be prioratized
    }

    public void CancelAction()
    {
        MinerTask minerTask = taskable.currentTask.GetComponent<MinerTask>();
        FactoryTask factoryTask = taskable.currentTask.GetComponent<FactoryTask>();
        if (minerTask)
        {
            MinerManager.instance.CancelTask(minerTask);
        }
        else if(factoryTask)
        {
            factoryTask.owner.GetComponent<FactoryBase>().CancelTask();
        }
    }

    public void RefreshUI()
    {
        if (taskable.currentTask)
        {
            taskImage.sprite = taskable.currentTask.spriteRenderer.sprite;
            taskName.text = taskable.currentTask.taskName;
            prioritizeButton.interactable = false;

            MinerTask minerTask = taskable.currentTask.GetComponent<MinerTask>();
            if (minerTask && !minerTask.owner)
            {
                prioritizeButton.interactable = true;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
