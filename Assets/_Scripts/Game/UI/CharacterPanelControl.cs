using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelControl : MonoBehaviour
{
    public Animator statsaAnimator;
    public Animator upgradeaAnimator;

    public GameObject statsPanel;
    public GameObject upgradePanel;
    
    private void OnEnable()
    {
        statsaAnimator.Play(MenuControl.NameHashIDSelected);
        upgradeaAnimator.Play(MenuControl.NameHashIDDisable);
        
        statsPanel.SetActive(true);
        upgradePanel.SetActive(false);
    }


    public void OnClickPanelStats()
    {
        statsaAnimator.Play(MenuControl.NameHashIDSelected);
        upgradeaAnimator.Play(MenuControl.NameHashIDNonTrigger);
        
        statsPanel.SetActive(true);
        upgradePanel.SetActive(false);
    }
    public void OnClickPanelUpgrade()
    {
        upgradeaAnimator.Play(MenuControl.NameHashIDSelected);
        statsaAnimator.Play(MenuControl.NameHashIDNonTrigger);
        
        statsPanel.SetActive(false);
        upgradePanel.SetActive(true);
    }
    
    
}
