using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class UnitSFX : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float pitchSFX = 0.5f;



    public EventInstance CreateSFX(string eventName)
    {
        EventInstance sfx = RuntimeManager.CreateInstance(eventName);
        RuntimeManager.AttachInstanceToGameObject(sfx, transform, GetComponent<Rigidbody>());
        return sfx;
    }

    public void SetParameter(EventInstance sfx, string paramName, float value)
    {
        sfx.setParameterByName(paramName, value, false);
    }

    public void PlaySFX(EventInstance sfx)
    {
        if (IsVisible())
        {
            sfx.start();
        }
        sfx.release();
    }
    
    public void StopSFX(EventInstance sfx)
    {
        sfx.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayOneShotSFX(string eventName, bool playOnlyIfVisible = true)
    {
        if (!IsVisible() && playOnlyIfVisible) return;
        if (string.IsNullOrEmpty(eventName)) return;
        FMODUnity.RuntimeManager.PlayOneShot(eventName, transform.position);
    }
  
    public void PlayOneShotPitchedSFX(string eventName)
    {
        if (!IsVisible()) return;
        if (string.IsNullOrEmpty(eventName)) return;

        EventInstance sfx = CreateSFX(eventName);
        SetParameter(sfx, "voicePitch", Random.Range(pitchSFX * 0.925f, pitchSFX * 1.075f));
        PlaySFX(sfx);
    }

    public bool IsVisible()
    {
        Renderer m_Renderer = GetComponent<Renderer>();
        if (m_Renderer && m_Renderer.isVisible)
        {
            return true;
        }
        return false;
    }

    public void PlaySFX(string eventName, string[] parameters, float[] values, bool playOnlyIfVisible = true)
    {
        if (!IsVisible() && playOnlyIfVisible) return;
        if (string.IsNullOrEmpty(eventName)) return;

        EventInstance sfx = RuntimeManager.CreateInstance(eventName);
        RuntimeManager.AttachInstanceToGameObject(sfx, transform, GetComponent<Rigidbody>());
        for (int i = 0; i < parameters.Length; i++)
        {
            sfx.setParameterByName(parameters[i], values[i], false);
        }
        sfx.start();
        sfx.release();
    }
    
    public void PlaySFX(string eventName, bool playOnlyIfVisible = true)
    {
        if (!IsVisible() && playOnlyIfVisible) return;
        if (string.IsNullOrEmpty(eventName)) return;
        FMODUnity.RuntimeManager.PlayOneShot(eventName, transform.position);
    }
}