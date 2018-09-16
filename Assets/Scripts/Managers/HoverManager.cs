using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HoverManager : MonoBehaviour {

    [Header("Prefab Connections")]
    public TaskActionBar taskActionBarPrefab;
    TaskActionBar taskActionBarInstance;
    public BuildingActionBar buildingActionBarPrefab;
    BuildingActionBar buildingActionBarInstance;
    public WallActionBar wallActionBarPrefab;
    WallActionBar wallActionBarInstance;

    GameObject hoveredObject;
    TaskableBase selectedObject;

    [Header("UI Connections")]
    public Text itemName;
    public Text itemDescription;
    public Image itemImage;

    public Sprite unknownSprite;

    UnityAction<TaskableBase> updateListener;
    UnityAction<TaskableBase> destroyedListener;

    // Use this for initialization
    void Start()
    {
        updateListener = new UnityAction<TaskableBase>(RefreshMenus);
        destroyedListener = new UnityAction<TaskableBase>(DestroyMenus);
        EventManager.StartListening("BuildingCreated", updateListener);
        EventManager.StartListening("BuildingBuilt", updateListener);
        EventManager.StartListening("BuildingDeconstructing", updateListener);
        EventManager.StartListening("BuildingDamaged", updateListener);
        EventManager.StartListening("BuildingRepaired", updateListener);
        EventManager.StartListening("WallMining", updateListener);
        EventManager.StartListening("TaskCreated", updateListener);
        EventManager.StartListening("TaskStarted", updateListener);
        EventManager.StartListening("TaskDestroyed", updateListener);
        EventManager.StartListening("BuildingSold", destroyedListener);
        EventManager.StartListening("WallDestroyed", destroyedListener);
    }

    void RefreshMenus(TaskableBase taskable)
    {
        if (taskable == selectedObject)
        {
            if (buildingActionBarInstance)
            {
                buildingActionBarInstance.RefreshUI();
            }
            if (wallActionBarInstance)
            {
                wallActionBarInstance.RefreshUI();
            }
            if(selectedObject && selectedObject.currentTask)
            {
                if (taskActionBarInstance)
                {
                    taskActionBarInstance.RefreshUI();
                }
                else
                {
                    taskActionBarInstance = Instantiate(taskActionBarPrefab);
                    taskActionBarInstance.Setup(selectedObject);
                }
            }
            else
            {
                if(taskActionBarInstance)
                {
                    Destroy(taskActionBarInstance.gameObject);
                }
            }
        }
    }

    void DestroyMenus(TaskableBase taskable)
    {
        if (taskable == selectedObject)
        {
            DeselectTile();
        }
    }

    // Update is called once per frame
    void Update () {
        // Right click deselects current selection
        if(Input.GetMouseButtonDown(1) && selectedObject)
        {
            DeselectTile();
        }

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

    public void SelectTile(TaskableBase tile)
    {
        DeselectTile();

        selectedObject = tile;

        BuildingBase buildingObject = tile.GetComponent<BuildingBase>();
        WallBase wallObject = tile.GetComponent<WallBase>();
        if(buildingObject)
        {
            buildingActionBarInstance = Instantiate(buildingActionBarPrefab);
            buildingActionBarInstance.Setup(buildingObject);

            if(buildingObject.currentTask)
            {
                taskActionBarInstance = Instantiate(taskActionBarPrefab);
                taskActionBarInstance.Setup(buildingObject);
            }
        }
        else if (wallObject)
        {
            wallActionBarInstance = Instantiate(wallActionBarPrefab);
            wallActionBarInstance.Setup(wallObject);

            if (wallObject.currentTask)
            {
                taskActionBarInstance = Instantiate(taskActionBarPrefab);
                taskActionBarInstance.Setup(wallObject);
            }
        }
    }

    public void DeselectTile()
    {
        selectedObject = null;
        if(taskActionBarInstance)
        {
            Destroy(taskActionBarInstance.gameObject);
        }
        if(buildingActionBarInstance)
        {
            Destroy(buildingActionBarInstance.gameObject);
        }
        if(wallActionBarInstance)
        {
            Destroy(wallActionBarInstance.gameObject);
        }
    }

    private void UpdateDisplay()
    {
        if(selectedObject)
        {
            HoverInfo hoveredInfo = selectedObject.GetComponent<HoverInfo>();

            if (hoveredInfo && hoveredInfo.visible)
            {
                itemName.text = hoveredInfo.displayName;
                itemImage.sprite = hoveredInfo.sprite;
                itemDescription.text = hoveredInfo.description;
            }
            else
            {
                itemName.text = "Unknown";
                itemImage.sprite = unknownSprite;
                itemDescription.text = "This area has yet to be discovered.";
            }
        }
        else if (hoveredObject)
        {
            HoverInfo hoveredInfo = hoveredObject.GetComponent<HoverInfo>();

            if (hoveredInfo && hoveredInfo.visible)
            {
                itemName.text = hoveredInfo.displayName;
                itemImage.sprite = hoveredInfo.sprite;
                itemDescription.text = hoveredInfo.description;
            }
            else
            {
                itemName.text = "Unknown";
                itemImage.sprite = unknownSprite;
                itemDescription.text = "This area has yet to be discovered.";
            }
        }
    }
}
