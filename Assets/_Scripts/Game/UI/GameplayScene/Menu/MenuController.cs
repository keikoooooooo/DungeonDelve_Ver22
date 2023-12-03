using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : Singleton<MenuController>
{
    [SerializeField] private TextMeshProUGUI currencyText;
    
    [Space]
    public UnityEvent OnClickEscOpenMenuEvent;
    public UnityEvent OnClickBOpenMenuEvent;
    public UnityEvent OnCloseMenuEvent;
    
    
    private bool isOpenMenu;
    private bool isEventRegistered;
    
    private PlayerController _player;
    private UserData _userData;
    
    
    
    private void OnEnable()
    {
        if(!GameManager.Instance) return;
        GUI_Manager.SendRef(GameManager.Instance);

        _player = GameManager.Instance.Player;
        _userData = GameManager.Instance.UserData;
        if (!isEventRegistered)
        {
            isEventRegistered = true;
            _userData.OnCoinChangedEvent += OnCoinChanged;
        }
        
        _userData.SendEventCoinChaged();
    }
    private void Start()
    {
        isOpenMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() => HandleInput();
    private void OnDisable()
    {
        if(!isEventRegistered) return;
        _userData.OnCoinChangedEvent -= OnCoinChanged;
    }

    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpenMenu = !isOpenMenu;
            if(isOpenMenu)
            {
                OnClickEscOpenMenuEvent?.Invoke();
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }

        if (!Input.GetKeyDown(KeyCode.B)) return;
        isOpenMenu = true;
        OnClickBOpenMenuEvent?.Invoke();
        OpenMenu();
    }
    private void OnCoinChanged(int _value) => currencyText.text = $"{_value}";

    
    
    private void OpenMenu()
    {
        GUI_Manager.UpdateGUIData();
        Cursor.lockState = CursorLockMode.None;
        _player.cinemachineFreeLook.enabled = false;
        Time.timeScale = 0;
    }
    public void CloseMenu()
    {
        OnCloseMenuEvent?.Invoke();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        _player.cinemachineFreeLook.enabled = true;
        _player.PlayerData.PlayerRenderTexture.CloseRenderUI();
    }


    public void ExitToLoginScene(string _sceneName)
    {
        Time.timeScale = 1;
        PlayFabController.Instance.ClearAccountTemp();
        LoadSceneManager.Instance.LoadScene(_sceneName);
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus && !isOpenMenu ? CursorLockMode.Locked : CursorLockMode.None;
    }
    
}
