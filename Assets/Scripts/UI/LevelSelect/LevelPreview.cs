using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPreview : MonoBehaviour {

    [Header("Prefab Connections")]
    public ObjectivePreview objectivePreviewPrefab;

    [Header("Instance Connections")]
    public Text title;
    public Image previewImage;
    public RectTransform mainObjectivesArea;
    public RectTransform bonusObjectivesArea;
    public Button beginButton;
    public Button cancelButton;

	public void Setup(Level level)
    {
        title.text = level.levelName;
        previewImage.sprite = level.levelPreview;

        foreach(Transform previousObjective in mainObjectivesArea.GetComponentInChildren<Transform>())
        {
            Destroy(previousObjective.gameObject);
        }
        foreach(ObjectiveBase objective in level.objectives)
        {
            ObjectivePreview op = Instantiate(objectivePreviewPrefab, mainObjectivesArea.transform);
            op.Setup(objective);
        }

        // TODO: setup bonus objectives too

        beginButton.onClick.RemoveAllListeners();
        beginButton.onClick.AddListener(() => level.BeginAction());
        this.gameObject.SetActive(true);
    }

    public void CancelAction()
    {
        this.gameObject.SetActive(false);
    }
}
