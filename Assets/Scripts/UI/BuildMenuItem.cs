using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuItem : MonoBehaviour {

    public Text text;
    public Image image;
    public Button button;
    public delegate void Action(BuildingBase buildingBase);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetupItem(BuildingBase buildingBase, Action action)
    {
        this.text.text = buildingBase.displayName;
        this.image.sprite = buildingBase.GetComponent<SpriteRenderer>().sprite;
        this.button.onClick.AddListener(delegate { action(buildingBase); });
    }
}
