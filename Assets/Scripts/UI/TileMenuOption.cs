using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMenuOption : MonoBehaviour {

    public Text text;
    public Button button;
    public delegate void MinerAction(MinerTask task);
    public delegate void BuildingAction(BuildingTask task);
    public int height = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetName(string name)
    {
        this.text.text = name;
    }

    public void SetAction(MinerAction action, MinerTask task)
    {
        this.button.onClick.AddListener(delegate { action(task); });
    }

    public void SetAction(BuildingAction action, BuildingTask task)
    {
        this.button.onClick.AddListener(delegate { action(task); });
    }
}
