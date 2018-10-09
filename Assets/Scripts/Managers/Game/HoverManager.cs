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
    public FactoryActionBar factoryActionBarPrefab;
    FactoryActionBar factoryActionBarInstance;
    public HaulableActionBar haulableActionBarPrefab;
    HaulableActionBar haulableActionBarInstance;

    [Header("Instance Connections")]
    public GameObject selectedIcon;

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
        EventManager.StartListening("HaulableDeposited", destroyedListener);
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

    // Called on events that should refresh the menu
    void RefreshMenus(TaskableBase taskable)
    {
        if (taskable && taskable == selectedObject)
        {
            if(taskable.currentTask)
            {
                // Create or update task action bar
                if (taskActionBarInstance)
                {
                    taskActionBarInstance.RefreshUI();
                }
                else
                {
                    taskActionBarInstance = Instantiate(taskActionBarPrefab, transform);
                    taskActionBarInstance.Setup(selectedObject);
                }

                // clean up building and wall action bars
                if (buildingActionBarInstance)
                {
                    Destroy(buildingActionBarInstance.gameObject);
                }
                if (wallActionBarInstance)
                {
                    Destroy(wallActionBarInstance.gameObject);
                }
                if (factoryActionBarInstance)
                {
                    Destroy(factoryActionBarInstance.gameObject);
                }
                if (haulableActionBarInstance)
                {
                    Destroy(haulableActionBarInstance.gameObject);
                }
            }
            else
            {
                // Clean up task action bar
                if (taskActionBarInstance)
                {
                    Destroy(taskActionBarInstance.gameObject);
                }

                // refresh building and wall action bars
                BuildingBase buildingObject = taskable.GetComponent<BuildingBase>();
                WallBase wallObject = taskable.GetComponent<WallBase>();
                HaulableBase haulObject = taskable.GetComponent<HaulableBase>();
                if (buildingObject)
                {
                    if (buildingActionBarInstance)
                    {
                        buildingActionBarInstance.RefreshUI();
                    }
                    else
                    {
                        buildingActionBarInstance = Instantiate(buildingActionBarPrefab, transform);
                        buildingActionBarInstance.Setup(buildingObject);
                    }
                    FactoryBase factoryObject = buildingObject.GetComponent<FactoryBase>();
                    if (factoryObject)
                    {
                        if (factoryActionBarInstance)
                        {
                            factoryActionBarInstance.RefreshUI();
                        }
                        else
                        {
                            factoryActionBarInstance = Instantiate(factoryActionBarPrefab, transform);
                            factoryActionBarInstance.Setup(factoryObject);
                        }
                    }
                }
                else if (wallObject) {
                    if (wallActionBarInstance)
                    {
                        wallActionBarInstance.RefreshUI();
                    }
                    else
                    {
                        wallActionBarInstance = Instantiate(wallActionBarPrefab, transform);
                        wallActionBarInstance.Setup(wallObject);
                    }
                }
                else if (haulObject)
                {
                    if(haulableActionBarInstance)
                    {
                        haulableActionBarInstance.RefreshUI();
                    }
                    else
                    {
                        haulableActionBarInstance = Instantiate(haulableActionBarPrefab, transform);
                        haulableActionBarInstance.Setup(haulObject);
                    }
                }
            }
        }
    }

    // Called on events that should destroy the menu
    void DestroyMenus(TaskableBase taskable)
    {
        if (taskable == selectedObject)
        {
            DeselectTile();
        }
    }

    public void SelectTile(TaskableBase tile)
    {
        DeselectTile();

        selectedObject = tile;
        selectedIcon.SetActive(true);
        selectedIcon.transform.parent = selectedObject.transform;
        selectedIcon.transform.position = selectedObject.transform.position;

        BuildingBase buildingObject = tile.GetComponent<BuildingBase>();
        WallBase wallObject = tile.GetComponent<WallBase>();
        HaulableBase haulObject = tile.GetComponent<HaulableBase>();
        if (buildingObject)
        {
            if(buildingObject.currentTask)
            {
                taskActionBarInstance = Instantiate(taskActionBarPrefab, transform);
                taskActionBarInstance.Setup(buildingObject);
            }
            else
            {
                buildingActionBarInstance = Instantiate(buildingActionBarPrefab, transform);
                buildingActionBarInstance.Setup(buildingObject);

                FactoryBase factoryObject = buildingObject.GetComponent<FactoryBase>();
                if (factoryObject)
                {
                    factoryActionBarInstance = Instantiate(factoryActionBarPrefab, transform);
                    factoryActionBarInstance.Setup(factoryObject);
                }
            }
        }
        else if (wallObject)
        {
            if (wallObject.currentTask)
            {
                taskActionBarInstance = Instantiate(taskActionBarPrefab, transform);
                taskActionBarInstance.Setup(wallObject);
            }
            else
            {
                wallActionBarInstance = Instantiate(wallActionBarPrefab, transform);
                wallActionBarInstance.Setup(wallObject);
            }
        }
        else if (haulObject)
        {
            if (haulObject.currentTask)
            {
                taskActionBarInstance = Instantiate(taskActionBarPrefab, transform);
                taskActionBarInstance.Setup(haulObject);
            }
            else
            {
                haulableActionBarInstance = Instantiate(haulableActionBarPrefab, transform);
                haulableActionBarInstance.Setup(haulObject);
            }
        }
    }

    public void DeselectTile()
    {
        selectedObject = null;
        selectedIcon.transform.parent = null;
        selectedIcon.SetActive(false);
        if (taskActionBarInstance)
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
        if(factoryActionBarInstance)
        {
            Destroy(factoryActionBarInstance.gameObject);
        }
        if (haulableActionBarInstance)
        {
            Destroy(haulableActionBarInstance.gameObject);
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
