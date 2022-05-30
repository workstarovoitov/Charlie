using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuManagement
{
    public class EndMenu : Menu<EndMenu>
    {
        [SerializeField] private Image bckImage;
        [SerializeField] private GameObject buttons;

        [SerializeField] private string deathSFX;

        public override void ShowMenu()
        {
            Time.timeScale = 0.05f;

            buttons.SetActive(false);
            if (!string.IsNullOrEmpty(deathSFX)) 
            { 
                FMODUnity.RuntimeManager.PlayOneShot(deathSFX, transform.position);
            }
            StartCoroutine(SetFade());
        
        }

        public void OnExit()
        {
            ButtonClickSFX();
            Time.timeScale = 1;
            LevelLoader.LoadMainMenuLevel();
            MainMenu.Open();
        }


        IEnumerator SetFade()
        {
            float alpha = 1f;
            float alphaTarget = .0f;
            bckImage.color = new Color(0.2f, 0.1f, 0.1f, alpha);

            while (Mathf.Abs(alpha - alphaTarget) > .2f)
            {
                alpha = Mathf.Lerp(alpha, alphaTarget, Time.unscaledDeltaTime);
                alpha = Mathf.Clamp(alpha, 0f, 1.0f);
                bckImage.color = new Color(0.2f, 0.1f, 0.1f, alpha);
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            }

            buttons.SetActive(true);
            selectedButton.Select();
        }

    }
}