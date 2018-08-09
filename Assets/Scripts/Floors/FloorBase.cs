using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBase : MonoBehaviour {

    public int resourceCount = 0;
    public int resourcesPerHaul = 5;
    public bool buildable = true;

    public enum resourceTypes { NONE, ORE };
    public resourceTypes resourceType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int MineResources()
    {
        if (resourceCount > resourcesPerHaul)
        {
            resourceCount -= resourcesPerHaul;
            return resourcesPerHaul;
        }
        else if (resourceCount > 0)
        {
            int remainingResources = resourcesPerHaul - resourceCount;
            resourceCount = 0;
            return remainingResources;
        } else
        {
            return 0;
        }
    }
}
