using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : Singleton<MenuController>
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TextMeshProUGUI currencyText;
    
    [Space]
    public UnityEvent OnClickEscOpenMenuEvent;
    public UnityEvent OnClickBOpenMenuEvent;
    public UnityEvent OnCloseMenuEvent;
    
    private bool _isOpenMenu;
    private bool _isEventRegistered;
    private PlayerController _player;
    private UserData _userData;

    
    private void OnEnable()
    {
        if(!GameManager.Instance) return;
        GUI_Manager.SendRef(GameManager.Instance);

        _player = GameManager.Instance.Player;
        _userData = GameManager.Instance.UserData;
        if (!_isEventRegistered)
        {
            _isEventRegistered = true;
            _userData.OnCoinChangedEvent += OnCoinChanged;
        }
        
        _userData.SendEventCoinChaged();
    }
    private void Start()
    {
        _isOpenMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() => HandleInput();
    private void OnDisable()
    {
        if(!_isEventRegistered) return;
        _userData.OnCoinChangedEvent -= OnCoinChanged;
        _isEventRegistered = false;
    }
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

        if (!Input.GetKeyDown(KeyCode.B) || _isOpenMenu) return;
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
