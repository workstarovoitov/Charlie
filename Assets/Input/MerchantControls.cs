//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Input/MerchantControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @MerchantControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MerchantControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MerchantControls"",
    ""maps"": [
        {
            ""name"": ""Merchant"",
            ""id"": ""a4b1f384-f680-4bb8-b1bd-7950e5e468fc"",
            ""actions"": [
                {
                    ""name"": ""MerchantBuyButton"",
                    ""type"": ""Button"",
                    ""id"": ""f595f6d3-13f0-4115-9760-e94cc4c312f9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MerchantCloseButton"",
                    ""type"": ""Button"",
                    ""id"": ""c4211701-babe-48ec-b466-038f7f93d345"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MerchantExit"",
                    ""type"": ""Button"",
                    ""id"": ""5a9c7f39-d080-4285-9e88-fa3e5b7d4e03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6dda2e03-1cbf-4a67-9a41-e0e983d16987"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MerchantBuyButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b403f6b5-2dd2-4e8e-83b0-119bcdd95ead"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MerchantBuyButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d78ee92b-6792-4684-b32b-077db640942d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MerchantBuyButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8215e442-fe5b-4c1f-9eb3-76a07604df0c"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MerchantCloseButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""742d22c0-e567-4266-beaa-25545189da01"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MerchantCloseButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9b90ee90-9d27-4623-a4a0-3a08503d4dab"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MerchantExit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0eb9710-fc9f-497c-a1d0-f77a4cfc8280"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MerchantExit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""78e337db-6042-4afa-82d5-ce1edbe864c5"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MerchantExit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Merchant
        m_Merchant = asset.FindActionMap("Merchant", throwIfNotFound: true);
        m_Merchant_MerchantBuyButton = m_Merchant.FindAction("MerchantBuyButton", throwIfNotFound: true);
        m_Merchant_MerchantCloseButton = m_Merchant.FindAction("MerchantCloseButton", throwIfNotFound: true);
        m_Merchant_MerchantExit = m_Merchant.FindAction("MerchantExit", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Merchant
    private readonly InputActionMap m_Merchant;
    private IMerchantActions m_MerchantActionsCallbackInterface;
    private readonly InputAction m_Merchant_MerchantBuyButton;
    private readonly InputAction m_Merchant_MerchantCloseButton;
    private readonly InputAction m_Merchant_MerchantExit;
    public struct MerchantActions
    {
        private @MerchantControls m_Wrapper;
        public MerchantActions(@MerchantControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MerchantBuyButton => m_Wrapper.m_Merchant_MerchantBuyButton;
        public InputAction @MerchantCloseButton => m_Wrapper.m_Merchant_MerchantCloseButton;
        public InputAction @MerchantExit => m_Wrapper.m_Merchant_MerchantExit;
        public InputActionMap Get() { return m_Wrapper.m_Merchant; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MerchantActions set) { return set.Get(); }
        public void SetCallbacks(IMerchantActions instance)
        {
            if (m_Wrapper.m_MerchantActionsCallbackInterface != null)
            {
                @MerchantBuyButton.started -= m_Wrapper.m_MerchantActionsCallbackInterface.OnMerchantBuyButton;
                @MerchantBuyButton.performed -= m_Wrapper.m_MerchantActionsCallbackInterface.OnMerchantBuyButton;
                @MerchantBuyButton.canceled -= m_Wrapper.m_MerchantActionsCallbackInterface.OnMerchantBuyButton;
                @MerchantCloseButton.started -= m_Wrapper.m_MerchantActionsCallbackInterface.OnMerchantCloseButton;
                @MerchantCloseButton.performed -= m_Wrapper.m_MerchantActionsCallbackInterface.OnMerchantCloseButton;
                @MerchantCloseButton.canceled -= m_Wrapper.m_MerchantActionsCallbackInterface.OnMerchantCloseButton;
                @MerchantExit.started -= m_Wrapper.m_MerchantActionsCallbackInterface.OnMerchantExit;
                @MerchantExit.performed -= m_Wrapper.m_MerchantActionsCallbackInterface.OnMerchantExit;
                @MerchantExit.canceled -= m_Wrapper.m_MerchantActionsCallbackInterface.OnMerchantExit;
            }
            m_Wrapper.m_MerchantActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MerchantBuyButton.started += instance.OnMerchantBuyButton;
                @MerchantBuyButton.performed += instance.OnMerchantBuyButton;
                @MerchantBuyButton.canceled += instance.OnMerchantBuyButton;
                @MerchantCloseButton.started += instance.OnMerchantCloseButton;
                @MerchantCloseButton.performed += instance.OnMerchantCloseButton;
                @MerchantCloseButton.canceled += instance.OnMerchantCloseButton;
                @MerchantExit.started += instance.OnMerchantExit;
                @MerchantExit.performed += instance.OnMerchantExit;
                @MerchantExit.canceled += instance.OnMerchantExit;
            }
        }
    }
    public MerchantActions @Merchant => new MerchantActions(this);
    public interface IMerchantActions
    {
        void OnMerchantBuyButton(InputAction.CallbackContext context);
        void OnMerchantCloseButton(InputAction.CallbackContext context);
        void OnMerchantExit(InputAction.CallbackContext context);
    }
}
