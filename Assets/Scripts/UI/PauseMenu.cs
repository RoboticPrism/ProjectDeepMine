using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [Header("Instance Connections")]
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject retryConfirmPanel;
    [SerializeField]
    GameObject quitConfirmPanel;

    bool paused = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Menu")) {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if(paused)
        {
            menu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        paused = !paused;
        
    }

    public void ResumeAction()
    {
        TogglePause();
    }

    public void OptionsAction()
    {
        //TODO: add options here
    }

    public void RetryAction()
    {
        menu.SetActive(false);
        retryConfirmPanel.SetActive(true);
    }

    public void RetryConfirmAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RetryCancelAction()
    {
        menu.SetActive(true);
        retryConfirmPanel.SetActive(false);
    }

    public void QuitAction()
    {
        menu.SetActive(false);
        quitConfirmPanel.SetActive(true);
    }

    public void QuitConfirmAction()
    {
        //TODO: add save state here
        SceneManager.LoadSceneAsync("LevelSelect");
    }

    public void QuitCancelAction()
    {
        menu.SetActive(true);
        quitConfirmPanel.SetActive(false);
    }
}
