using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBarButton : MonoBehaviour {

    [Header("Instance Connections")]
    public Image image;
    public Text text;
    public Button button;

    public delegate void Action();

	public void Setup(Task task, Action action)
    {
        text.text = task.taskName;
        text.enabled = true;
        image.sprite = task.spriteRenderer.sprite;
        image.enabled = true;
        button.onClick.AddListener(() => action());
    }

    public void SetInteractible(bool state)
    {
        button.interactable = state;
    }
}
