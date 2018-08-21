using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : FloorBase {

	// Use this for initialization
	void Start () {
		if(!CheckIfCovered())
        {
            EnemyManager.instance.spawnPoints.Add(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool CheckIfCovered()
    {
        Vector3Int location = TilemapManager.instance.wallTilemap.WorldToCell(this.transform.position);
        return TilemapManager.instance.wallTilemap.GetInstantiatedObject(location) != null;
    }
}
