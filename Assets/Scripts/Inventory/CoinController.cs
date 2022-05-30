using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MenuManagement;

[RequireComponent(typeof(UnitSFX))]

public class CoinController : UsableObject
{
    [SerializeField] private GameObject bag;
    public GameObject Bag
    {
        get => bag;
    }

    public new void Use()
    {
        if (Inventory.AddItemToInventory(bag))
        {
            USFX.PlayOneShotSFX(Item.GrabSFX, false);
            Destroy(gameObject);
        }
        else
        {
            USFX.PlayOneShotSFX(Item.FullSFX, false);
        }
    }

    public new void OnTriggerEnter2D(Collider2D col)
    {
        if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
        {
            SettingsMenu sm = FindObjectOfType<SettingsMenu>(true);
            if (sm.IsMobileDevice())
            {
                col.GetComponent<PlayerInput>().actions.FindAction("Dash").Disable();
                col.GetComponent<PlayerInput>().actions.FindAction("Use").Enable();
            }
            col.GetComponent<UnitUse>().SetObjectToUse(gameObject);
            Inventory = col.GetComponent<UnitInventoryManager>();
            Use();
        }
    }
}
