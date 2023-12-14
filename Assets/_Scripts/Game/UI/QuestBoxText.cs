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
    
    public event Action<QuestBoxText, QuestSetup> OnQuestSelectEvent;


    public void SetBoxQuest(QuestSetup _questSetup)
    {
        questSetup = _questSetup;
        titleText.text = questSetup.GetTitle();
    }
    
    public void SelectQuest()
    {
        OnQuestSelectEvent?.Invoke(this, questSetup);
        iconAccept.enabled = true;
    }
    public void TriggerBoxQuest()
    {
        GUI_Quest.IsTriggerBoxQuest(this, true);
    }
    public void NonTriggerBoxQuest()
    {
        GUI_Quest.IsTriggerBoxQuest(this, false);
    }
    public void DeSelectQuest()
    {
        iconAccept.enabled = false;
    }

    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<QuestBoxText> ReleaseCallback { get; set; }
}
