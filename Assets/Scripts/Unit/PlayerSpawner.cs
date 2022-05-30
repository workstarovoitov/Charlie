using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using MenuManagement;

public class PlayerSpawner : MonoBehaviour
{
    //public static event Action OnPlayerSpawn;

    void Start()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CinemachineVirtualCamera vcam = camera.GetComponentInChildren<CinemachineVirtualCamera>();         
       
        vcam.Follow = transform;
        SettingsMenu sm = FindObjectOfType<SettingsMenu>(true);
        if (sm && sm.IsMobileDevice())
        {       
            string size = System.Enum.GetName(typeof(CONTROLSIZE), sm.CTRLSize);

            switch (size)
            {
                case "Small":
                    vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 2.5f, 0);
                    break;
                case "Medium":
                    vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 1.75f, 0);
                    break;
                case "Large":
                    vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 0.75f, 0);
                    break;
            }
        }
        else
        {
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 2.5f, 0);
        }
        //OnPlayerSpawn?.Invoke();
    }
}
