using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulableBase : TaskableBase {
    [SerializeField]
    protected int value = 5;
    [Header("Prefab Connections")]
    [SerializeField]
    public HaulTask haulTaskPrefab;
    [Header("Instance Connections")]
    [SerializeField]
    SpriteRenderer spriteRenderer;

    public void Pickup(GameObject hauler)
    {
        spriteRenderer.sortingLayerName = "Hauling";
        transform.parent = hauler.transform;
        transform.position = hauler.transform.position;
    }

    public void Drop()
    {
        spriteRenderer.sortingLayerName = "Wall";
        transform.parent = null;
        transform.position = new Vector2(
            Mathf.Round(transform.position.x) + 0.5f,
            Mathf.Round(transform.position.y) + 0.5f);
    }

    public virtual void Deposit()
    {
        EventManager.TriggerEvent("HaulableDeposited", this);
        DestroySelf();
    }

	public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
