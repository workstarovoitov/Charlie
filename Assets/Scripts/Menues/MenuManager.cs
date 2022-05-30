using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace MenuManagement
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenuPrefab;
        [SerializeField] private SettingsMenu settingsMenuPrefab;
        [SerializeField] private GameMenu gameMenuPrefab;
        [SerializeField] private PauseMenu pauseMenuPrefab;
        [SerializeField] private MerchantMenu merchantMenuPrefab;
        [SerializeField] private DialogueMenu dialogueMenuPrefab;
        [SerializeField] private SavingMenu savingMenuPrefab;
        [SerializeField] private LoadingMenu loadingMenuPrefab;
        [SerializeField] private DefeatMenu defeatMenuPrefab;
        [SerializeField] private EndMenu endMenuPrefab;

        [SerializeField] private Transform menuParent;

        //TODO: change stack to list
        private Stack<Menu> menuStack = new Stack<Menu>();


        private static MenuManager instance;

        public static MenuManager Instance { get { return instance; } }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                InitializeMenues();
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        private void InitializeMenues()
        {
            if (menuParent == null)
            {
                GameObject menuParentObject = new GameObject("Menus");
                menuParent = menuParentObject.transform;
            }

            DontDestroyOnLoad(menuParent.gameObject);


            BindingFlags myFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            FieldInfo[] fields = this.GetType().GetFields(myFlags);


            foreach (FieldInfo field in fields)
            {
                Menu prefab = field.GetValue(this) as Menu;

                if (prefab != null)
                {
                    Menu menuInstance = Instantiate(prefab, menuParent);
                    menuInstance.gameObject.SetActive(false);

                    if (prefab == mainMenuPrefab && SceneManager.GetActiveScene().buildIndex == 0) 
                    {
                        OpenMenu(menuInstance);
                    }
                    if (prefab == gameMenuPrefab && SceneManager.GetActiveScene().buildIndex != 0) 
                    {
                        OpenMenu(menuInstance);
                    }
                }
            }
        }

        public void OpenMenu(Menu menuInstance)
        {
            if (menuInstance == null)
            {
                Debug.Log("MENUMANAGER OpenMenu ERROR: invalid menu");
                return;
            }

            if (menuStack.Count > 0)
            {
                foreach (Menu menu in menuStack)
                {
                    menu.gameObject.SetActive(false);
                }
            }

            menuInstance.gameObject.SetActive(true);
            menuInstance.ShowMenu();

            menuStack.Push(menuInstance);
        }

        public void CloseMenu()
        {
            if (menuStack.Count == 0)
            {
                Debug.LogWarning("MENUMANAGER CloseMenu ERROR: No menus in stack!");
                return;
            }

            Menu topMenu = menuStack.Pop();
            topMenu.gameObject.SetActive(false);

            if (menuStack.Count > 0)
            {
                Menu nextMenu = menuStack.Peek();
                nextMenu.gameObject.SetActive(true);
                nextMenu.ShowMenu();
            }
        }
       
    }
}