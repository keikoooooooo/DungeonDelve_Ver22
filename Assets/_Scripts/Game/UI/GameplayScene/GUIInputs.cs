using UnityEngine.InputSystem;

public class GUIInputs : Singleton<GUIInputs>
{
    private Inputs _guiInput;
    public static bool CanInput { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        _guiInput = new Inputs();
    }
    
    private void OnEnable()
    {
        CanInput = true;
        _guiInput.Enable();

        _guiInput.UI.OpenMenu.started += OnOpenMenuPressed;
        _guiInput.UI.OpenMenu.canceled += OnOpenMenuPressed;
        
        _guiInput.UI.OpenBag.started += OnOpenBagPressed;
        _guiInput.UI.OpenBag.canceled += OnOpenBagPressed;
        
        _guiInput.UI.CollectItem.started += OnCollectItemPressed;
        _guiInput.UI.CollectItem.canceled += OnCollectItemPressed;
    }
    private void OnDisable()
    {
        _guiInput.UI.OpenMenu.started -= OnOpenMenuPressed;
        _guiInput.UI.OpenMenu.canceled -= OnOpenMenuPressed;
        
        _guiInput.UI.OpenBag.started -= OnOpenBagPressed;
        _guiInput.UI.OpenBag.canceled -= OnOpenBagPressed;       
        
        _guiInput.UI.CollectItem.started -= OnCollectItemPressed;
        _guiInput.UI.CollectItem.canceled -= OnCollectItemPressed;
        
        _guiInput.Disable();
    }


    /// <summary>
    /// Trả về TRUE nếu nhấn phím: Esc
    /// </summary>
    public static bool Esc;
    private static void OnOpenMenuPressed(InputAction.CallbackContext context) => Esc = context.ReadValueAsButton() && CanInput;
    

    /// <summary>
    /// Trả về TRUE nếu nhấn phím: B
    /// </summary>
    public static bool B;
    private static void OnOpenBagPressed(InputAction.CallbackContext context) => B = context.ReadValueAsButton() && CanInput;

    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: F
    /// </summary>
    public static bool F;
    private static void OnCollectItemPressed(InputAction.CallbackContext context) => F = context.ReadValueAsButton() && CanInput;
    
}
