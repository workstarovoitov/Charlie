using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MenuManagement;
[RequireComponent(typeof(UnitSFX))]

public class UsableObject : MonoBehaviour, IUsable
{
    [SerializeField] public LayerMask CollisionLayer;

    [SerializeField] private InventoryItem item = null;
    public InventoryItem Item
    {
        get => item;
        set => item = value;
    }

    [SerializeField] private int amount;
    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    [HideInInspector]
    public UnitInventoryManager Inventory;
    [HideInInspector]
    public UnitSFX USFX;


    public void Start()
    {
        if (!USFX) USFX = GetComponent<UnitSFX>();
        if (item)        USFX.PlayOneShotSFX(item.SpawnSFX);
    }

    public void Use()
    {
        if (Inventory.AddItemToInventory(gameObject))
        {
            USFX.PlayOneShotSFX(item.GrabSFX, false);
            Destroy(gameObject);
        }
        else
        {
            USFX.PlayOneShotSFX(item.FullSFX, false);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
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
        }
    }
   
    public void OnTriggerStay2D(Collider2D col)
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
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if ((CollisionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
        {
            col.GetComponent<UnitUse>().SetObjectToUse(null);
            SettingsMenu sm = FindObjectOfType<SettingsMenu>(true);
            if (sm.IsMobileDevice())
            {
                col.GetComponent<PlayerInput>().actions.FindAction("Dash").Disable();
                col.GetComponent<PlayerInput>().actions.FindAction("Use").Enable();
            }
            Inventory = null;
        }
    }


}
