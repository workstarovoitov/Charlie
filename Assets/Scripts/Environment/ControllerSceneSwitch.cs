using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControllerSceneSwitch : UsableObject, IUsable
{
    [SerializeField] private string sceneToLoad;
    public static event Action OnSave;

    public new void Start()
    {
    }

    public new void Use()
    {
        OnSave();
        StartCoroutine(LoadScene());
        return;
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/");
        bus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SceneManager.LoadScene(sceneToLoad);
    }
}