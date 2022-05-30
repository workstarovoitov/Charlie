using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnitSFX))]

public class MeatController : UsableObject, IUsable
{
    [SerializeField] private List<InventoryItem> items = null;
    public List<InventoryItem> Items
    {
        get => items;
        set => items = value;
    }
    [SerializeField] private int activeItemNum = -1;
    public int ActiveItemNum
    {
        get => activeItemNum;
        set 
        { 
            activeItemNum = value;
            Item = items[activeItemNum];

            transform.Find("Pict").gameObject.GetComponent<SpriteRenderer>().sprite = Item.ItemSprite;
        }
    }
    public new void Start()
    {
        if (activeItemNum < 0) activeItemNum = Mathf.RoundToInt(Random.Range(0, items.Count));
        Item = items[activeItemNum];

        transform.Find("Pict").gameObject.GetComponent<SpriteRenderer>().sprite = Item.ItemSprite;
        if (!USFX) USFX = GetComponent<UnitSFX>();
        USFX.PlayOneShotSFX(Item.SpawnSFX);
    }
}
