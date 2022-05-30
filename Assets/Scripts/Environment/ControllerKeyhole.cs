using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ControllerKeyhole : UsableObject, IUsable
{
    [SerializeField] private DOORSTATE state;
    [SerializeField] private ControllerDoor door;
  
    private GameObject keyImage = null;

    public UnityEvent OnLockOpened;

    public new void Start()
    {
        if (!USFX) USFX = GetComponent<UnitSFX>();

        if (Item != null)
        {
            keyImage = transform.Find("Key").gameObject;
            keyImage.GetComponent<SpriteRenderer>().sprite = Item.ItemSprite;
        }
    }

    public new void Use()
    {
        int pageIndex = Inventory.GetPageIndex(Item);
        foreach (ItemsStack stack in Inventory.UInventory.Pages[pageIndex].Stack)
        {
            if (stack.Items.ItemName.Contains(Item.ItemName))
            {
                USFX.PlayOneShotSFX(Item.UseSFX, false);
                SetState(DOORSTATE.OPENED);
                Inventory.RemoveItemFromInventory(Item);
                keyImage.GetComponent<SpriteRenderer>().sprite = null;
                Item = null;
                OnLockOpened.Invoke();
                gameObject.SetActive(false);
                return;
            }
        }
        USFX.PlayOneShotSFX(Item.BlockedSFX, false);
    }

    public void SetState(DOORSTATE newstate)
    {
        state = newstate;
        door.SetState(state);
    }


}