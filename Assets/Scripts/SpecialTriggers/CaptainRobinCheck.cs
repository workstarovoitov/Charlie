using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CaptainRobinCheck : MonoBehaviour
{
    public UnityEvent OnRobinDead;
    private GameObject progress;
    private bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        progress = GameObject.FindGameObjectWithTag("UserInterface");
    }

    // Update is called once per frame
    void Update()
    {
        if (active && progress.GetComponent<GameProgressManager>().Progress.ViciousRobinDead)
        {
            OnRobinDead.Invoke();
            active = false;
            enabled = false;
        }
    }
}
