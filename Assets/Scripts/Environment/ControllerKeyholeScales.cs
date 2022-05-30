using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerKeyholeScales: UsableObject, IUsable
{
    [SerializeField] private DOORSTATE state;
    [SerializeField] private ControllerDoor door;
    [SerializeField] private GameObject key;
    [SerializeField] private int keyNum;
    [SerializeField] private Transform spawnPoint;

    private GameObject keyImage = null;

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
                Instantiate(key, new Vector3(spawnPoint.position.x + Random.Range(-0.1f, 0.1f), spawnPoint.position.y + Random.Range(0f, 0.2f), 0f), Quaternion.Euler(Vector3.zero));
                Inventory.RemoveItemFromInventory(Item);
                keyNum--;

                if (keyNum == 0)
                {
                    SetState(DOORSTATE.OPENED);
                    Destroy(gameObject);
                }
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