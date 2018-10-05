using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulableBase : TaskableBase {

    public HaulTask haulTaskPrefab;
    public int value = 5;

    public void Deposite()
    {
        ResourceManager.instance.AddBlueGems(value, this.transform.position);
        EventManager.TriggerEvent("HaulableDeposited", this);
        DestroySelf();
    }

	public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
