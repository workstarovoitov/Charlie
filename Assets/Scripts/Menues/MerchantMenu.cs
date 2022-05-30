using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MenuManagement
{
    public class MerchantMenu : Menu<MerchantMenu>
    {
        [SerializeField] private Text storeName;
       
        [SerializeField] private GameObject storeDescriptionPage;
        [SerializeField] private Text storeDescription;

        [SerializeField] private GameObject itemDescriptionPage;
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemDescription;
        

        [SerializeField] private GameObject inventoryButtons;

        [SerializeField] private Text itemPrice;
        [SerializeField] private Text amount;

        [Header("SFX")]
        [SerializeField] private string pauseSFX;
        [SerializeField] private string resumeSFX;
        [SerializeField] private string notEnoughMoneySFX;
        [SerializeField] private string buySFX;



        private int currentItem;
        private MerchantInventory merchantInventory;
        private static ControllerMerchant merchant;
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
            actions.Menu.ItemCloseButton.performed += _ => OnItemCloseButton();
            actions.Menu.MerchantBuyButton.performed += _ => OnMerchantBuyButton();
        }

        public static void SetMerchant(ControllerMerchant cm)
        {
            merchant = cm;
        }
        
        public override void ShowMenu()
        {
            selectedButton.Select();
            if (!merchant)
            {
                Debug.LogWarning("No merchant set!");
                return;
            }
            if (!IsMobileDevice())
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerInput>().enabled = false;
            }

            merchantInventory = merchant.MInventory;

            ButtonSFX(pauseSFX);

            storeDescriptionPage.SetActive(true);
            itemDescriptionPage.SetActive(false);
            UpdateMerchantPage();
        }

        public void UpdateMerchantPage()
        {
            storeName.text = merchantInventory.StoreName;
            storeDescription.text = merchantInventory.StoreDescription;

            amount.text = merchant.GetPlayerAmount() + "";

            Button[] bttns;
            bttns = inventoryButtons.GetComponentsInChildren<Button>(true);

            for (int i = 0; i < bttns.Length; i++)
            {
                bttns[i].GetComponentInChildren<Text>().text = "";
                Image[] imgs = bttns[i].GetComponentsInChildren<Image>();
                if (i < merchantInventory.SlotsInInventory)
                {
                    bttns[i].gameObject.SetActive(true);
                    imgs[0].sprite = merchantInventory.ItemFrame;
                    imgs[1].enabled = false;
                }
                else
                {
                    imgs[0].sprite = null;
                    imgs[1].sprite = null;
                    bttns[i].gameObject.SetActive(false);
                }

                if (i < merchantInventory.Stack.Count)
                {
                    imgs[1].enabled = true;
                    imgs[1].sprite = merchantInventory.Stack[i].Items.ItemSprite;
                    imgs[1].preserveAspect = true;
                    if (merchantInventory.Stack[i].Quantity > 1)
                    {
                        bttns[i].GetComponentInChildren<Text>().text = merchantInventory.Stack[i].Quantity + "";
                    }
                }
            }
        }

        public void OnItemChoose(int itemNum)
        {
            ButtonSelectSFX();
            storeDescriptionPage.SetActive(false);
            itemDescriptionPage.SetActive(true);

            //return if empty button 
            if (itemNum + 1 > merchantInventory.Stack.Count)
            {
                itemName.text = "";
                itemDescription.text = "";
                itemPrice.text = "";

                return;
            }
            currentItem = itemNum;

            itemName.text = merchantInventory.Stack[currentItem].Items.ItemName;
            itemDescription.text = merchantInventory.Stack[currentItem].Items.ItemDescription;
            itemPrice.text = merchantInventory.Stack[currentItem].Price + "";
        }

        public void OnMerchantBuyButton()
        {
            if (!itemDescriptionPage.activeSelf)
            {
                return;
            }
            ItemBuy();
        }

        public void ItemBuy()
        {
            if (merchant.SellItem(currentItem))
            {
                ButtonSFX(buySFX);
                UpdateMerchantPage();
                OnItemChoose(currentItem);
            }
            else
            {
                ButtonSFX(notEnoughMoneySFX);
            }
        }
       
        public void OnItemCloseButton()
        {
            ButtonClickSFX();
            if (!itemDescriptionPage.activeSelf)
            {
                OnBackPressed();
                return;
            }
            storeDescriptionPage.SetActive(true);
            itemDescriptionPage.SetActive(false);
        }
        
        public void OnResume()
        {
            OnBackPressed();
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