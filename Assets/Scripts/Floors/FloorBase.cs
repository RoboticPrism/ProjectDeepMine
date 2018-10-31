using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorBase : MonoBehaviour {
    public int resourceCount = 0;
    public int resourcesPerHaul = 5;
    public bool buildable = true;

    public enum resourceTypes { NONE, ORE, ENERGY_CRYSTAL };
    public resourceTypes resourceType;

    [SerializeField]
    public Sprite resourceIcon;
    [SerializeField]
    Sprite outOfResourcesSprite;
    [SerializeField]
    SpriteRenderer spriteRenderer;

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
            spriteRenderer.sprite = outOfResourcesSprite;
            return remainingResources;
        } else
        {
            spriteRenderer.sprite = outOfResourcesSprite;
            return 0;
        }
    }

    public bool HasResources()
    {
        return resourceCount > 0;
    }
}
