using UnityEngine;
using UnityEngine.UI;

namespace MenuManagement
{
    public abstract class Menu<T> : Menu where T : Menu<T>
    {
        private static T _instance;
        public static T Instance { get { return _instance; } }

        public NewControls actions;

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = (T)this;
                actions = new NewControls();
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        public static void Open()
        {
            if (MenuManager.Instance != null && Instance != null)
            {
                MenuManager.Instance.OpenMenu(Instance);
            }
        }
    }

    [RequireComponent(typeof(Canvas))]
    public abstract class Menu : MonoBehaviour
    {
        [SerializeField] private string buttonPressSFX;
        [SerializeField] private string buttonSelectSFX;
        [SerializeField] public Button selectedButton;
       
        public bool IsMobileDevice()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                return true;
            }
            return false;
        }
        
        public void ButtonClickSFX()
        {
            if (string.IsNullOrEmpty(buttonPressSFX)) return;
            FMODUnity.RuntimeManager.PlayOneShot(buttonPressSFX, transform.position);
        }

        public void ButtonSelectSFX()
        {
            if (string.IsNullOrEmpty(buttonSelectSFX)) return;
            FMODUnity.RuntimeManager.PlayOneShot(buttonSelectSFX, transform.position);
        }

        public void ButtonSFX(string sfx)
        {
            if (string.IsNullOrEmpty(sfx)) return;
            FMODUnity.RuntimeManager.PlayOneShot(sfx, transform.position);
        }

        public virtual void OnDecreasePage()
        {

        }

        public virtual void OnIncreasePage()
        {

        }

        public virtual void OnBackPressed()
        {
            ButtonClickSFX();
            if (MenuManager.Instance != null)
            {
                MenuManager.Instance.CloseMenu();
            }
        }
        
        public abstract void ShowMenu();
    }

    public enum CONTROLSIZE
    {
        Small = 0,
        Medium = 1,
        Large = 2
    }
}
