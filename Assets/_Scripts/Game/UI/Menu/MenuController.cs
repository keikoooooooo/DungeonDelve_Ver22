using UnityEngine;
using UnityEngine.Events;

public class MenuController : Singleton<MenuController>
{
    public UnityEvent OnMenuOpenEvent;
    public UnityEvent OnMenuCloseEvent;
    private bool isOpenMenu;
    
    public PlayerController Player { get; private set; }

    
    private void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        Initialized();
    }
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        isOpenMenu = !isOpenMenu;
        if(isOpenMenu) OpenMenu();
        else           CloseMenu();
    }
    

    private void Initialized()
    {
        isOpenMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OpenMenu()
    {
        OnMenuOpenEvent?.Invoke();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        Player.cinemachineFreeLook.enabled = false;
    }
    public void CloseMenu()
    {
        OnMenuCloseEvent?.Invoke();
        Player.cinemachineFreeLook.enabled = true;
        Player.PlayerData.PlayerRenderTexture.CloseRenderUI();
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    
    
    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus && !isOpenMenu ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
