using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;


public class SwitchPanelControl : MonoBehaviour
{
    
    public List<Animator> Animators;
    public List<GameObject> Panels;
    
    private static readonly int NameHashID_Selected = Animator.StringToHash("Selected");
    private static readonly int NameHashID_DeSelected = Animator.StringToHash("DeSelected");
    private static readonly int NameHashID_Trigger = Animator.StringToHash("Trigger");
    private static readonly int NameHashID_NonTrigger = Animator.StringToHash("NonTrigger");
    
    
    public void SetActivePanel(GameObject _panelObject)
    { 
        Panels.ForEach(panel => panel.SetActive(panel == _panelObject));
    }
    
    public void SelectButton(Animator _animatorCheck)
    {
        foreach (var animator in Animators)
        {
            if(animator == _animatorCheck)
                animator.Play(NameHashID_Selected);
            else if(animator.IsTag("Selected"))
                animator.Play(NameHashID_NonTrigger);
        }
    }
    
    public void DeSelectButton(Animator _animatorCheck)
    {
        foreach (var animator in Animators.Where(animator => animator == _animatorCheck && animator.IsTag("Selected")))
        {
            animator.Play(NameHashID_DeSelected);
        }
    }
    
    public void TriggerButton(Animator _animatorCheck)
    {
        foreach (var animator in Animators.Where(animator => animator == _animatorCheck && !animator.IsTag("Selected")))
        {
            animator.Play(NameHashID_Trigger);
        }
    }
    
    public void NonTriggerButton(Animator _animatorCheck)
    {
        foreach (var animator in Animators.Where(animator => animator == _animatorCheck && !animator.IsTag("Selected")))
        {
            animator.Play(NameHashID_NonTrigger);
        }
    }
}
