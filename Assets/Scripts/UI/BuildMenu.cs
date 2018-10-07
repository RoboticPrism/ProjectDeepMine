using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour {

    public bool open = false;
    public float openSpeed = 5f;

    [Header("Prefab Connections")]
    public BuildMenuItem menuItemPrefab;

    [Header("Instance Connections")]
    public RectTransform scrollZone;
    public Button openButton;

    [Header("Buildables")]
    public List<BuildingBase> buildings;

    Coroutine currentCoroutine = null;
    
    // Use this for initialization
    void Start () {
        CreateMenuItems();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // creates a new menu item
    private BuildMenuItem CreateMenuItem(BuildingBase buildingBase)
    {
        BuildMenuItem item = Instantiate(menuItemPrefab, scrollZone);
        item.SetupItem(buildingBase, BuildManager.instance.SelectBuild);
        return item;
    }

    // Populates menu items into the scroll zone
    private void CreateMenuItems()
    {
        int i = 0;
        foreach (BuildingBase buildingBase in buildings)
        {
            RectTransform rt = CreateMenuItem(buildingBase).GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -90 - (i * 170));
            i++;
        }
        scrollZone.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20 + (i * 170));
    }

    public void OpenMenu()
    {
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(OpenMenuCoroutine());
        }
    }

    private IEnumerator OpenMenuCoroutine()
    {
        open = true;
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 target = new Vector3(90, rect.position.y, rect.position.z);
        while (rect.position.x < 89)
        {
            rect.position = Vector3.Lerp(rect.position, target, openSpeed);
            yield return null;
        }
        rect.position = target;
        currentCoroutine = null;
    }

    public void CloseMenu()
    {
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(CloseMenuCoroutine());
        }
    }

    private IEnumerator CloseMenuCoroutine()
    {
        open = false;
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 target = new Vector3(-rect.sizeDelta.x * 2 - 90, rect.position.y, rect.position.z);
        while (rect.position.x > -rect.sizeDelta.x * 2 + 1 - 90)
        {
            rect.position = Vector3.Lerp(rect.position, target, openSpeed);
            yield return null;
        }
        rect.position = target;
        currentCoroutine = null;
    }
}
