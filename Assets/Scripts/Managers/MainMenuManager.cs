using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartAction()
    {
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    public void ExitAction()
    {
        Application.Quit();
    }
}
