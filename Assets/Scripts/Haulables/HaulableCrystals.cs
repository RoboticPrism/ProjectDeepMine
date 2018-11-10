using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulableCrystals : HaulableBase {
    public override void Deposite()
    {
        ResourceManager.instance.AddEnergyCrystals(value, this.transform.position);
        base.Deposite();
    }
}
