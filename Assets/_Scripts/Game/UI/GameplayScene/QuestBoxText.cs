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
    public bool IsReceivedQuest { get; private set; }
    /// <summary>
    /// Nhiệm vụ đã hoàn thành chưa ?
    /// </summary>
    public bool IsCompletedQuest { get; private set; }
    
    /// <summary>
    /// Nhiệm vụ có đang bị khóa ?
    /// </summary>
    public bool IsLocked { get; private set; }
    
    
    public event Action<QuestBoxText, QuestSetup> OnQuestSelectEvent;


    public void SetQuestBox(QuestSetup _questSetup)
    {
        questSetup = _questSetup;
        titleText.text = questSetup.GetTitle();
        IsCompletedQuest = _questSetup.GetCompletedQuest();
        IsLocked = _questSetup.IsLocked();
    }
    
    public void SelectQuest() =>  OnQuestSelectEvent?.Invoke(this, questSetup);
    public void TriggerBoxQuest() => GUI_Quest.IsTriggerQuestBox(this, true);
    public void NonTriggerBoxQuest() =>  GUI_Quest.IsTriggerQuestBox(this, false);
    public void OnAcceptQuest() 
    {
        iconAccept.enabled = true;
        IsReceivedQuest = true;
    }
    public void OnCancelQuest()
    {
        iconAccept.enabled = false;
        IsReceivedQuest = false;
    }
    

    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<QuestBoxText> ReleaseCallback { get; set; }
}
