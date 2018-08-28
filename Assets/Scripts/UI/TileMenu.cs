using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMenu : MonoBehaviour {

    ClickableTileBase selectedTile;
    public TileMenuOption menuOptionPrefab;
    public Canvas canvas;
    public Transform optionArea;
    public Text title;
    public int panelWidth = 4;

    public List<Task> tasksToRender = new List<Task>();

	// Use this for initialization
	void Start () {
        
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
