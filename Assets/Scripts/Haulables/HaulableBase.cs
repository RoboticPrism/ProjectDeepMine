using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulableBase : TaskableBase {
    [SerializeField]
    public HaulTask haulTaskPrefab;
    [SerializeField]
    protected int value = 5;

    public virtual void Deposite()
    {
        EventManager.TriggerEvent("HaulableDeposited", this);
        DestroySelf();
    }

	public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
