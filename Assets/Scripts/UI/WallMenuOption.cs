﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallMenuOption : MonoBehaviour {

    public Text text;
    public Button button;
    public delegate void Action();
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

    public void SetAction(Action action)
    {
        this.button.onClick.AddListener(delegate { action(); });
    }
}
