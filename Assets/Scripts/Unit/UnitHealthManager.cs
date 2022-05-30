using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class UnitHealthManager : MonoBehaviour
{
    [SerializeField] UnitMain uMain;

    [SerializeField] private float currentHealth = 100;
    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    [SerializeField] private float currentMaxHealth = 100;
    public float CurrentMaxHealth
    {
        get => currentMaxHealth;
        set => currentMaxHealth = value;
    }
    [SerializeField] private float maxHealth = 100;
    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    [SerializeField] private float minimalMaxHealth = 30;
    [SerializeField] private float traumaScaler = 0.1f;

    [SerializeField] private LayerMask deadLayer;       //layer for dead bodies
    [Header("SFX")]
    [SerializeField] private string deathSFX = "";
    private float healthRecoverySpeed = 100;

    private float lastHealthChangeTime = 0;

    public UnityEvent OnDied;

    public void RestoreHealth()
    {
        if (uMain.uState.CurrentState != UNITSTATE.DEATH)
        {
            currentMaxHealth = maxHealth;
            currentHealth = maxHealth;
        }
    }

    public void UseItem(HealthPotion item)
    {
        StartCoroutine(RunHealSequence(item));
    }
    
    public void UseItem(HealPotion item)
    {
        currentMaxHealth = maxHealth;

        if (item.HealthRestorePortion > 1.0f)
        {
            HealthIncrease(item.HealthRestorePortion);
        }

        else
        {
            HealthIncrease(item.HealthRestorePortion * currentMaxHealth);
        }
    }

    public void HealthDecrease(float cost)
    {
        lastHealthChangeTime = Time.time;
        currentHealth -= cost;
        if (currentHealth < 0) 
        { 
            currentHealth = 0;
        }

        if (currentMaxHealth > minimalMaxHealth)
        {
            currentMaxHealth -= cost * traumaScaler;
        }

        if (currentHealth <= 0)
        {
            DeathSequence();
        }
    }   

    public void HealthIncrease(float cost)
    {
        currentHealth += cost;
        if (currentHealth > currentMaxHealth) currentHealth = currentMaxHealth;
    }

    public void DeathSequence()
    {

        if (uMain.uState.Flying)
        {
            uMain.uState.Flying = false;
            uMain.uMovement.GravityScale = 4;
            uMain.uMovement.ResetState();
        }

        uMain.uState.CurrentState = UNITSTATE.DEATH;

        uMain.uSFX.PlayOneShotPitchedSFX(deathSFX);

        uMain.uMovementAttack.CancelInvoke();
        uMain.uMovementAttack.StopAllCoroutines();
        uMain.uMovementAttack.ResetAttack();
        //uMain.uMovementAttack.enabled = false;
        
        //gameObject.layer = (int)Mathf.Log(deadLayer.value, 2);


        StartCoroutine(RunDeathAnimation());
    }

    IEnumerator RunDeathAnimation()
    {

        while (!uMain.uState.CurrentGroundedState)
        {
            yield return new WaitForFixedUpdate();
        }

        uMain.uState.CurrentState = UNITSTATE.DEATH;
        uMain.uAnimator.SetAnimatorTrigger("Death");
 
       // uMain.uMovement.enabled = false;
        uMain.rb.velocity = Vector2.zero; 
 
        gameObject.layer = (int)Mathf.Log(deadLayer.value, 2);


    }

    IEnumerator RunHealSequence(HealthPotion hp)
    {
        int restoreNum = hp.HealthRestoreNum;
        while (restoreNum > 0)
        {
            if(hp.HealthRestorePortion > 1.0f)
            {
                HealthIncrease(hp.HealthRestorePortion);
            }
            else
            {
                HealthIncrease(hp.HealthRestorePortion * MaxHealth);
            }
            restoreNum--;
            yield return new WaitForSeconds(1f/hp.HealthRestoreSpeed);
        }
        yield return null;
    }
    
    public void AE_Dead()
    {
        OnDied.Invoke();
    }

    public void RestartUnit()
    {
        uMain.uMovementAttack.enabled = true;
        uMain.uMovement.enabled = true;
        EnemyActions ea = GetComponent<EnemyActions>();
        if (ea != null) ea.enabled = true;
        uMain.uState.CurrentState = UNITSTATE.IDLE;
        uMain.uAnimator.SetAnimatorTrigger("Idle");
        currentMaxHealth = maxHealth;
        currentHealth = maxHealth;
        gameObject.layer = (int)Mathf.Log(uMain.uState.DefaultLayer, 2);

    }
}
