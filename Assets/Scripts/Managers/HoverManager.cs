using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverManager : MonoBehaviour {

    GameObject hoveredObject;

    public Text itemName;
    public Text itemDescription;
    public Image itemImage;

    public Sprite unknownSprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D raycastHit = Physics2D.Raycast(worldPoint, Vector2.zero, LayerMask.GetMask("Hoverable"));
        if(raycastHit.transform && raycastHit.transform.gameObject && raycastHit.transform.gameObject.GetComponent<HoverInfo>())
        {
            hoveredObject = raycastHit.transform.gameObject;
        }
        else
        {
            worldPoint = new Vector3(Mathf.Floor(worldPoint.x) + 0.5f, Mathf.Floor(worldPoint.y) + 0.5f, -1);

            Vector3Int wallPosition = TilemapManager.instance.wallTilemap.WorldToCell(worldPoint);
            GameObject wall = TilemapManager.instance.wallTilemap.GetInstantiatedObject(wallPosition);

            Vector3Int floorPosition = TilemapManager.instance.floorTilemap.WorldToCell(worldPoint);
            GameObject floor = TilemapManager.instance.floorTilemap.GetInstantiatedObject(floorPosition);

            if (wall)
            {
                hoveredObject = wall;
            } else if(floor)
            {
                hoveredObject = floor;
            } else
            {
                hoveredObject = null;
            }
        }
        UpdateDisplay();
	}

    private void UpdateDisplay()
    {
        HoverInfo hoveredInfo = hoveredObject.GetComponent<HoverInfo>();

        if (hoveredInfo && hoveredInfo.visible)
        {
            itemName.text = hoveredInfo.displayName;
            itemImage.sprite = hoveredInfo.sprite;
            itemDescription.text = hoveredInfo.description;
        } else {
            itemName.text = "Unknown";
            itemImage.sprite = unknownSprite;
            itemDescription.text = "This area has yet to be discovered.";
        }
    }
}
