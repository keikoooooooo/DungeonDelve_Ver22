using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelControl : MonoBehaviour
{
    [Space]
    public Animator controlsAnimator;
    public Animator graphicsAnimator;
    public Animator audioAnimator;

    [Space]
    public GameObject controlPanel;
    public GameObject graphicsPanel;
    public GameObject audioPanel;
    
    private readonly HashSet<Animator> _animators = new();
    private readonly HashSet<GameObject> _panels = new();


    private void OnEnable()
    {
        if(_animators.Count != 0)
            SelectButton(controlsAnimator);
        
        if(_panels.Count != 0)
            SetActivePanel(controlPanel);
    }
    private void Start()
    {
        _animators.Add(controlsAnimator);
        _animators.Add(graphicsAnimator);
        _animators.Add(audioAnimator);

        _panels.Add(controlPanel);
        _panels.Add(graphicsPanel);
        _panels.Add(audioPanel);
    }

    

    public void SetActivePanel(GameObject _panelObject) => MenuControl.SetActivePanel(_panelObject, _panels);
    public void SelectButton(Animator _animatorCheck) => MenuControl.SelectButton(_animatorCheck, _animators);
    public void TriggerButton(Animator _animatorCheck) => MenuControl.TriggerButton(_animatorCheck, _animators);
    public void NonTriggerButton(Animator _animatorCheck) => MenuControl.NonTriggerButton(_animatorCheck, _animators);

}
