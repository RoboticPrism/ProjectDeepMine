using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HaulableActionBar : MonoBehaviour {

    public Button haulButton;
    HaulableBase haulable;

    public void Setup(HaulableBase haulable)
    {
        this.haulable = haulable;
        haulButton.onClick.AddListener(() => HaulAction());

        RefreshUI();
    }

    public void HaulAction()
    {
        haulable.CreateTask(haulable.haulTaskPrefab);
    }

    public void RefreshUI()
    {
        haulButton.interactable = false;
        if (!haulable.currentTask)
        {
            if (haulable.haulTaskPrefab && HaulTask.TaskAvailable(haulable))
            {
                haulButton.interactable = true;
            }
        }
    }
}
