using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(UnitAnimator))]

public class ControllerChest : UsableObject, IUsable
{
    [SerializeField] private string openedSFX = "";
    [SerializeField] private string closedSFX = "";
    [SerializeField] private string blockedSFX = "";

    private UnitAnimator uAnimator;
    private DOORSTATE state = DOORSTATE.CLOSED;
    private bool switched = false;
    private GameObject keyImage = null;
    private bool checkActive = true;
    private GameObject ui;
    private GameObject key;
      
    public UnityEvent OnChestOpen;

    private new void Start()
    {
        if (!USFX) USFX = GetComponent<UnitSFX>();
        if (!uAnimator) uAnimator = GetComponent<UnitAnimator>();
        ui = transform.Find("UI").gameObject;
        key = transform.Find("Key").gameObject;

        if (Item != null)
        {
            keyImage = transform.Find("Key").gameObject;
            keyImage.GetComponent<SpriteRenderer>().sprite = Item.ItemSprite;
        }

        if (switched)
        {
            uAnimator.SetAnimatorTrigger("Open");
            ui.GetComponent<UISwitcher>().StopAllCoroutines();
            ui.GetComponent<Collider2D>().enabled = false;
            ui.SetActive(false);

            key.GetComponent<UISwitcher>().StopAllCoroutines();
            key.GetComponent<Collider2D>().enabled = false;
            key.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            enabled = false;

        }
    }


    private void Update()
    {
        if (checkActive && switched)
        {
            checkActive = false;
            uAnimator.SetAnimatorTrigger("Open");
            ui.GetComponent<UISwitcher>().StopAllCoroutines();
            ui.GetComponent<Collider2D>().enabled = false;
            ui.SetActive(false);

            key.GetComponent<UISwitcher>().StopAllCoroutines();
            key.GetComponent<Collider2D>().enabled = false;
            key.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            enabled = false;
        }
    }


    public new void Use()
    {
        if (state == DOORSTATE.CLOSED && Item != null)
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

                    ui.GetComponent<UISwitcher>().StopAllCoroutines();
                    ui.GetComponent<Collider2D>().enabled = false;
                    ui.SetActive(false);

                    key.GetComponent<UISwitcher>().StopAllCoroutines();
                    key.GetComponent<Collider2D>().enabled = false;
                    key.SetActive(false);
                    
                    return;
                }
            }
            USFX.PlayOneShotSFX(blockedSFX);
            return;
        }

        if (state == DOORSTATE.CLOSED)
        {
            state = DOORSTATE.OPENED; 
        }
        else
        {
            state = DOORSTATE.CLOSED;
        }

        SetState(state);
    }

    public void SetState(DOORSTATE newstate)
    {
        state = newstate;
        if (state == DOORSTATE.OPENED)
        {
            if (!switched)
            {
                switched = true;
                OnChestOpen?.Invoke();
            }
            uAnimator.SetAnimatorTrigger("Open");
            USFX.PlayOneShotSFX(openedSFX);
        }
        else
        {
            uAnimator.SetAnimatorTrigger("Close");
            USFX.PlayOneShotSFX(closedSFX);
        }
    }


}