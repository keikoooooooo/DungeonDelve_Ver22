using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuController : Singleton<MenuController>
{
    public GameObject menuPanel;

    public PlayerController Player { get; private set; }

    
    private void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        Initialized();
    }
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        menuPanel.SetActive(!menuPanel.activeSelf);
        if (menuPanel.activeSelf)
        {
            OpenMenu();
            return;
        } 
        CloseMenu();
    }



    private void Initialized()
    {
        menuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OpenMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    private void CloseMenu()
    {
        Player.PlayerData.PlayerRenderTexture.CloseRenderUI();
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }


    
    
    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus && !menuPanel.activeSelf ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
