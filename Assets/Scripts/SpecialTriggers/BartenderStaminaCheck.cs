using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BartenderStaminaCheck : MonoBehaviour
{

    public UnityEvent OnStaminaRestored;
    private GameObject player;
    private bool active = true;
    // Start is called before the first frame update
    void Start()
    { 
        if (!active)
        {
            enabled = false;
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (active && player.GetComponent<UnitStaminaManager>().CurrentStamina > 100)
        {
            OnStaminaRestored.Invoke();
            active = false;
            enabled = false;
        }
       
    }
}
