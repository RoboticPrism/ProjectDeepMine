using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMenu : MonoBehaviour {

    ClickableTileBase selectedTile;
    public TileMenuOption menuOptionPrefab;
    MinerManager minerManager;
    public Canvas canvas;
    public Transform optionArea;
    public Text title;
    public int panelWidth = 4;

    public List<Task> tasksToRender = new List<Task>();

	// Use this for initialization
	void Start () {
        minerManager = FindObjectOfType<MinerManager>();
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
            TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
            menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
            menuOption.SetName(task.taskName);
            menuOption.SetAction(AddTask, task);
            i++;
        }
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, 1 + (i * menuOptionPrefab.height));
    }

    void AddTask(Task task)
    {
        minerManager.AddTaskToQueue(task);
        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
