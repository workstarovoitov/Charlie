using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

namespace MenuManagement
{
    public class PauseMenu : Menu<PauseMenu>
    {
        public static event Action OnSaveButton;

        [SerializeField] private Text inventoryPageName;

        [SerializeField] private GameObject itemDescriptionPage;
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemDescription;

        [SerializeField] private GameObject inventoryButtons;
        [SerializeField] private GameObject controlsButtons;
       
        [SerializeField] private GameObject potionUseButton;

        [Header("SFX")]
        [SerializeField] private string pauseSFX;
        [SerializeField] private string resumeSFX;


        private UnitInventoryManager inventory;

        private int currentInventoryPage = 0;
        private int currentItem = 0;
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
            actions.Menu.Pause.performed += _ => OnResume();
            actions.Menu.IncreasePage.performed += _ => OnChangePage(1);
            actions.Menu.DecreasePage.performed += _ => OnChangePage(-1);
            actions.Menu.ItemCloseButton.performed += _ => OnItemCloseButton();
            actions.Menu.ItemThrowButton.performed += _ => OnItemThrowButton();
            actions.Menu.FlaskUseButton.performed += _ => OnFlaskUseButton();
        }

        public override void ShowMenu()
        {
            selectedButton.Select();
            if (SceneManager.GetActiveScene().buildIndex == 0) return;
            ButtonSFX(pauseSFX);


            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!IsMobileDevice())
            {
                player.GetComponent<PlayerInput>().enabled = false;
            }
            inventory = player.GetComponent<UnitInventoryManager>();

            controlsButtons.SetActive(true);
            itemDescriptionPage.SetActive(false);
            currentInventoryPage = 0;
            ShowPage();
        }

        public void ShowPage()
        {
            inventoryPageName.text = inventory.UInventory.Pages[currentInventoryPage].InventoryPageName;
            if (inventory.UInventory.Pages[currentInventoryPage].ItemsThatFitsToPage == INVENTORYPAGESTYPE.POTION)
            {
                potionUseButton.SetActive(true);

            }
            else
            {
                potionUseButton.SetActive(false);
            }
            Button[] bttns;
            bttns = inventoryButtons.GetComponentsInChildren<Button>(true);

            for (int i = 0; i < bttns.Length; i++)
            {
                Image[] imgs = bttns[i].GetComponentsInChildren<Image>();
                if (i < inventory.UInventory.Pages[currentInventoryPage].SlotsInInventory)
                {
                    bttns[i].gameObject.SetActive(true);
                    imgs[0].sprite = inventory.UInventory.Pages[currentInventoryPage].ItemFrame;
                    imgs[1].enabled = false;
                    bttns[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    bttns[i].gameObject.SetActive(false);
                }

                if (i < inventory.UInventory.Pages[currentInventoryPage].Stack.Count)
                {
                    imgs[1].enabled = true;
                    imgs[1].sprite = inventory.UInventory.Pages[currentInventoryPage].Stack[i].Items.ItemSprite;
                    imgs[1].preserveAspect = true;
                    if (inventory.UInventory.Pages[currentInventoryPage].Stack[i].Items.StackCount > 1)
                    {
                        bttns[i].GetComponentInChildren<Text>().text = inventory.UInventory.Pages[currentInventoryPage].Stack[i].Quantity + "/" + inventory.UInventory.Pages[currentInventoryPage].Stack[i].Items.StackCount;
                    }
                }
            }
        }

        public void OnChangePage(int page)
        {
            ButtonClickSFX();
            currentInventoryPage += page;
            currentInventoryPage = (currentInventoryPage + inventory.UInventory.Pages.Count) % inventory.UInventory.Pages.Count;

            ShowPage();
        }

        public void OnItemChoose(int itemNum)
        {
            ButtonSelectSFX();
            itemDescriptionPage.SetActive(true);
            controlsButtons.SetActive(false);

            //return if empty button 
            if (itemNum + 1 > inventory.UInventory.Pages[currentInventoryPage].Stack.Count)
            {
                itemName.text = "";
                itemDescription.text = "";
                currentItem = -1;
                return;
            }

            currentItem = itemNum;
            itemName.text = inventory.UInventory.Pages[currentInventoryPage].Stack[itemNum].Items.ItemName;
            itemDescription.text = inventory.UInventory.Pages[currentInventoryPage].Stack[itemNum].Items.ItemDescription;
        }

        public override void OnDecreasePage()
        {
            OnChangePage(-1);
        }

        public override void OnIncreasePage()
        {
            OnChangePage(1);
        }
        public void OnItemThrowButton()
        {
            if (!itemDescriptionPage.activeSelf)
            {
                return;
            }
            ItemThrow();
        }
        public void OnFlaskUseButton()
        {
            if (!itemDescriptionPage.activeSelf )
            {
                return;
            }
            if (inventory.UInventory.Pages[currentInventoryPage].ItemsThatFitsToPage != INVENTORYPAGESTYPE.POTION)
            {
                return;
            }
            FlaskUse();
        }

        public void FlaskUse()
        {
            if (!itemDescriptionPage.activeSelf || currentItem < 0)
            {
                return;
            }
            inventory.ActivePotionIndex = currentItem;
            inventory.OnFlask();

            OnChangePage(0);
        }
        public void ItemThrow()
        {
            if (!itemDescriptionPage.activeSelf || currentItem < 0)
            {
                return;
            }
            if (currentItem == 0 && inventory.UInventory.Pages[currentInventoryPage].ItemsThatFitsToPage == INVENTORYPAGESTYPE.WEAPON) return;
            inventory.ThrowItem(currentInventoryPage, currentItem);

            OnChangePage(0);
        }


        public void OnRMSave()
        {
            Debug.Log("save file removed");
            ES3.DeleteFile("SaveFile.es3");
            ES3.DeleteFile("AutoSaveFile.es3");
            ES3.DeleteFile("MainSaveFile.es3");

            ES3.DeleteFile();


        }
        public void OnSave()
        {
            Debug.Log("save from menu");
            OnSaveButton?.Invoke();
        }

        public void OnResume()
        {
            OnBackPressed();
        }
       
        public void OnRestartPressed()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(ES3.Load<string>("savedScene"));
            base.OnBackPressed();
        }

        public void OnSettingsPressed()
        {
            ButtonClickSFX();
            SettingsMenu.Open();
        }

        public void OnMainMenuPressed()
        {
            Time.timeScale = 1;
            LevelLoader.LoadMainMenuLevel();
            MainMenu.Open();
        }
        public void OnItemCloseButton()
        {
            ButtonClickSFX();
            if (!itemDescriptionPage.activeSelf)
            {
                OnBackPressed();
                return;
            }
            controlsButtons.SetActive(true);
            itemDescriptionPage.SetActive(false);
        }

        public override void OnBackPressed()
        {
            ButtonClickSFX();
            ButtonSFX(resumeSFX);
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