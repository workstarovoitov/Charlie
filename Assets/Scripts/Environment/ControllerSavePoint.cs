using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;
using MenuManagement;

[RequireComponent(typeof(UnitAnimator))]

public class ControllerSavePoint : UsableObject, IUsable
{
    [SerializeField] private string applySFX = "";
    [SerializeField] private string blockedSFX = "";
    [SerializeField] private string idleSFX = "";
    [SerializeField] private bool active = true;

    [SerializeField] private float enemyRadius = 5f;
    [SerializeField] private Vector3 enemyRadiusBias = Vector3.zero;
    [SerializeField] private LayerMask enemyLayer;

    private UnitAnimator uAnimator;
    public static event Action OnSave;
    private bool checkActive = true;
    private GameObject ui;
    private GameObject key;
    private EventInstance idle;

    public UnityEvent OnSavePointActivated;

    private new void Start()
    {
        if (!USFX) USFX = GetComponent<UnitSFX>();
        if (!uAnimator) uAnimator = GetComponent<UnitAnimator>();
        idle = USFX.CreateSFX(idleSFX);
        ui = transform.Find("UI").gameObject;
        key = transform.Find("Key").gameObject;
        if (!active)
        {
            uAnimator.SetAnimatorTrigger("Off");
            Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position + enemyRadiusBias, enemyRadius, enemyLayer);

            for (int i = 0; i < hitCollider.Length; i++)
            {
                hitCollider[i].gameObject.SetActive(false);
            }
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
        if (checkActive && !active)
        {
            checkActive = false;
            uAnimator.SetAnimatorTrigger("Off");
            Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position + enemyRadiusBias, enemyRadius, enemyLayer);

            for (int i = 0; i < hitCollider.Length; i++)
            {
                hitCollider[i].gameObject.SetActive(false);
            }
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
        if (active && !IsEnemyNearSavePoint())
        {
            if (Item != null)
            {
                int pageIndex = Inventory.GetPageIndex(Item);
                foreach (ItemsStack stack in Inventory.UInventory.Pages[pageIndex].Stack)
                {
                    if (stack.Items.ItemName.Contains(Item.ItemName))
                    {        
                        Inventory.RemoveItemFromInventory(Item);
                        Item = null;
                        USFX.StopSFX(idle);
                        OnSavePointActivated.Invoke();
                        OnSavePoint();
                        return;
                    }
                }
                USFX.PlayOneShotSFX(blockedSFX);
                return;
            }
            OnSavePoint();
        }
        USFX.PlayOneShotSFX(blockedSFX);
        return;
    }

    private void OnSavePoint()
    {
        active = false;
        uAnimator.SetAnimatorTrigger("Off");
        USFX.PlayOneShotSFX(applySFX);
        ui.GetComponent<UISwitcher>().StopAllCoroutines();
        ui.GetComponent<Collider2D>().enabled = false;
        ui.SetActive(false);
        
        key.GetComponent<UISwitcher>().StopAllCoroutines();
        key.GetComponent<Collider2D>().enabled = false;
        key.SetActive(false);

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = false;
        col.enabled = false;
        OnSave();
    }

    public new void OnTriggerEnter2D(Collider2D col)
    {
        if (IsEnemyNearSavePoint())
        {
            ui.GetComponent<UISwitcher>().StopAllCoroutines();
            ui.GetComponent<Collider2D>().enabled = false;

            key.GetComponent<UISwitcher>().StopAllCoroutines();
            key.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            ui.GetComponent<Collider2D>().enabled = true;
            key.GetComponent<Collider2D>().enabled = true;
        }

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

    private bool IsEnemyNearSavePoint()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position + enemyRadiusBias, enemyRadius, enemyLayer);

        if (hitCollider != null) return true;
        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        // front wall check box

        //cliff check rays 
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + enemyRadiusBias, enemyRadius);

    }
#endif
}