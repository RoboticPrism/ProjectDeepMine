using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour {

    public float moveSpeed = 1f;

    public Vector2 topRight;
    public Vector2 bottomLeft;
    public int borderPadding = 0;

    UnityAction<ClickableTileBase> wallDestroyedListener;

    // Use this for initialization
    void Start () {
        wallDestroyedListener = new UnityAction<ClickableTileBase>(SetBounds);
        EventManager.StartListening("WallDestroyed", wallDestroyedListener);
	}
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + x, bottomLeft.x - borderPadding, topRight.x + borderPadding), 
            Mathf.Clamp(transform.position.y + y, bottomLeft.y - borderPadding, topRight.y + borderPadding), 
            transform.position.z
        );
	}

    // Extends the camera bounds to include the given location
    void SetBounds(ClickableTileBase tileBase)
    {
        Vector3 location = tileBase.transform.position;
        if(location.x > topRight.x)
        {
            topRight = new Vector2(location.x, topRight.x);
        }
        if(location.y > topRight.y)
        {
            topRight = new Vector2(topRight.x, location.y);
        }
        if(location.x < bottomLeft.x)
        {
            bottomLeft = new Vector2(location.x, bottomLeft.y);
        }
        if (location.y < bottomLeft.y)
        {
            bottomLeft = new Vector2(bottomLeft.x, location.y);
        }
    }
}
