using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour {

    public TextMesh text;
    public SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(string newText, Sprite newSprite)
    {
        text.text = newText;
        spriteRenderer.sprite = newSprite;
        this.transform.position += new Vector3(0, 0, -2);
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
