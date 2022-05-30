using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(UnitSFX))]

class IdleSFX : MonoBehaviour
{
	[SerializeField] private string idleSFX;
	[SerializeField] private bool playOnStart = false;
    private UnitSFX uSFX;

    void Start()
    {
        uSFX = GetComponent<UnitSFX>();
        if (playOnStart)
        {
            AE_IdleSFXInvisible();
        }
    }
    public void AE_IdleSFX()
    {
        if (idleSFX == null) return;
        uSFX.PlayOneShotSFX(idleSFX);
    }
    public void AE_IdleSFXInvisible()
    {
        if (idleSFX == null) return;
        uSFX.PlayOneShotSFX(idleSFX, false);
    }
}
