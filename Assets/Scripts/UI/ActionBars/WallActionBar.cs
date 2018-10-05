using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallActionBar : MonoBehaviour {

    public Button mineButton;
    WallBase wall;


    public void Setup(WallBase wall)
    {
        this.wall = wall;
        mineButton.onClick.AddListener(() => MineAction());

        RefreshUI();
    }

    public void MineAction()
    {
        wall.CreateTask(wall.mineTaskPrefab);
    }

    public void RefreshUI()
    {
        mineButton.interactable = false;
        if (!wall.currentTask && wall.mineTaskPrefab)
        {
            mineButton.interactable = true;
        }
    }
}
