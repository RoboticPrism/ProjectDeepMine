using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesHoverInfo : HoverInfo {
    [SerializeField]
    public GameObject resourceHolder;

    public Sprite GetResourceIcon()
    {
        FloorBase floor = resourceHolder.GetComponent<FloorBase>();
        MiningDrill miningDrill = resourceHolder.GetComponent<MiningDrill>();
        if(floor)
        {
            return floor.resourceIcon;
        }
        return null;
    }

    public int GetResourceCount()
    {
        FloorBase floor = resourceHolder.GetComponent<FloorBase>();
        MiningDrill miningDrill = resourceHolder.GetComponent<MiningDrill>();
        if (floor)
        {
            return floor.resourceCount;
        }
        return 0;
    }
}
