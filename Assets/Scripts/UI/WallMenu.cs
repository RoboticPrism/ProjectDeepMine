using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallMenu : MonoBehaviour {

    WallBase selectedWall;
    public WallMenuOption menuOptionPrefab;
    MinerManager minerManager;
    public Canvas canvas;
    public Transform optionArea;
    public Text title;
    public int panelWidth = 4;

    public enum options
    {
        MINE,
        MINE_NOW,
        INCREASE_PRIORITY
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

    public void CreateMenu(WallBase selectedWall)
    {
        this.selectedWall = selectedWall;
        this.title.text = selectedWall.displayName;
        MineableWall mineableWall = selectedWall.GetComponent<MineableWall>();
        if (mineableWall)
        {
            if (mineableWall.task == null)
            {
                optionsToRender.Add(options.MINE);
                optionsToRender.Add(options.MINE_NOW);
            }
            else
            {
                optionsToRender.Add(options.INCREASE_PRIORITY);
            }
            RenderOptions();
        } else
        {
            DestroySelf();
        }
    }

    void RenderOptions()
    {
        int i = 0;
        foreach (options option in optionsToRender)
        {
            WallMenuOption menuOption = Instantiate(menuOptionPrefab, optionArea);
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
            else if (option == options.INCREASE_PRIORITY)
            {
                menuOption.SetName("Increase Priority");
                menuOption.SetAction(IncreasePriorityAction);
            }
            i++;
        }
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, 1 + (i * menuOptionPrefab.height));
    }

    void MineAction()
    {
        minerManager.AddWallToQueue(selectedWall.GetComponent<MineableWall>());
        DestroySelf();
    }

    void MineNowAction()
    {
        minerManager.AddWallToQueueNow(selectedWall.GetComponent<MineableWall>());
        DestroySelf();
    }

    void IncreasePriorityAction()
    {

        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
