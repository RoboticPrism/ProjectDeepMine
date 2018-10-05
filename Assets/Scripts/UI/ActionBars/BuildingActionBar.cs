using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingActionBar : MonoBehaviour {

    public Button buildButton;
    public Button repairButton;
    public Button sellButton;
    BuildingBase building;

	public void Setup(BuildingBase building)
    {
        this.building = building;
        buildButton.onClick.AddListener(() => BuildAction());
        repairButton.onClick.AddListener(() => RepairAction());
        sellButton.onClick.AddListener(() => DeconstructAction());

        RefreshUI();
    }

    public void BuildAction()
    {
        building.CreateTask(building.buildTaskPrefab);
    }

    public void RepairAction()
    {
        building.CreateTask(building.repairTaskPrefab);
    }

    public void DeconstructAction()
    {
        building.CreateTask(building.deconstructTaskPrefab);
    }

    public void RefreshUI ()
    {
        buildButton.interactable = false;
        repairButton.interactable = false;
        sellButton.interactable = false;
        if (!building.currentTask)
        {
            if (building.buildTaskPrefab && BuildTask.TaskAvailable(building))
            {
                buildButton.interactable = true;
            }
            if(building.repairTaskPrefab && RepairTask.TaskAvailable(building))
            {
                repairButton.interactable = true;
            }
            if(building.deconstructTaskPrefab && DeconstructTask.TaskAvailable(building))
            {
                sellButton.interactable = true;
            }
        }
    }
}
