using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestBox : MonoBehaviour, IPooled<QuestBox>
{
    public Animator animator;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image iconAccept;
    public event Action<QuestBox> OnQuestSelectEvent;
    
    
    /// <summary> Thông tin quest mà box đang giữ </summary>
    public QuestSetup questSetup { get; private set; }
    
    /// <summary> Có đang nhận nhiệm vụ này ? </summary>
    public bool IsReceived { get; private set; }
    
    /// <summary> Nhiệm vụ đã hoàn thành chưa ? </summary>
    public bool IsCompleted { get; private set; }
    
    /// <summary> Nhiệm vụ có đang bị khóa ? </summary>
    public bool IsLocked { get; private set; }
    


    public void SetQuestBox(QuestSetup _questSetup)
    {
        questSetup = _questSetup;
        titleText.text = questSetup.GetTitle();

        var _task = _questSetup.GetTask();
        IsLocked = _task.IsLocked;
        IsReceived = _task.IsReceived;
        IsCompleted = _task.IsCompleted;
        
        if (IsReceived)
            OnAcceptQuest();
        else
            OnCancelQuest();
    }
    
    public void SelectQuest() =>  OnQuestSelectEvent?.Invoke(this);
    public void TriggerBoxQuest() => GUI_Quest.IsTriggerQuestBox(this, true);
    public void NonTriggerBoxQuest() =>  GUI_Quest.IsTriggerQuestBox(this, false);
    public void OnAcceptQuest() 
    {
        iconAccept.enabled = true;
        IsReceived = true;
    }
    public void OnCancelQuest()
    {
        iconAccept.enabled = false;
        IsReceived = false;
    }
    

    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<QuestBox> ReleaseCallback { get; set; }
}
