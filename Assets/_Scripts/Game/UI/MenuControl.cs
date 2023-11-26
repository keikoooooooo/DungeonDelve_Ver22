using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuControl : Singleton<MenuControl>
{
    public GameObject menuPanel;
    
    [Space]
    public Animator characterAnimator;
    public Animator weaponAnimator;
    public Animator bagAnimator;
    public Animator settingAnimator;
    
    [Space]
    public GameObject characterPanel;
    public GameObject weaponPanel;
    public GameObject bagPanel;
    public GameObject settingPanel;
    

    public PlayerController Player { get; private set; }
    private readonly HashSet<Animator> _animators = new();
    private readonly HashSet<GameObject> _panels = new();
    
    public static readonly int NameHashIDSelected = Animator.StringToHash("Selected");
    public static readonly int NameHashIDTrigger = Animator.StringToHash("Trigger");
    public static readonly int NameHashIDNonTrigger = Animator.StringToHash("NonTrigger");
    public static readonly int NameHashIDDisable = Animator.StringToHash("Disable");

    private void Start()
    {
         //Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
         
        _animators.Add(characterAnimator);
        _animators.Add(weaponAnimator);
        _animators.Add(bagAnimator);
        _animators.Add(settingAnimator);

        _panels.Add(characterPanel);
        _panels.Add(weaponPanel);
        _panels.Add(bagPanel);
        _panels.Add(settingPanel);
        
        SelectButton(characterAnimator);
        SetActivePanel(characterPanel);
        menuPanel.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        menuPanel.SetActive(!menuPanel.activeSelf);
        
        var _type = menuPanel.activeSelf;
        if (!_type) return;
        
        
        SetActivePanel(characterPanel);
        foreach (var animator in _animators)
        {
            animator.Play(NameHashIDDisable);
        }
        characterAnimator.Play(NameHashIDSelected);
    }


    
    
    /// <summary>
    /// Set active object theo para và deActive các object khác
    /// </summary>
    /// <param name="_panelObject"> GameObject cần set active </param>
    public void SetActivePanel(GameObject _panelObject)
    {
        foreach (var panel in _panels)
        {
            panel.SetActive(_panelObject == panel);
        }
    }
    
    /// <summary>
    /// Chạy Animation của button vừa nhấn
    /// </summary>
    /// <param name="_animatorCheck"> Button cần chạy animation khi nhấn </param>
    public void SelectButton(Animator _animatorCheck)
    {
        foreach (var animator in _animators)
        {
            if(animator == _animatorCheck) animator.Play(NameHashIDSelected);
            else if(animator.IsTag("Selected")) animator.Play(NameHashIDNonTrigger);
        }
    }
    
    /// <summary>
    /// Chạy Animation của button vừa trigger với con trỏ chuột
    /// </summary>
    /// <param name="_animatorCheck"> Button cần chạy animation khi trigger </param>
    public void TriggerButton(Animator _animatorCheck)
    {
        foreach (var animator in _animators.Where(animator => animator == _animatorCheck && !animator.IsTag("Selected")))
        {
            animator.Play(NameHashIDTrigger);
        }
    }
    
    /// <summary>
    /// Chạy Animation của button không còn trigger với con trỏ chuột
    /// </summary>
    /// <param name="_animatorCheck"> Button cần chạy animation khi không còn trigger </param>
    public void NonTriggerButton(Animator _animatorCheck)
    {
        foreach (var animator in _animators.Where(animator => animator == _animatorCheck && !animator.IsTag("Selected")))
        {
            animator.Play(NameHashIDNonTrigger);
        }
    }
    
    
    
    /// <summary>
    /// Set active object theo para có trong danh sách, và deActive các object khác
    /// </summary>
    /// <param name="_panelObject"> GameObject cần set active </param>
    /// <param name="_panelList"> Danh sách cần kiểm tra </param>
    public static void SetActivePanel(GameObject _panelObject, HashSet<GameObject> _panelList)
    {
        foreach (var panel in _panelList)
        {
            panel.SetActive(_panelObject == panel);
        }
    }
    
    /// <summary>
    /// Chạy Animation của button vừa nhấn có trong danh sách
    /// </summary>
    /// <param name="_animatorCheck">  Button cần chạy animation khi nhấn  </param>
    /// <param name="_animatorList"> Danh sách cần kiểm tra </param>
    public static void SelectButton(Animator _animatorCheck, HashSet<Animator> _animatorList)
    {
        foreach (var animator in _animatorList)
        {
            if(animator == _animatorCheck) 
                animator.Play(NameHashIDSelected);
            else if(animator.IsTag("Selected")) 
                animator.Play(NameHashIDNonTrigger);
        }
    }
    
    /// <summary>
    /// Chạy Animation của button vừa trigger với con trỏ chuột trong danh sách
    /// </summary>
    /// <param name="_animatorCheck"> Button cần chạy animation khi trigger </param>
    /// <param name="_animatorList"> Danh sách cần kiểm tra </param>
    public static void TriggerButton(Animator _animatorCheck, HashSet<Animator> _animatorList)
    {
        foreach (var animator in _animatorList.Where(animator => animator == _animatorCheck && !animator.IsTag("Selected")))
        {
            animator.Play(NameHashIDTrigger);
        }
    }
    
    /// <summary>
    /// Chạy Animation của button không còn trigger với con trỏ chuột trong danh sách
    /// </summary>
    /// <param name="_animatorCheck"> Button cần chạy animation khi không còn trigger </param>
    /// <param name="_animatorList"> Danh sách cần kiểm tra </param>
    public static void NonTriggerButton(Animator _animatorCheck, HashSet<Animator> _animatorList)
    {
        foreach (var animator in _animatorList.Where(animator => animator == _animatorCheck && !animator.IsTag("Selected")))
        {
            animator.Play(NameHashIDNonTrigger);
        }
    }
    

}
