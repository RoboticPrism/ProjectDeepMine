using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryBase : MonoBehaviour {

    public RectTransform ObjectiveRow;
    public Text progressText;

	// Use this for initialization
	protected virtual void Start () {
        UpdateView();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void UpdateView()
    {

    }

    public virtual bool RequirementsMet()
    {
        return false;
    }
}
