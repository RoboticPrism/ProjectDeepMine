using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ObjectiveStatus : MonoBehaviour {

    protected ObjectiveBase objective;

    [Header("Instance Connections")]
    public Text progressText;

    protected UnityAction updateListener;

    public virtual void Setup(ObjectiveBase newObjective)
    {
        objective = newObjective;
        updateListener = new UnityAction(Refresh);
    }

    void Refresh()
    {
        UpdateView();
        VictoryManager.instance.CheckVictory();
    }

    public virtual void UpdateView()
    {

    }

    public virtual bool RequirementsMet()
    {
        return false;
    }
}
