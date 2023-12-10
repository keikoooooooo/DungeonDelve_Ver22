public class GUIInputs : Singleton<GUIInputs>
{
    public static Inputs InputAction;
    
    protected override void Awake()
    {
        base.Awake();
        InputAction = new Inputs();
    }
    private void OnEnable() => EnableInput();
    private void OnDisable() => DisableInput();

    
    public static void EnableInput() => InputAction.Enable();
    public static void DisableInput() => InputAction.Disable();

}
