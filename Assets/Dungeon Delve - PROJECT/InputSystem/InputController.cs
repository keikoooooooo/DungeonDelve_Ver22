//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/FantasyProject/InputSystem/InputController.inputactions
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

public partial class @Inputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputController"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""21e5f8c8-35de-4373-a906-70413692dede"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""c8ebd47a-ab82-4d7d-95b7-45c10a04bb48"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SwitchMode"",
                    ""type"": ""Button"",
                    ""id"": ""cce6242e-239e-43d0-8bb5-b6ad2e0facd8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""88737262-ffa1-45a0-ab64-aec32e99978b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e1dcddb9-b8c3-4d2a-a0ae-6744efda8341"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""NormalAttack"",
                    ""type"": ""Button"",
                    ""id"": ""c5918dcc-406a-4ee1-8817-9ddfabe2ddbd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ElementalSkill"",
                    ""type"": ""Button"",
                    ""id"": ""447e42d5-6db5-43ac-b7c7-a0aa6cec0b92"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ElementalBurst"",
                    ""type"": ""Button"",
                    ""id"": ""da088eb4-35dd-495e-a9cc-8a4aa149a0be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""AWSD"",
                    ""id"": ""d81c5b47-0b14-41d9-81fb-ea7a377edaea"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""774b3eb5-02bd-4b4d-9bab-e897d0dab4d3"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2be8892d-28e2-4652-98ec-2335bedd13f1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""aceae3d1-a5cc-4e24-bae8-ace95e55034e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""54160f6b-164e-46cb-8afc-82b85283b652"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""53e4ff93-7b16-4e5c-b3e6-26c043f888b5"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0811d869-bfd3-4981-966c-d1d36ffcaf1b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f02d6c3-c8be-4c28-b04b-420e0e1152cd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20a97c9d-1cb6-4ebc-aa14-ff73b91823c0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""NormalAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f905aa21-2251-453e-9689-e39256454c2b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ElementalSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75773657-a6dd-421d-abdf-4e7132b4ff97"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ElementalBurst"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Two Modifiers"",
                    ""id"": ""8e4d6dff-9520-46e9-81c0-599a8fec7cd7"",
                    ""path"": ""TwoModifiers"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchMode"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier1"",
                    ""id"": ""1199beef-c255-4cb7-9fba-f8d6f61aa547"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SwitchMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""cc064da0-aa51-4f20-acf0-115bd6755cc0"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SwitchMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""modifier2"",
                    ""id"": ""8cfba0f1-2ccd-400a-8f7c-1ae236897f3b"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SwitchMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""864c58dc-af46-40ab-8a75-7c22d1c32ea1"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SwitchMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""a79ecc3d-9a69-44d7-9d4d-01a1018cda4f"",
            ""actions"": [
                {
                    ""name"": ""OpenMenu"",
                    ""type"": ""Button"",
                    ""id"": ""3c878113-ffc0-4574-bb21-66bb70f70cb0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OpenBag"",
                    ""type"": ""Button"",
                    ""id"": ""eacc5913-f847-4c26-80a9-65029fab0322"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CollectItem"",
                    ""type"": ""Button"",
                    ""id"": ""33f4dadf-a790-4125-95a4-45514e7fb867"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Item1"",
                    ""type"": ""Button"",
                    ""id"": ""5b9007aa-42d0-477d-a2fb-1050a7071e0d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Item2"",
                    ""type"": ""Button"",
                    ""id"": ""6dd50be5-1451-458a-9823-50092390128e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Item3"",
                    ""type"": ""Button"",
                    ""id"": ""34989309-9c7d-4689-97f2-f091cd79ec72"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Item4"",
                    ""type"": ""Button"",
                    ""id"": ""350a75f2-fe1a-4b2e-b833-e321885370d4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c48c1e2f-2c8a-4358-a3e9-eb4adad90333"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""OpenMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""621a7a25-de20-416c-8bef-359c3165ce31"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""OpenBag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""029d1000-f8bd-4b03-ac23-60640b89adce"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CollectItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b758bd0f-6d52-4f44-badd-4b2c5b3067a3"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Item1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eceb3d90-5b94-4570-b6ba-54457b6561e8"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Item2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fdda826-1ba4-4d1c-9162-cc2afb520445"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Item3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""caa5fa29-8b8c-4c0c-a0c2-aed6e18dc670"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Item4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""TESTER"",
            ""id"": ""b7a1f602-dd22-492e-ac1e-cf447dcbe5d5"",
            ""actions"": [
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""ac458292-4e34-4e62-8a1c-6ee88fef0b78"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ScalePanel"",
                    ""type"": ""Button"",
                    ""id"": ""2600b1b8-4d78-412b-bbe2-9764075ce4f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tab"",
                    ""type"": ""Button"",
                    ""id"": ""6ff5041e-d8a8-4477-8ea9-81b528007023"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c3e35aa7-cf3f-4520-9e6b-367d80630ecf"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ef2498f-20f8-4d99-bbcb-3ad958ee7d81"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ScalePanel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c885361-70c5-45b9-ae2f-43d167638019"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_SwitchMode = m_Player.FindAction("SwitchMode", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_NormalAttack = m_Player.FindAction("NormalAttack", throwIfNotFound: true);
        m_Player_ElementalSkill = m_Player.FindAction("ElementalSkill", throwIfNotFound: true);
        m_Player_ElementalBurst = m_Player.FindAction("ElementalBurst", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_OpenMenu = m_UI.FindAction("OpenMenu", throwIfNotFound: true);
        m_UI_OpenBag = m_UI.FindAction("OpenBag", throwIfNotFound: true);
        m_UI_CollectItem = m_UI.FindAction("CollectItem", throwIfNotFound: true);
        m_UI_Item1 = m_UI.FindAction("Item1", throwIfNotFound: true);
        m_UI_Item2 = m_UI.FindAction("Item2", throwIfNotFound: true);
        m_UI_Item3 = m_UI.FindAction("Item3", throwIfNotFound: true);
        m_UI_Item4 = m_UI.FindAction("Item4", throwIfNotFound: true);
        // TESTER
        m_TESTER = asset.FindActionMap("TESTER", throwIfNotFound: true);
        m_TESTER_Enter = m_TESTER.FindAction("Enter", throwIfNotFound: true);
        m_TESTER_ScalePanel = m_TESTER.FindAction("ScalePanel", throwIfNotFound: true);
        m_TESTER_Tab = m_TESTER.FindAction("Tab", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_SwitchMode;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_NormalAttack;
    private readonly InputAction m_Player_ElementalSkill;
    private readonly InputAction m_Player_ElementalBurst;
    public struct PlayerActions
    {
        private @Inputs m_Wrapper;
        public PlayerActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @SwitchMode => m_Wrapper.m_Player_SwitchMode;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @NormalAttack => m_Wrapper.m_Player_NormalAttack;
        public InputAction @ElementalSkill => m_Wrapper.m_Player_ElementalSkill;
        public InputAction @ElementalBurst => m_Wrapper.m_Player_ElementalBurst;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @SwitchMode.started += instance.OnSwitchMode;
            @SwitchMode.performed += instance.OnSwitchMode;
            @SwitchMode.canceled += instance.OnSwitchMode;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @NormalAttack.started += instance.OnNormalAttack;
            @NormalAttack.performed += instance.OnNormalAttack;
            @NormalAttack.canceled += instance.OnNormalAttack;
            @ElementalSkill.started += instance.OnElementalSkill;
            @ElementalSkill.performed += instance.OnElementalSkill;
            @ElementalSkill.canceled += instance.OnElementalSkill;
            @ElementalBurst.started += instance.OnElementalBurst;
            @ElementalBurst.performed += instance.OnElementalBurst;
            @ElementalBurst.canceled += instance.OnElementalBurst;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @SwitchMode.started -= instance.OnSwitchMode;
            @SwitchMode.performed -= instance.OnSwitchMode;
            @SwitchMode.canceled -= instance.OnSwitchMode;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @NormalAttack.started -= instance.OnNormalAttack;
            @NormalAttack.performed -= instance.OnNormalAttack;
            @NormalAttack.canceled -= instance.OnNormalAttack;
            @ElementalSkill.started -= instance.OnElementalSkill;
            @ElementalSkill.performed -= instance.OnElementalSkill;
            @ElementalSkill.canceled -= instance.OnElementalSkill;
            @ElementalBurst.started -= instance.OnElementalBurst;
            @ElementalBurst.performed -= instance.OnElementalBurst;
            @ElementalBurst.canceled -= instance.OnElementalBurst;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_OpenMenu;
    private readonly InputAction m_UI_OpenBag;
    private readonly InputAction m_UI_CollectItem;
    private readonly InputAction m_UI_Item1;
    private readonly InputAction m_UI_Item2;
    private readonly InputAction m_UI_Item3;
    private readonly InputAction m_UI_Item4;
    public struct UIActions
    {
        private @Inputs m_Wrapper;
        public UIActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @OpenMenu => m_Wrapper.m_UI_OpenMenu;
        public InputAction @OpenBag => m_Wrapper.m_UI_OpenBag;
        public InputAction @CollectItem => m_Wrapper.m_UI_CollectItem;
        public InputAction @Item1 => m_Wrapper.m_UI_Item1;
        public InputAction @Item2 => m_Wrapper.m_UI_Item2;
        public InputAction @Item3 => m_Wrapper.m_UI_Item3;
        public InputAction @Item4 => m_Wrapper.m_UI_Item4;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            @OpenMenu.started += instance.OnOpenMenu;
            @OpenMenu.performed += instance.OnOpenMenu;
            @OpenMenu.canceled += instance.OnOpenMenu;
            @OpenBag.started += instance.OnOpenBag;
            @OpenBag.performed += instance.OnOpenBag;
            @OpenBag.canceled += instance.OnOpenBag;
            @CollectItem.started += instance.OnCollectItem;
            @CollectItem.performed += instance.OnCollectItem;
            @CollectItem.canceled += instance.OnCollectItem;
            @Item1.started += instance.OnItem1;
            @Item1.performed += instance.OnItem1;
            @Item1.canceled += instance.OnItem1;
            @Item2.started += instance.OnItem2;
            @Item2.performed += instance.OnItem2;
            @Item2.canceled += instance.OnItem2;
            @Item3.started += instance.OnItem3;
            @Item3.performed += instance.OnItem3;
            @Item3.canceled += instance.OnItem3;
            @Item4.started += instance.OnItem4;
            @Item4.performed += instance.OnItem4;
            @Item4.canceled += instance.OnItem4;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            @OpenMenu.started -= instance.OnOpenMenu;
            @OpenMenu.performed -= instance.OnOpenMenu;
            @OpenMenu.canceled -= instance.OnOpenMenu;
            @OpenBag.started -= instance.OnOpenBag;
            @OpenBag.performed -= instance.OnOpenBag;
            @OpenBag.canceled -= instance.OnOpenBag;
            @CollectItem.started -= instance.OnCollectItem;
            @CollectItem.performed -= instance.OnCollectItem;
            @CollectItem.canceled -= instance.OnCollectItem;
            @Item1.started -= instance.OnItem1;
            @Item1.performed -= instance.OnItem1;
            @Item1.canceled -= instance.OnItem1;
            @Item2.started -= instance.OnItem2;
            @Item2.performed -= instance.OnItem2;
            @Item2.canceled -= instance.OnItem2;
            @Item3.started -= instance.OnItem3;
            @Item3.performed -= instance.OnItem3;
            @Item3.canceled -= instance.OnItem3;
            @Item4.started -= instance.OnItem4;
            @Item4.performed -= instance.OnItem4;
            @Item4.canceled -= instance.OnItem4;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);

    // TESTER
    private readonly InputActionMap m_TESTER;
    private List<ITESTERActions> m_TESTERActionsCallbackInterfaces = new List<ITESTERActions>();
    private readonly InputAction m_TESTER_Enter;
    private readonly InputAction m_TESTER_ScalePanel;
    private readonly InputAction m_TESTER_Tab;
    public struct TESTERActions
    {
        private @Inputs m_Wrapper;
        public TESTERActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Enter => m_Wrapper.m_TESTER_Enter;
        public InputAction @ScalePanel => m_Wrapper.m_TESTER_ScalePanel;
        public InputAction @Tab => m_Wrapper.m_TESTER_Tab;
        public InputActionMap Get() { return m_Wrapper.m_TESTER; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TESTERActions set) { return set.Get(); }
        public void AddCallbacks(ITESTERActions instance)
        {
            if (instance == null || m_Wrapper.m_TESTERActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TESTERActionsCallbackInterfaces.Add(instance);
            @Enter.started += instance.OnEnter;
            @Enter.performed += instance.OnEnter;
            @Enter.canceled += instance.OnEnter;
            @ScalePanel.started += instance.OnScalePanel;
            @ScalePanel.performed += instance.OnScalePanel;
            @ScalePanel.canceled += instance.OnScalePanel;
            @Tab.started += instance.OnTab;
            @Tab.performed += instance.OnTab;
            @Tab.canceled += instance.OnTab;
        }

        private void UnregisterCallbacks(ITESTERActions instance)
        {
            @Enter.started -= instance.OnEnter;
            @Enter.performed -= instance.OnEnter;
            @Enter.canceled -= instance.OnEnter;
            @ScalePanel.started -= instance.OnScalePanel;
            @ScalePanel.performed -= instance.OnScalePanel;
            @ScalePanel.canceled -= instance.OnScalePanel;
            @Tab.started -= instance.OnTab;
            @Tab.performed -= instance.OnTab;
            @Tab.canceled -= instance.OnTab;
        }

        public void RemoveCallbacks(ITESTERActions instance)
        {
            if (m_Wrapper.m_TESTERActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITESTERActions instance)
        {
            foreach (var item in m_Wrapper.m_TESTERActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TESTERActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TESTERActions @TESTER => new TESTERActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSwitchMode(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnNormalAttack(InputAction.CallbackContext context);
        void OnElementalSkill(InputAction.CallbackContext context);
        void OnElementalBurst(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnOpenMenu(InputAction.CallbackContext context);
        void OnOpenBag(InputAction.CallbackContext context);
        void OnCollectItem(InputAction.CallbackContext context);
        void OnItem1(InputAction.CallbackContext context);
        void OnItem2(InputAction.CallbackContext context);
        void OnItem3(InputAction.CallbackContext context);
        void OnItem4(InputAction.CallbackContext context);
    }
    public interface ITESTERActions
    {
        void OnEnter(InputAction.CallbackContext context);
        void OnScalePanel(InputAction.CallbackContext context);
        void OnTab(InputAction.CallbackContext context);
    }
}
