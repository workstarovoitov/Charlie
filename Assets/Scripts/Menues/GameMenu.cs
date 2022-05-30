using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace MenuManagement
{
    public class GameMenu : Menu<GameMenu>
    {     
        [SerializeField] private GameObject potionPC;
        [SerializeField] private BarControl playerHealth;
        [SerializeField] private BarControl playerStamina;

        [Header("Stick Settings")]
        [SerializeField] private GameObject stickControls;
        [SerializeField] private GameObject stickControlsSmall;
        [SerializeField] private GameObject stickControlsMedium;
        [SerializeField] private GameObject stickControlsLarge;

        [Header("Buttons Settings")]
        [SerializeField] private GameObject buttonControls;
        [SerializeField] private GameObject buttonControlsSmall;
        [SerializeField] private GameObject buttonControlsMedium;
        [SerializeField] private GameObject buttonControlsLarge;

        [Header("Boss Settings")]
        [SerializeField] private BarControl bossHealth1;
        public BarControl BossHealth1 => bossHealth1;
        [SerializeField] private BarControl bossHealth2;
        public BarControl BossHealth2 => bossHealth2;

        [SerializeField] private Text bossName;
        public Text BossName
        {
            get => bossName;
            set => bossName = value;
        }


        private GameObject player;

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
            //PlayerSpawner.OnPlayerSpawn += Initialize;
            PlayerDead.OnPlayerDead += LevelFail;
            ControllerTalker.OnDialogueStart += StartDialogue;
            ControllerMerchant.OnTradeStart += StartTrade;
            actions.Menu.Pause.performed += _ => OnPause();
        }

        public override void ShowMenu() 
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerHealth.SetTarget(player);
            playerStamina.SetTarget(player);
            TouchControlsEnable(IsMobileDevice());
            foreach(PotionsControl pc in GetComponentsInChildren<PotionsControl>())
            {
                pc.UpdateTarget();
            }
        }
 
        public void OnPause()
        {
            Time.timeScale = 0;
            PauseMenu.Open();
        }
                
        public void LevelFail()
        {
            StartCoroutine(OpenFailWindow());
        }
        
        public void StartDialogue(ControllerTalker talker)
        {
            Time.timeScale = 0;
            DialogueMenu.SetTalker(talker);
            DialogueMenu.Open();
        }

        public void StartTrade(ControllerMerchant talker)
        {
            Time.timeScale = 0;
            MerchantMenu.SetMerchant(talker);
            MerchantMenu.Open();
        }

        public void TouchControlsEnable(bool enable)
        {
            
            if (!enable)
            {
                potionPC.SetActive(true);
                stickControls.SetActive(false);
                buttonControls.SetActive(false);
                return;
            }

            potionPC.SetActive(false);

            SettingsMenu sm = FindObjectOfType<SettingsMenu>(true);
            string size = System.Enum.GetName(typeof(CONTROLSIZE), sm.CTRLSize);
            if (sm.StickEnabled)
            {
                stickControls.SetActive(true);
                buttonControls.SetActive(false);

                switch (size)
                {
                    case "Small":
                        stickControlsSmall.SetActive(true);
                        stickControlsMedium.SetActive(false);
                        stickControlsLarge.SetActive(false);
                        break;
                    case "Medium":
                        stickControlsSmall.SetActive(false);
                        stickControlsMedium.SetActive(true);
                        stickControlsLarge.SetActive(false);
                        break;
                    case "Large":
                        stickControlsSmall.SetActive(false);
                        stickControlsMedium.SetActive(false);
                        stickControlsLarge.SetActive(true);
                        break;
                }
            }
            else
            {
                stickControls.SetActive(false);
                buttonControls.SetActive(true);
                switch (size)
                {
                    case "Small":
                        buttonControlsSmall.SetActive(true);
                        buttonControlsMedium.SetActive(false);
                        buttonControlsLarge.SetActive(false);
                        break;
                    case "Medium":
                        buttonControlsSmall.SetActive(false);
                        buttonControlsMedium.SetActive(true);
                        buttonControlsLarge.SetActive(false);
                        break;
                    case "Large":
                        buttonControlsSmall.SetActive(false);
                        buttonControlsMedium.SetActive(false);
                        buttonControlsLarge.SetActive(true);
                        break;
                }

            }
        }
        
        IEnumerator OpenFailWindow()
        {
            yield return new WaitForSeconds(0.25f);
            Time.timeScale = 0;
            DefeatMenu.Open();
        }
    }
}
