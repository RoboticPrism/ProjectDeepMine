using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    
    public RectTransform bar;

    public void UpdateBar(float percent)
    {
        bar.anchorMax = new Vector2(percent, bar.anchorMax.y);
    }

    public void UpdateColor(Color color)
    {
        bar.GetComponentInChildren<Image>().color = color;
    }
}
