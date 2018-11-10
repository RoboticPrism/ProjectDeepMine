using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DepositBase : MonoBehaviour {
    [SerializeField]
    HaulableBase chunkPrefab;

    UnityAction<TaskableBase> destroyedListener;

    // Use this for initialization
    void Start () {
        destroyedListener = new UnityAction<TaskableBase>(CheckWallDestroyed);
        EventManager.StartListening("WallDestroyed", destroyedListener);
    }

    void CheckWallDestroyed(TaskableBase destroyedObject)
    {
        WallBase wall = destroyedObject.GetComponent<WallBase>();
        if(wall)
        {
            if( TilemapManager.instance.wallTilemap.WorldToCell(wall.transform.position) == 
                TilemapManager.instance.depositTilemap.WorldToCell(this.transform.position))
            {
                DestroySelf();
            }
        }
    }

    void DestroySelf()
    {
        Debug.Log("destroy");
        Instantiate(chunkPrefab, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
