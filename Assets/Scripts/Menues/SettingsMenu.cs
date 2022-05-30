using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManagement
{
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [SerializeField] private Text soundValue;
        [SerializeField] private Text fxValue;
        [SerializeField] private Text sizeValue;
        [SerializeField] private GameObject stickSwImage;
        
        [SerializeField] private GameObject soundMenu;
        [SerializeField] private GameObject controlsPCMenu;
        [SerializeField] private GameObject controlsMobileMenu;
        
        private float currentAmbVol;
        private float currentFXVol;
        private CONTROLSIZE ctrlSize;
        public CONTROLSIZE CTRLSize
        {
            get => ctrlSize;
        }

        private bool stickEnabled;
        public bool StickEnabled
        {
            get => stickEnabled;
        }

        private FMOD.Studio.VCA vcaAmb;
        private FMOD.Studio.VCA vcaFX;
        private int currentPage = 0;

        private void OnEnable()
        {
            actions.Enable();
        }

        private void OnDisable()
        {
            actions.Disable();
        }
        private void Start()
        {
            actions.Menu.IncreasePage.performed += _ => OnChangePage(1);
            actions.Menu.DecreasePage.performed += _ => OnChangePage(-1);
        }

        public override void ShowMenu()
        {
            selectedButton.Select();
            vcaAmb = FMODUnity.RuntimeManager.GetVCA("vca:/amb");
            vcaFX = FMODUnity.RuntimeManager.GetVCA("vca:/fx");
            currentAmbVol = PlayerPrefs.HasKey("ambVol") ? PlayerPrefs.GetFloat("ambVol") : 1f;
            currentFXVol = PlayerPrefs.HasKey("fxVol") ? PlayerPrefs.GetFloat("fxVol") : 1f;
            vcaAmb.setVolume(currentAmbVol);
            vcaFX.setVolume(currentFXVol);
            soundValue.text = (Mathf.RoundToInt(currentAmbVol * 100)).ToString();
            fxValue.text = (Mathf.RoundToInt(currentFXVol * 100)).ToString();
            
            //if (IsMobileDevice())
            //{
                ctrlSize = PlayerPrefs.HasKey("ctrlSize") ? (CONTROLSIZE)PlayerPrefs.GetInt("ctrlSize") : 0;
                sizeValue.text = System.Enum.GetName(typeof(CONTROLSIZE), ctrlSize);

                if (!PlayerPrefs.HasKey("joystickEnabled"))
                {
                    PlayerPrefs.SetInt("joystickEnabled", 0);
                    PlayerPrefs.Save();
                }

                stickEnabled = PlayerPrefs.GetInt("joystickEnabled") > 0 ? true : false;
                stickSwImage.SetActive(stickEnabled);
            //}
            ShowPage();
        }

        public void OnChangeAmbVolume(float vol)
        {
            ButtonClickSFX();
            currentAmbVol += vol;
            currentAmbVol = Mathf.Clamp(currentAmbVol, 0f, 1f);
            vcaAmb.setVolume(currentAmbVol);
            PlayerPrefs.SetFloat("ambVol", currentAmbVol);
            PlayerPrefs.Save();
            soundValue.text = (Mathf.RoundToInt(currentAmbVol * 100)).ToString();
        }

        public void OnChangeFXVolume(float vol)
        {
            ButtonClickSFX();
            currentFXVol += vol;
            currentFXVol = Mathf.Clamp(currentFXVol, 0f, 1f);
            vcaFX.setVolume(currentFXVol);
            PlayerPrefs.SetFloat("fxVol", currentFXVol);
            PlayerPrefs.Save();
            fxValue.text = (Mathf.RoundToInt(currentFXVol * 100)).ToString();
        }
        public void OnChangeControlsSize(int val)
        {
            ButtonClickSFX();
            ctrlSize += val;
            ctrlSize = (CONTROLSIZE)(((int)ctrlSize + System.Enum.GetNames(typeof(CONTROLSIZE)).Length) % System.Enum.GetNames(typeof(CONTROLSIZE)).Length);
            PlayerPrefs.SetInt("ctrlSize", (int)ctrlSize);
            PlayerPrefs.Save();
            sizeValue.text = System.Enum.GetName(typeof(CONTROLSIZE), ctrlSize);
        }       
        
        public void OnChangePage(int val)
        {
            ButtonClickSFX();
            currentPage += val;
            currentPage = (currentPage + 2) % 2;
            ShowPage();
        }

        public override void OnDecreasePage()
        {
            OnChangePage(-1);
        }

        public override void OnIncreasePage()
        {
            OnChangePage(1);
        }

        public void ShowPage()
        {
            soundMenu.SetActive(false);
            controlsPCMenu.SetActive(false);
            controlsMobileMenu.SetActive(false);

            switch (currentPage)
            {
                case 0:
                    soundMenu.SetActive(true);
                    break;
                case 1:
                    if (IsMobileDevice())
                    {
                        controlsMobileMenu.SetActive(true);
                    }
                    else
                    {
                        controlsPCMenu.SetActive(true);
                    }
                    break;
            }
        }
        public void OnStickEnable()
        {
            ButtonClickSFX();

            if (PlayerPrefs.GetInt("joystickEnabled") > 0)
            {
                PlayerPrefs.SetInt("joystickEnabled", 0);
                stickEnabled = false;
            }
            else
            {
                PlayerPrefs.SetInt("joystickEnabled", 1);
                stickEnabled = true;
            }

            stickSwImage.SetActive(stickEnabled);
            PlayerPrefs.Save();
        }
    }
}