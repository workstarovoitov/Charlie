using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelLoadEvent : MonoBehaviour
{
    public UnityEvent OnLoad;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendLoadEvent());
    }

    IEnumerator SendLoadEvent()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        OnLoad.Invoke();
    }
}
