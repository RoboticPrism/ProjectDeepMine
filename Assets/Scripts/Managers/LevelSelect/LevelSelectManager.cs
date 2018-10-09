using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour {

    [HideInInspector]
    public static LevelSelectManager instance;

    [Header("Instance Connections")]
    public LevelPreview levelPreview;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PreviewLevel(Level level)
    {
        levelPreview.Setup(level);
        levelPreview.gameObject.SetActive(true);
    }
}
