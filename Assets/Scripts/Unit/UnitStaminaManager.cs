using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitStaminaManager : MonoBehaviour
{
    [SerializeField] UnitMain uMain;

    [SerializeField] private float currentStamina = 100;
    public float CurrentStamina
    {
        get => currentStamina;
        set => currentStamina = value;
    }

    [SerializeField] private float currentMaxStamina = 100;
    public float CurrentMaxStamina
    {
        get => currentMaxStamina;
        set => currentMaxStamina = value;
    }
    [SerializeField] private float maxStamina = 100;
    public float MaxStamina
    {
        get => maxStamina;
        set => maxStamina = value;
    }
    [SerializeField] private float minimalMaxStamina = 30;
    [SerializeField] private float staminaThreshold = 1;
    [SerializeField] private float staminaRecoverySpeed = 100;
    [SerializeField] private float stunMultiplier = 0.02f;

    private float lastStaminaDecreaseTime = 0;
    private bool staminaRecovering = false;
    private bool run = false;
    private bool lowStamina = false;
    public bool LowStamina
    {
        get => lowStamina;
        set => lowStamina = value;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!uMain.uState.CurrentRunState && run)
        {
            run = false;
            StopCoroutine(RunStaminaSelfrestore());
            StartCoroutine(RunStaminaSelfrestore(5, staminaRecoverySpeed));
        }
        if (currentStamina < currentMaxStamina && Time.time - lastStaminaDecreaseTime >= staminaThreshold && !staminaRecovering)
        {
            StopCoroutine(RunStaminaSelfrestore());
            if (uMain.uState.CurrentRunState)
            {
                StartCoroutine(RunStaminaSelfrestore(1, staminaRecoverySpeed * 0.3f));
                run = true;
            }
            else
            {
                StartCoroutine(RunStaminaSelfrestore(5, staminaRecoverySpeed));
            }
        }
    }

    public void RestoreStamina()
    {
        currentMaxStamina = maxStamina;
        currentStamina = maxStamina;
        StartCoroutine(RunStaminaSelfrestore(5, staminaRecoverySpeed));
    }

    public void UseItem(StaminaPotion item)
    {
        StartCoroutine(RunStaminaRestore(item));
    }
   
    public void UseItem(BeerPotion item)
    {
        currentStamina *= 10;
        currentMaxStamina = maxStamina;
    }

    public void StaminaDecrease(float staminaCost)
    {
        StopCoroutine(RunStaminaSelfrestore());

        lastStaminaDecreaseTime = Time.time;
        currentStamina -= staminaCost;
        if (currentStamina < 0)
        {
            uMain.uStamina.LowStamina = true;
            currentStamina = 0;
        }

        if (currentMaxStamina > minimalMaxStamina)
        {
            currentMaxStamina -= staminaCost * stunMultiplier;
        }
        staminaRecovering = false;
    }  
    
    public void StaminaIncrease(float stamina)
    {
        currentStamina += stamina;
        if (currentStamina > currentMaxStamina) currentStamina = currentMaxStamina;
    }

    IEnumerator RunStaminaRestore(StaminaPotion sp)
    {
        float startTime = Time.time;
        while (startTime + sp.StaminaRestoreTime > Time.time)
        {
            StaminaIncrease(sp.StaminaRestorePortion);
            yield return new WaitForSeconds(1f / sp.StaminaRestoreSpeed);
        }
    }

    IEnumerator RunStaminaSelfrestore(int restore = 5, float speed = 100)
    {
        staminaRecovering = true;
        while (currentStamina < currentMaxStamina && staminaRecovering)
        {
            StaminaIncrease(restore);
            yield return new WaitForSeconds(1 / speed);
        }
        yield return null;
    }
}
