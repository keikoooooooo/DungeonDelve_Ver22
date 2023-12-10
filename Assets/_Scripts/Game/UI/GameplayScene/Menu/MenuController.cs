using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MenuController : Singleton<MenuController>
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TextMeshProUGUI currencyText;
    
    [Space]
    public UnityEvent OnClickEscOpenMenuEvent;
    public UnityEvent OnClickBOpenMenuEvent;
    public UnityEvent OnCloseMenuEvent;
    
    private UserData _userData;
    private PlayerController _player;
    private GameManager _gameManager;
    
    private bool _isOpenMenu;
    private bool _isEventRegistered;

    
    private void OnEnable()
    {
        RegisterEvent();
    }
    private void Start()
    {
        _isOpenMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        UnRegisterEvent();
    }

    private void RegisterEvent()
    {
        GUIInputs.InputAction.UI.OpenMenu.performed += OpenMenu;
        GUIInputs.InputAction.UI.OpenBag.performed += OpenBag;

        _gameManager = GameManager.Instance;
        if(!_gameManager) return;
        GUI_Manager.SendRef(_gameManager);

        _player = _gameManager.Player;
        _userData = _gameManager.UserData;
        
        if (!_isEventRegistered)
        {
            _isEventRegistered = true;
            _userData.OnCoinChangedEvent += OnCoinChanged;
        }
        _userData.SendEventCoinChaged();
    }
    private void UnRegisterEvent()
    {
        GUIInputs.InputAction.UI.OpenMenu.performed -= OpenMenu;
        GUIInputs.InputAction.UI.OpenBag.performed -= OpenBag;
        
        if(!_isEventRegistered) return;
        _userData.OnCoinChangedEvent -= OnCoinChanged;
        _isEventRegistered = false;
    }
    
    
    private void OpenMenu(InputAction.CallbackContext _context)
    {
        _isOpenMenu = !_isOpenMenu;
        if(_isOpenMenu)
        {
            OpenMenu();
            OnClickEscOpenMenuEvent?.Invoke(); 
        }
        else
        {
            CloseMenu();
        }
    }
    private void OpenBag(InputAction.CallbackContext _context)
    {
        if(_isOpenMenu) return;
        _isOpenMenu = true;
        OpenMenu();
        OnClickBOpenMenuEvent?.Invoke(); 
    }
    private void OnCoinChanged(int _value) => currencyText.text = $"{_value}";
    
    
    private void OpenMenu()
    {
        GUI_Manager.UpdateGUIData();
        menuPanel.SetActive(true);
        _player.cinemachineFreeLook.enabled = false;
        _player.PlayerData.PlayerRenderTexture.OpenRenderUI(PlayerRenderTexture.RenderType.Character);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    public void CloseMenu()
    {
        Time.timeScale = 1;
        OnCloseMenuEvent?.Invoke();
        menuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        _player.cinemachineFreeLook.enabled = true;
        _player.PlayerData.PlayerRenderTexture.CloseRenderUI();
        _isOpenMenu = false;
    }
    

    public void ExitToLoginScene(string _sceneName)
    {
        Time.timeScale = 1;
        PlayFabController.Instance.ClearAccountTemp();
        LoadSceneManager.Instance.LoadScene(_sceneName);
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus && !_isOpenMenu ? CursorLockMode.Locked : CursorLockMode.None;
    }
    
}
