using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitSFX))]

public class ControllerLattice : ControllerDoor
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private DOORSTATE state = DOORSTATE.CLOSED;
    private DOORSTATE oldState;
    [SerializeField] private Transform openPosition;
    [SerializeField] private Transform closedPosition;
    [SerializeField] private string openSFX = "";
    [SerializeField] private string closeSFX = "";

    private UnitSFX uSFX;
    private BoxCollider2D bc;
    private Transform tagrgetPosition;
    private bool updatePosition = false;
    private bool onStartSwitch = true;

    private void Start()
    {
        uSFX = GetComponent<UnitSFX>();
        bc = GetComponent<BoxCollider2D>();

        onStartSwitch = true;
        SetState(state);
    }

    // Update is called once per frame
    void Update()
    {
        if (oldState != state)
        {
            SetState(state);
        }
        if (!updatePosition) return;
        if (Vector2.Distance(tagrgetPosition.position, transform.position) > .1f)
        {
            transform.position = Vector3.Lerp(transform.position, tagrgetPosition.position, Time.deltaTime * speed);
        }
        else
        {
            updatePosition = false;
        }

    }

    public override void SetState(DOORSTATE newstate)
    {
        oldState = state;
        state = newstate;
        if (newstate == DOORSTATE.OPENED)
        {
            OnOpen();
            return;
        }
        OnClose();
    }
    public override DOORSTATE GetState()
    {
       return state;
    }


    public void OnOpen()
    {
        uSFX.PlayOneShotSFX(openSFX, onStartSwitch);
        if (bc != null)
        {
            bc.enabled = false;
        }
        tagrgetPosition = openPosition;
        updatePosition = true;
        onStartSwitch = false;

    }
    public void OnClose()
    {
        uSFX.PlayOneShotSFX(closeSFX, onStartSwitch);
        if (bc != null)
        {
            bc.enabled = true;
        }

        tagrgetPosition = closedPosition;
        updatePosition = true;
        onStartSwitch = false;

    }

}
