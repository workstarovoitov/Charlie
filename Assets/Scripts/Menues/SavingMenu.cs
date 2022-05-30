using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuManagement
{
    public class SavingMenu : Menu<SavingMenu>
    {
        [SerializeField] private Image bckImage;
        [SerializeField] private float speed;

        public override void ShowMenu()
        {
            StartCoroutine(SetFade());
        }

        IEnumerator SetFade()
        {
            float alpha = .0f;
            float alphaTarget = 1f;
            bckImage.color = new Color(0.2f, 0.1f, 0.1f, alpha);

            while (Mathf.Abs(alpha - alphaTarget) > .1f)
            {
                Time.timeScale = Mathf.Abs(1 - alpha);
                alpha = Mathf.Lerp(alpha, alphaTarget, speed);
                alpha = Mathf.Clamp(alpha, 0f, 1.0f);
                bckImage.color = new Color(0.2f, 0.1f, 0.1f, alpha);
                yield return new WaitForSecondsRealtime(0.0075f);
            }
            Time.timeScale = 1;
            GameMenu.Open();
        }

    }
}