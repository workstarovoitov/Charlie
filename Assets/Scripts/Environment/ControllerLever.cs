using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ControllerLever : UsableObject, IUsable
{
    [SerializeField] private DOORSTATE state;

    [SerializeField] private Sprite openedPosition;
    [SerializeField] private Sprite closedPosition;
    [SerializeField] private string leverSFX = "";
    [SerializeField] private string leverBlockedSFX = "";
    [SerializeField] private bool oneTimeSwitch;


    [SerializeField] private ControllerDoor door;
    private bool switched = false;

    public UnityEvent OnLeverSwitched;

    public new void Start()
    {
        if (!USFX) USFX = GetComponent<UnitSFX>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedPosition;
    }

    public new void Use()
    {
        if (oneTimeSwitch && switched)
        {
            
            if (USFX != null)
            {
                USFX.PlayOneShotSFX(leverBlockedSFX, false);
            }
            return;
        }

        switched = true;
        OnLeverSwitched.Invoke();

        if (state == DOORSTATE.OPENED)
        {
            state = DOORSTATE.CLOSED;
        }
        else
        {
            state = DOORSTATE.OPENED;
        }

        SetState(state);
    }

    public void SetState(DOORSTATE newstate)
    {
        state = newstate;
        door.SetState(state);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (USFX != null)
        {
            USFX.PlayOneShotSFX(leverSFX, false);
        }
        if (state == DOORSTATE.OPENED)
        {
            sr.sprite = openedPosition;
            return;
        }
        sr.sprite = closedPosition;
    }


}