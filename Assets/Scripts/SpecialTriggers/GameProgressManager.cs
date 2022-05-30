using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuManagement;

public class GameProgressManager : MonoBehaviour
{
    [SerializeField] private GameProgress progress;
    public GameProgress Progress => progress;


    // Start is called before the first frame update
    private void Start()
    {
        ControllerSavePoint.OnSave += SaveProgress;
        ControllerSceneSwitch.OnSave += SaveProgress;
        PauseMenu.OnSaveButton += SaveProgress;

        if (ES3.KeyExists("progress"))
        {
            ES3.LoadInto("progress", progress);
        }
    }
    private void OnDestroy()
    {
        ControllerSavePoint.OnSave -= SaveProgress;
        ControllerSceneSwitch.OnSave -= SaveProgress;
        PauseMenu.OnSaveButton -= SaveProgress;
    }

    public void SaveProgress()
    {
        ES3.Save("progress", progress);
    }

}
