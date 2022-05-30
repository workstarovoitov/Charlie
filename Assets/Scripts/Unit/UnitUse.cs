using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using MenuManagement;
[RequireComponent(typeof(UnitMain))]

public class UnitUse : MonoBehaviour
{
	public GameObject usableObject;

    public void Awake()
    {
        SettingsMenu sm = FindObjectOfType<SettingsMenu>(true);
        if (sm && sm.IsMobileDevice())
        {
            GetComponent<PlayerInput>().actions.FindAction("Use").Disable();
        }
    } 
    public void Start()
    {
        SettingsMenu sm = FindObjectOfType<SettingsMenu>(true);
        if (sm && sm.IsMobileDevice())
        {
            GetComponent<PlayerInput>().actions.FindAction("Use").Disable();
        }
    }
    public void SetObjectToUse(GameObject obj)
	{
        usableObject = obj;
	}

    public void OnUse()
    {
        if (usableObject == null) return;
        usableObject.GetComponent<IUsable>().Use();
    }
}
