using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulableOre : HaulableBase {
    public override void Deposite()
    {
        ResourceManager.instance.AddOre(value, this.transform.position);
        base.Deposite();
    }
}
