using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBase : MonoBehaviour {

    public int resourceCount = 0;
    public bool buildable = true;

    public enum resourceTypes { NONE, ORE };
    public resourceTypes resourceType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
