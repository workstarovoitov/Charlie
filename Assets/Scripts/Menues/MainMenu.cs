using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace MenuManagement
{
    public class MainMenu : Menu<MainMenu>
    {
        private FMOD.Studio.VCA vcaAmb;
        private FMOD.Studio.VCA vcaFX;

        public override void ShowMenu()
        {            
            selectedButton.Select();

            vcaAmb = FMODUnity.RuntimeManager.GetVCA("vca:/amb");
            vcaFX = FMODUnity.RuntimeManager.GetVCA("vca:/fx");
            float currentAmbVol = PlayerPrefs.HasKey("ambVol") ? PlayerPrefs.GetFloat("ambVol") : 1f;
            float currentFXVol = PlayerPrefs.HasKey("fxVol") ? PlayerPrefs.GetFloat("fxVol") : 1f;
            vcaAmb.setVolume(currentAmbVol);
            vcaFX.setVolume(currentFXVol);
            selectedButton.Select();
            selectedButton.Select();

        }

        public void OnNGPressed()
        {
            ButtonClickSFX();
            Debug.Log("save file removed");
            ES3.DeleteFile("SaveFile.es3");
            ES3.DeleteFile("AutoSaveFile.es3");
            ES3.DeleteFile("MainSaveFile.es3");
            ES3.DeleteFile();

            //uInventoryBackup.Pages = new List<InventoryPage>(uInventoryDefault.Pages);

            // ES3.DeleteFile();
            //PlayerPrefs.DeleteAll();
            //PlayerPrefs.Save();
            StartCoroutine(LoadScene());
        }

        public void OnContinuePressed()
        {
            ButtonClickSFX();
            SceneManager.LoadScene((string)ES3.Load("savedScene"));
            LoadingMenu.Open();
            //LevelsMenu.Open();
        }

        public void OnSettingsPressed()
        {
            ButtonClickSFX();
            SettingsMenu.Open();
        }

        public void OnCommentsPress()
        {
            Application.OpenURL("market://details?id=" + Application.identifier);
        }
        public override void OnBackPressed()
        {
            ButtonClickSFX();
            Application.Quit();
        }
        IEnumerator LoadScene()
        {
            yield return new WaitForSecondsRealtime(0.25f);

            SceneManager.LoadScene("Tavern");
            LoadingMenu.Open();
        }

    }
}