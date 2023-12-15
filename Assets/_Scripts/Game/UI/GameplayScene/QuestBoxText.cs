using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoxText : MonoBehaviour, IPooled<QuestBoxText>
{
    public Animator animator;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image iconAccept;
    
    public QuestSetup questSetup { get; private set; }
    
    /// <summary>
    /// Có đang nhận nhiệm vụ này ?
    /// </summary>
    public bool IsAcceptQuest { get; private set; }
    
    public event Action<QuestBoxText, QuestSetup> OnQuestSelectEvent;


    public void SetQuestBox(QuestSetup _questSetup)
    {
        questSetup = _questSetup;
        titleText.text = questSetup.GetTitle();
    }
    
    public void SelectQuest() =>  OnQuestSelectEvent?.Invoke(this, questSetup);
    public void TriggerBoxQuest() => GUI_Quest.IsTriggerQuestBox(this, true);
    public void NonTriggerBoxQuest() =>  GUI_Quest.IsTriggerQuestBox(this, false);
    public void OnAcceptQuest() 
    {
        iconAccept.enabled = true;
        IsAcceptQuest = true;
    }
    public void OnCancelQuest()
    {
        iconAccept.enabled = false;
        IsAcceptQuest = false;
    }
    

    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<QuestBoxText> ReleaseCallback { get; set; }
}
