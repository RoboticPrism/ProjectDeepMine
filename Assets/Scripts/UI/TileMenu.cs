using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TileMenu : MonoBehaviour {

    TaskableBase selectedTile;
    public TileMenuOption menuOptionPrefab;
    public Canvas canvas;
    public Transform optionArea;
    public Text title;
    public int panelWidth = 4;

    public List<Task> tasksToRender = new List<Task>();

    UnityAction<TaskableBase> updateListener;
    UnityAction<TaskableBase> destroyedListener;

    // Use this for initialization
    void Start () {
        updateListener = new UnityAction<TaskableBase>(RecreateMenu);
        destroyedListener = new UnityAction<TaskableBase>(DestroyMenu);
        EventManager.StartListening("BuildingCreated", updateListener);
        EventManager.StartListening("BuildingBuilt", updateListener);
        EventManager.StartListening("BuildingDeconstructing", updateListener);
        EventManager.StartListening("BuildingDamaged", updateListener);
        EventManager.StartListening("BuildingRepaired", updateListener);
        EventManager.StartListening("WallMining", updateListener);
        EventManager.StartListening("BuildingSold", destroyedListener);
        EventManager.StartListening("WallDestroyed", destroyedListener);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            !RectTransformUtility.RectangleContainsScreenPoint(
                optionArea.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            DestroySelf();
        } else if (Input.GetMouseButtonDown(1))
        {
            DestroySelf();
        }
    }

    public void CreateMenu(TaskableBase selectedTile)
    {
        this.selectedTile = selectedTile;
        this.title.text = selectedTile.GetComponent<HoverInfo>().displayName;
        MineableWall mineableWall = selectedTile.GetComponent<MineableWall>();
        BuildingBase buildingBase = selectedTile.GetComponent<BuildingBase>();

        foreach (Task task in selectedTile.potentialTasks)
        {
            if (task.TaskAvailable())
            {
                tasksToRender.Add(task);
            }
        }
        // Don't render a menu with no options
        if (tasksToRender.Count == 0)
        {
            DestroySelf();
        }
        else
        {
            RenderOptions();
        }
    }

    public void RecreateMenu(TaskableBase TaskableBase)
    {   
        if (TaskableBase.Equals(selectedTile))
        {
            foreach (TileMenuOption option in optionArea.GetComponentsInChildren<TileMenuOption>())
            {
                Destroy(option.gameObject);
            }
            foreach (Task task in selectedTile.potentialTasks)
            {
                if (task.TaskAvailable())
                {
                    tasksToRender.Add(task);
                }
            }
            // Don't render a menu with no options
            if (tasksToRender.Count == 0)
            {
                DestroySelf();
            }
            else
            {
                RenderOptions();
            }
        }
    }

    public void DestroyMenu(TaskableBase TaskableBase)
    {
        if(TaskableBase.Equals(selectedTile))
        {
            DestroySelf();
        }
    }

    void RenderOptions()
    {
        int i = 0;
        foreach (Task task in tasksToRender)
        {
            MinerTask minerTask = task.GetComponent<MinerTask>();
            BuildingTask buildingTask = task.GetComponent<BuildingTask>();
            if (minerTask)
            {
                foreach (MinerTask.priotities priority in System.Enum.GetValues(typeof(MinerTask.priotities)))
                {
                    if (priority == MinerTask.priotities.QUEUE && minerTask.CanQueue())
                    {
                        TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
                        menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
                        menuOption.SetName(minerTask.taskName);
                        menuOption.SetAction(AddMinerTask, minerTask);
                        i++;
                    }
                    else if (priority == MinerTask.priotities.QUEUE_NOW && minerTask.CanQueueNow())
                    {
                        TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
                        menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
                        menuOption.SetName(minerTask.taskName + " now");
                        menuOption.SetAction(AddMinerTaskNow, minerTask);
                        i++;
                    }
                    else if (priority == MinerTask.priotities.REQUEUE_NOW && minerTask.CanRequeueNow())
                    {
                        TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
                        menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
                        menuOption.SetName("Prioritize " + minerTask.taskName);
                        menuOption.SetAction(PrioritizeMinerTask, minerTask);
                        i++;
                    }
                    else if (priority == MinerTask.priotities.CANCEL && minerTask.CanCancel())
                    {
                        TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
                        menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
                        menuOption.SetName("Cancel " + minerTask.taskName);
                        menuOption.SetAction(CancelMinerTask, minerTask);
                        i++;
                    }
                }
            }
            else if (buildingTask)
            {
                TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
                menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
                menuOption.SetName(buildingTask.taskName);
                menuOption.SetAction(AddBuildingTask, buildingTask);
                i++;
            }
            
        }
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, 1 + (i * menuOptionPrefab.height));
    }

    void AddMinerTask(MinerTask task)
    {
        MinerManager.instance.AddTaskToQueue(task);
        DestroySelf();
    }

    void AddMinerTaskNow(MinerTask task)
    {
        MinerManager.instance.DoTaskNow(task);
        DestroySelf();
    }

    void PrioritizeMinerTask(MinerTask task)
    {
        MinerManager.instance.DoTaskNow(task);
        DestroySelf();
    }

    void CancelMinerTask(MinerTask task)
    {
        MinerManager.instance.CancelTask(task);
        DestroySelf();
    }

    void AddBuildingTask(BuildingTask task)
    {
        task.owner.AddBuildingTask(task);
        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
