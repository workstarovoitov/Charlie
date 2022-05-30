using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MenuManagement
{
    public class DialogueMenu : Menu<DialogueMenu>
    {
        [SerializeField] private GameObject opponentPage;
        [SerializeField] private Text opponentName;
        [SerializeField] private Text opponentReplics;
        [SerializeField] private Image opponentIcon;
        [SerializeField] private GameObject playerPage;
        [SerializeField] private Text playerName;
        [SerializeField] private Text playerReplics;
        [SerializeField] private Image playerIcon;

        [SerializeField] private GameObject acceptButton;
        [SerializeField] private GameObject declineButton;

        private DialogueItem dialogue;

        private int currentReplic = 0;
        private static ControllerTalker opponent;

        public static void SetTalker(ControllerTalker talker)
        {
            opponent = talker;
        }

        public override void ShowMenu()
        {
            selectedButton.Select();
            if (!opponent)
            {
                Debug.LogWarning("No opponent set!");
                return;
            }

            if (!IsMobileDevice())
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInput>().enabled = false;
            }
            dialogue = opponent.Dialogue[opponent.DialogueNum];
            currentReplic = 0;
            ShowReplic(currentReplic);
        }

        public void ShowReplic(int replicNum)
        {
            if (replicNum >= dialogue.Replics.Count)
            {
                OnDialogueComplete();
                return;
            }

            if (dialogue.Replics[replicNum].IsPlayerReplic)
            {
                playerPage.SetActive(true);
                opponentPage.SetActive(false);

                playerName.text = dialogue.Replics[replicNum].CharacterName;
                playerReplics.text = dialogue.Replics[replicNum].ReplicText;
                playerIcon.sprite = dialogue.Replics[replicNum].CharacterPortrait;
            }
            else
            {
                playerPage.SetActive(false);
                opponentPage.SetActive(true);

                opponentName.text = dialogue.Replics[replicNum].CharacterName;
                opponentReplics.text = dialogue.Replics[replicNum].ReplicText;
                opponentIcon.sprite = dialogue.Replics[replicNum].CharacterPortrait;
            }

            acceptButton.SetActive(dialogue.Replics[replicNum].IsAcceptBttnEnabled);
            declineButton.SetActive(dialogue.Replics[replicNum].IsDeclineBttnEnabled);

            acceptButton.GetComponentInChildren<Text>().text = dialogue.Replics[replicNum].AcceptBttnText;
            declineButton.GetComponentInChildren<Text>().text = dialogue.Replics[replicNum].DeclineBttnText;
        }

        public void OnButtonAccept()
        {
            ButtonClickSFX();
            currentReplic++;
            ShowReplic(currentReplic);
        }

        public void OnDialogueComplete()
        {
            opponent.FinishDialogue();
            if (!IsMobileDevice())
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInput>().enabled = true;
            }

            Time.timeScale = 1f;
            if (MenuManager.Instance != null)
            {
                MenuManager.Instance.CloseMenu();
            }
        }

        public override void OnBackPressed()
        {
            if (dialogue.Replics[currentReplic].IsDeclineBttnFinishDialogue)
            {
                OnDialogueComplete();
                return;
            }
            ButtonClickSFX();
            if (!IsMobileDevice())
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInput>().enabled = true;
            }

            Time.timeScale = 1f;
            if (MenuManager.Instance != null)
            {
                MenuManager.Instance.CloseMenu();
            }
        }
    }
}