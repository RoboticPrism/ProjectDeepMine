using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutOfResources : MonoBehaviour {

    [SerializeField]
    int blinkRate = 30;
    int currentBlink = 0;

    [Header("Instance Connections")]
    [SerializeField]
    SpriteRenderer resourceSpriteRenderer;
    [SerializeField]
    GameObject spriteObject;

	// Use this for initialization
	void Start () {
		
	}

    public void Setup(Sprite sprite)
    {
        resourceSpriteRenderer.sprite = sprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if(currentBlink >= blinkRate)
        {
            spriteObject.SetActive(!spriteObject.activeSelf);
            currentBlink = 0;
        } else
        {
            currentBlink += 1;
        }
    }
}
