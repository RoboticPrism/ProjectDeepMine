using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A building capable of having building tasks
public class FactoryBase : BuildingBase {
    public List<FactoryTask> factoryTaskPrefabs;
}
