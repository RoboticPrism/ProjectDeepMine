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

    public enum options
    {
        MINE,
        MINE_NOW,
        INCREASE_MINING_PRIORITY,
        BUILD,
        BUILD_NOW,
        INCREASE_BUILD_PRIORITY,
        REPAIR,
        REPAIR_NOW,
        INCREASE_REPAIR_PRIORITY,
        DECONSTRUCT,
        DECONSTRUCT_NOW,
        INCREASE_DECONSTRUCT_PRIORITY
    }

    public List<options> optionsToRender = new List<options>();

	// Use this for initialization
	void Start () {
        minerManager = FindObjectOfType<MinerManager>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && 
            !RectTransformUtility.RectangleContainsScreenPoint(
                optionArea.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            DestroySelf();
        }
    }

    public void CreateMenu(ClickableTileBase selectedTile)
    {
        this.selectedTile = selectedTile;
        this.title.text = selectedTile.displayName;
        MineableWall mineableWall = selectedTile.GetComponent<MineableWall>();
        BuildingBase buildingBase = selectedTile.GetComponent<BuildingBase>();
        if (mineableWall)
        {
            if (mineableWall.task == null)
            {
                optionsToRender.Add(options.MINE);
                optionsToRender.Add(options.MINE_NOW);
            }
            else
            {
                optionsToRender.Add(options.INCREASE_MINING_PRIORITY);
            }
            RenderOptions();
        }
        else if (buildingBase)
        {
            if (!buildingBase.built)
            {
                if(buildingBase.buildTask == null)
                {
                    optionsToRender.Add(options.BUILD);
                    optionsToRender.Add(options.BUILD_NOW);
                }
                else
                {
                    optionsToRender.Add(options.INCREASE_BUILD_PRIORITY);
                }
            }
            else
            {
                if (buildingBase.deconstructTask == null)
                {
                    optionsToRender.Add(options.DECONSTRUCT);
                    optionsToRender.Add(options.DECONSTRUCT_NOW);
                }
                else
                {
                    optionsToRender.Add(options.INCREASE_DECONSTRUCT_PRIORITY);
                }
            }
            if(buildingBase.life < buildingBase.lifeMax)
            {
                if (buildingBase.repairTask == null)
                {
                    optionsToRender.Add(options.REPAIR);
                    optionsToRender.Add(options.REPAIR_NOW);
                }
                else
                {
                    optionsToRender.Add(options.INCREASE_REPAIR_PRIORITY);
                }
            }
            RenderOptions();
        }
        else
        {
            DestroySelf();
        }
    }

    void RenderOptions()
    {
        int i = 0;
        foreach (options option in optionsToRender)
        {
            TileMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
            menuOption.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-i * menuOption.height) - 1.5f);
            if (option == options.MINE)
            {
                menuOption.SetName("Mine");
                menuOption.SetAction(MineAction);
            }
            else if (option == options.MINE_NOW)
            {
                menuOption.SetName("Mine Now");
                menuOption.SetAction(MineNowAction);
            }
            else if (option == options.INCREASE_MINING_PRIORITY)
            {
                menuOption.SetName("Increase Priority");
                menuOption.SetAction(IncreaseMiningPriorityAction);
            }
            else if (option == options.BUILD)
            {
                menuOption.SetName("Build");
                menuOption.SetAction(BuildAction);
            }
            else if (option == options.BUILD_NOW)
            {
                menuOption.SetName("Build Now");
                menuOption.SetAction(BuildNowAction);
            }
            else if (option == options.INCREASE_BUILD_PRIORITY)
            {
                menuOption.SetName("Increase Build Priority");
                menuOption.SetAction(IncreaseBuildPriorityAction);
            }
            else if (option == options.REPAIR)
            {
                menuOption.SetName("Repair");
                menuOption.SetAction(RepairAction);
            }
            else if (option == options.REPAIR_NOW)
            {
                menuOption.SetName("Repair Now");
                menuOption.SetAction(RepairNowAction);
            }
            else if (option == options.INCREASE_REPAIR_PRIORITY)
            {
                menuOption.SetName("Increase Repair Priority");
                menuOption.SetAction(IncreaseRepairPriorityAction);
            }
            else if (option == options.DECONSTRUCT)
            {
                menuOption.SetName("Deconstruct");
                menuOption.SetAction(DeconstructAction);
            }
            else if (option == options.DECONSTRUCT_NOW)
            {
                menuOption.SetName("Deconstruct Now");
                menuOption.SetAction(DeconstructNowAction);
            }
            else if (option == options.INCREASE_DECONSTRUCT_PRIORITY)
            {
                menuOption.SetName("Increase Decostruction Priority");
                menuOption.SetAction(IncreaseDeconstructPriorityAction);
            }
            i++;
        }
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, 1 + (i * menuOptionPrefab.height));
    }

    void MineAction()
    {
        minerManager.AddWallToQueue(selectedTile.GetComponent<MineableWall>());
        DestroySelf();
    }

    void MineNowAction()
    {
        minerManager.AddWallToQueueNow(selectedTile.GetComponent<MineableWall>());
        DestroySelf();
    }

    void IncreaseMiningPriorityAction()
    {

        DestroySelf();
    }

    void BuildAction()
    {
        minerManager.AddBuildingToQueue(selectedTile.GetComponent<BuildingBase>());
        DestroySelf();
    }

    void BuildNowAction()
    {
        minerManager.AddBuildingToQueueNow(selectedTile.GetComponent<BuildingBase>());
        DestroySelf();
    }

    void IncreaseBuildPriorityAction()
    {

        DestroySelf();
    }

    void RepairAction()
    {
        minerManager.AddBuildingRepairToQueue(selectedTile.GetComponent<BuildingBase>());
        DestroySelf();
    }

    void RepairNowAction()
    {
        minerManager.AddBuildingRepairToQueueNow(selectedTile.GetComponent<BuildingBase>());
        DestroySelf();
    }

    void IncreaseRepairPriorityAction()
    {

        DestroySelf();
    }

    void DeconstructAction()
    {
        minerManager.AddBuildingDeconstructToQueue(selectedTile.GetComponent<BuildingBase>());
        DestroySelf();
    }

    void DeconstructNowAction()
    {
        minerManager.AddBuildingDeconstructToQueueNow(selectedTile.GetComponent<BuildingBase>());
        DestroySelf();
    }

    void IncreaseDeconstructPriorityAction()
    {

        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
