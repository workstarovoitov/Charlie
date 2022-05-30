using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(UnitMain))]

class UnitMovementFlap : MonoBehaviour
{
    [SerializeField] UnitMain uMain;
    [SerializeField] private float defaultFlapScale = 3f;

    [SerializeField] private string flapSFX;

    private float flapScale = 1f;
    
    [SerializeField] private GameObject flapVFX;

    public void SetFlapForce(float multiplier)
    {
        flapScale = remap(-1f, 1f, 0.1f, 1.75f, multiplier);
    }

    public void AE_Flap()
    {
        uMain.rb.velocity = new Vector2(uMain.rb.velocity.x,  defaultFlapScale * flapScale);
        uMain.uSFX.PlayOneShotSFX(flapSFX);
    }
    float remap(float origFrom, float origTo, float targetFrom, float targetTo, float value)
    {
        float rel = Mathf.InverseLerp(origFrom, origTo, value);
        return Mathf.Lerp(targetFrom, targetTo, rel);
    }
}