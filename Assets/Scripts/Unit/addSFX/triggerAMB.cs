using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

class triggerAMB : MonoBehaviour
{
    private EventInstance amb;

    public void PlayAMB(string ambience)
    {
        amb.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        new WaitForSeconds(5);
        if (ambience == null)
        {
            return;
        }
        amb = RuntimeManager.CreateInstance(ambience);
        RuntimeManager.AttachInstanceToGameObject(amb, transform, GetComponent<Rigidbody>());
        amb.start();
    }
    
    public void StopAMB()
    {
        amb.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
