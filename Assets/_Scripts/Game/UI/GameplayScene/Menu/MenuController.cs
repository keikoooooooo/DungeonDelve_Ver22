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
    private SO_GameItemData _gameItemData;
    private SO_CharacterUpgradeData _characterUpgradeData;
    
    
    
    private void OnEnable()
    {
        if(!GameManager.Instance) return;

        _player = GameManager.Instance.player;
        _userData = GameManager.Instance.UserData;
        _gameItemData = GameManager.Instance.GameItemData;
        _characterUpgradeData = GameManager.Instance.CharacterUpgradeData;
        
        if (!isEventRegistered)
        {
            isEventRegistered = true;
            _userData.OnCoinChangedEvent += OnCoinChanged;
        }
        GameManager.Instance.UserData.SendEventCoinChaged();
    }
    private void Start()
    {
        isOpenMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
        GUI_Manager.SendRef(_userData, _characterUpgradeData, _gameItemData, _player);
        _userData.SendEventCoinChaged();
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
