using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulableBlueGem : HaulableBase {
    public override void Deposite()
    {
        ResourceManager.instance.AddBlueGems(value, this.transform.position);
        base.Deposite();
    }
}
