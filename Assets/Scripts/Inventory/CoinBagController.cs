using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnitSFX))]

public class CoinBagController: UsableObject, IUsable
{
    public new void Use()

    {
        if (Inventory.AddItemToInventory(gameObject, Amount))
        {
            USFX.PlayOneShotSFX(Item.GrabSFX, false);
            Destroy(gameObject);
        }
        else
        {
            USFX.PlayOneShotSFX(Item.FullSFX, false);
        }
    }
}
