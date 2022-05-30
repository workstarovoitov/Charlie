using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

class soundtrackPlayer : MonoBehaviour
{
    [SerializeField] private string[] trackList;
    public float tense = 0;
    private EventInstance[] soundtrack;
    private SOUNDMODE currentMode = SOUNDMODE.EXPLORATION;

    public void Start()
    {
        PlayerDead.OnPlayerDead += StopAllAmbiences;

        soundtrack = new EventInstance[trackList.Length];
        for (int i = 0; i < trackList.Length; i++)
        {
            soundtrack[i] = RuntimeManager.CreateInstance(trackList[i]);
            RuntimeManager.AttachInstanceToGameObject(soundtrack[i], transform, GetComponent<Rigidbody>());
        }
        PlayAllAmbiences();
    }
    void OnDestroy()
    {
        StopAllAmbiences();
    }


    public void PlayAllAmbiences()
    {
        for (int i = 0; i < trackList.Length; i++)
        {
            PlayAMB(i);
        }
    }  
    
    public void StopAllAmbiences()
    {
        for (int i = 0; i < trackList.Length; i++)
        {
            StopAMB(i);
        }
    }
    public void PlayAMB(int trackNum)
    {
        soundtrack[trackNum].start();
    }
    public void StopAMB(int trackNum)
    {
        soundtrack[trackNum].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void SetTenseParameter(float value)
    {
        //soundtrack.setParameterByName("BanditPassTense", value);
    }


    public void SwitchMode(SOUNDMODE mode, float speed = 100f)
    {
        StopAllCoroutines();
        switch (mode)
        {
            case SOUNDMODE.EXPLORATION:
                if (currentMode == SOUNDMODE.COMBAT)
                {
                    StartCoroutine(SetTense(0.4f, 0.2f, speed));
                }
                else if(currentMode == SOUNDMODE.BOSS)
                {
                    StartCoroutine(SetTense(1.2f, 1.4f, speed));
                }

                currentMode = SOUNDMODE.EXPLORATION;
                break; 
            case SOUNDMODE.COMBAT:
                if (currentMode == SOUNDMODE.EXPLORATION)
                {

                    StartCoroutine(SetTense(0.2f, 0.4f, speed));
                }
                else if(currentMode == SOUNDMODE.BOSS)
                {
                    StartCoroutine(SetTense(1f, 0.6f, speed));
                }
                currentMode = SOUNDMODE.COMBAT;
                break;
            case SOUNDMODE.BOSS:
                if (currentMode == SOUNDMODE.EXPLORATION)
                {
                    StartCoroutine(SetTense(1.4f, 1f, speed));
                }
                else if (currentMode == SOUNDMODE.COMBAT)
                {
                    StartCoroutine(SetTense(0.6f, 1f, speed));
                }
                currentMode = SOUNDMODE.BOSS;
                break;
        }
    }
    public void SwitchToCombat(float speed)
    {
        SwitchMode(SOUNDMODE.COMBAT, speed);
    }
    public void SwitchToExploration(float speed)
    {
        SwitchMode(SOUNDMODE.EXPLORATION, speed);
    }
    public void SwitchToBoss(float speed)
    {
        SwitchMode(SOUNDMODE.BOSS, speed);
    }
    IEnumerator SetTense(float tenseStart, float tenseTarget, float speed)
    {
        tense = tenseStart;
        while (Mathf.Abs(tenseStart - tenseTarget) > .01f)
        {
            tense = Mathf.Lerp(tense, tenseTarget, Time.deltaTime * speed);
            SetTenseParameter(tense);
            yield return new WaitForFixedUpdate();
        }
    }
}
public enum SOUNDMODE
{
    INTRO = 0,
    EXPLORATION = 1,
    COMBAT = 2,
    BOSS = 3,
    OUTRO = 4,
};
