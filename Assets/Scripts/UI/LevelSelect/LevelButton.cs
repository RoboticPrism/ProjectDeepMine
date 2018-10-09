using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public Button button;
    public Level level;

    private void Start()
    {
        button.onClick.AddListener(ClickAction);
    }

    public void ClickAction()
    {
        LevelSelectManager.instance.PreviewLevel(level);
    }
}
