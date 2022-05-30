using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using MenuManagement;
using UnityEngine.SceneManagement;
using System.Collections;



public class PlayerSaver : MonoBehaviour
{
    private void Start()
    {
        LoadingMenu.Open();
        LoadStats();
        Save();
       
    }

    private void OnEnable()
    {
        ControllerSavePoint.OnSave += RestoreAndSaveStats;
        ControllerSceneSwitch.OnSave += SaveStats;
        PauseMenu.OnSaveButton += SaveStats;
    }


    private void OnDisable()
    {
        ControllerSavePoint.OnSave -= RestoreAndSaveStats;
        ControllerSceneSwitch.OnSave -= SaveStats;
        PauseMenu.OnSaveButton -= SaveStats;
    }

    public void RestoreAndSaveStats()
    {
        SavingMenu.Open();
        GetComponent<UnitHealthManager>().RestoreHealth();
        GetComponent<UnitStaminaManager>().RestoreStamina();
        Save();
    }
   
    public void SaveStats()
    {
        SavingMenu.Open();
        Save();
    }

    public void Save()
    {

        ES3.Save("savedScene", SceneManager.GetActiveScene().name);

        ES3.Save("CurrentHealth", GetComponent<UnitHealthManager>().CurrentHealth);
        ES3.Save("CurrentMaxHealth", GetComponent<UnitHealthManager>().CurrentMaxHealth);
        ES3.Save("MaxHealth", GetComponent<UnitHealthManager>().MaxHealth);

        ES3.Save("CurrentStamina", GetComponent<UnitStaminaManager>().CurrentStamina);
        ES3.Save("CurrentMaxStamina", GetComponent<UnitStaminaManager>().CurrentMaxStamina);
        ES3.Save("MaxStamina", GetComponent<UnitStaminaManager>().MaxStamina);
        ES3AutoSaveMgr.Current.Save();
        Debug.Log(" auto save complete");
    }

    public void LoadStats()
    {
        ES3AutoSaveMgr.Current.Load();
        GetComponent<UnitHealthManager>().CurrentHealth = ES3.Load("CurrentHealth", GetComponent<UnitHealthManager>().CurrentHealth);
        GetComponent<UnitHealthManager>().CurrentMaxHealth = ES3.Load("CurrentMaxHealth", GetComponent<UnitHealthManager>().CurrentMaxHealth);
        GetComponent<UnitHealthManager>().MaxHealth = ES3.Load("MaxHealth", GetComponent<UnitHealthManager>().MaxHealth);

        GetComponent<UnitStaminaManager>().CurrentStamina = ES3.Load("CurrentStamina", GetComponent<UnitStaminaManager>().CurrentStamina);
        GetComponent<UnitStaminaManager>().CurrentMaxStamina = ES3.Load("CurrentMaxStamina", GetComponent<UnitStaminaManager>().CurrentMaxStamina);
        GetComponent<UnitStaminaManager>().MaxStamina = ES3.Load("MaxStamina", GetComponent<UnitStaminaManager>().MaxStamina);
        Debug.Log(" auto load complete");
    }

    IEnumerator RunLoadMenu()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        LoadingMenu.Open();
    }
}
