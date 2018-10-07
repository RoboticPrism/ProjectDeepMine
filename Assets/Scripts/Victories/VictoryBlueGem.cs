using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryBlueGem : VictoryBase {

    public int requireBlueGems;

    UnityAction updateListener;

    protected override void Start()
    {
        base.Start();
        updateListener = new UnityAction(Refresh);
        EventManager.StartListening("BlueGemsChanged", updateListener);
    }

    public void Refresh()
    {
        UpdateView();
        VictoryManager.instance.CheckVictory();
    }

    public override void UpdateView()
    {
        progressText.text = ResourceManager.instance.blueGemsCount + " / " + requireBlueGems;
    }

    public override bool RequirementsMet()
    {
        return ResourceManager.instance.blueGemsCount >= requireBlueGems;
    }
}
