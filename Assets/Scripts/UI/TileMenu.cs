using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TileMenu : MonoBehaviour {

    ClickableTileBase selectedTile;
    public TileMenuOption menuOptionPrefab;
    public Canvas canvas;
    public Transform optionArea;
    public Text title;
    public int panelWidth = 4;

    public List<Task> tasksToRender = new List<Task>();

    UnityAction<ClickableTileBase> updateListener;
    UnityAction<ClickableTileBase> destroyedListener;

    // Use this for initialization
    void Start () {
        updateListener = new UnityAction<ClickableTileBase>(RecreateMenu);
        destroyedListener = new UnityAction<ClickableTileBase>(DestroyMenu);
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

    public void CreateMenu(ClickableTileBase selectedTile)
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

    public void RecreateMenu(ClickableTileBase clickableTileBase)
    {   
        if (clickableTileBase.Equals(selectedTile))
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

    public void DestroyMenu(ClickableTileBase clickableTileBase)
    {
        if(clickableTileBase.Equals(selectedTile))
        {
            DestroySelf();
        }
    }

    void RenderOptions()
    {
        int i = 0;
        foreach (Task task in tasksToRender)
        {
            foreach (Task.priotities priority in System.Enum.GetValues(typeof(Task.priotities)))
            {
                if(priority == Task.priotities.QUEUE && task.CanQueue())
                {
                    TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
                    menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
                    menuOption.SetName(task.taskName);
                    menuOption.SetAction(AddTask, task);
                    i++;
                }
                else if (priority == Task.priotities.QUEUE_NOW && task.CanQueueNow())
                {
                    TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
                    menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
                    menuOption.SetName(task.taskName + " now");
                    menuOption.SetAction(AddTaskNow, task);
                    i++;
                }
                else if(priority == Task.priotities.REQUEUE_NOW && task.CanRequeueNow())
                {
                    TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
                    menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
                    menuOption.SetName("Prioritize " + task.taskName);
                    menuOption.SetAction(PrioritizeTask, task);
                    i++;
                }
                else if (priority == Task.priotities.CANCEL && task.CanCancel())
                {
                    TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
                    menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
                    menuOption.SetName("Cancel " + task.taskName);
                    menuOption.SetAction(CancelTask, task);
                    i++;
                }
            }
        }
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, 1 + (i * menuOptionPrefab.height));
    }

    void AddTask(Task task)
    {
        MinerManager.instance.AddTaskToQueue(task);
        DestroySelf();
    }

    void AddTaskNow(Task task)
    {
        MinerManager.instance.DoTaskNow(task);
        DestroySelf();
    }

    void PrioritizeTask(Task task)
    {
        MinerManager.instance.DoTaskNow(task);
        DestroySelf();
    }

    void CancelTask(Task task)
    {
        MinerManager.instance.CancelTask(task);
        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
