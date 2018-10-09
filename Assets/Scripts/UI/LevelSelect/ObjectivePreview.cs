using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivePreview : MonoBehaviour {

    public Text text;
    public Image box;
    public Image check;

    public void Setup(ObjectiveBase objective)
    {
        text.text = objective.objectiveTitle;

        // TODO: show check when objective complete
    }
}
