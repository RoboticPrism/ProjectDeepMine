using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGemObjectiveStatus : ObjectiveStatus {

    BlueGemObjective blueGemObjective;

    public override void Setup(ObjectiveBase newObjective)
    {
        base.Setup(newObjective);
        blueGemObjective = objective.GetComponent<BlueGemObjective>();
        EventManager.StartListening("BlueGemsChanged", updateListener);
        UpdateView();
    }

    public override void UpdateView()
    {
        progressText.text = ResourceManager.instance.blueGemsCount + " / " + blueGemObjective.requireBlueGems;
    }

    public override bool RequirementsMet()
    {
        return ResourceManager.instance.blueGemsCount >= blueGemObjective.requireBlueGems;
    }
}
