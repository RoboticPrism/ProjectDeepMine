using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    public string levelName;
    public string levelDescription;
    public Sprite levelPreview;
    public string sceneName;

    public List<ObjectiveBase> objectives;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BeginAction()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
