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
        if (taskable.currentTask.GetComponent<MinerTask>())
        {
            MinerManager.instance.DoTaskNow(taskable.currentTask.GetComponent<MinerTask>());
        }
        //TODO: Case for buildingtask
    }

    public void CancelAction()
    {
        if (taskable.currentTask.GetComponent<MinerTask>())
        {
            MinerManager.instance.CancelTask(taskable.currentTask.GetComponent<MinerTask>());
        }
        //TODO: Case for buildingtask
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
