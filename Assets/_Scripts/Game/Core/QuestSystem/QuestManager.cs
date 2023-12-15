using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviourID behaviourID;
    //
    public static event Action OnPanelOpenEvent;
    public static event Action OnPanelCloseEvent;
    //
    public static event Action<QuestSetup> OnQuestStartedEvent; 
    public static event Action<QuestSetup> OnQuestCompletedEvent; 
    public static event Action<QuestSetup> OnQuestCancelEvent; 
    public static List<QuestSetup> QuestLists { get; private set; }
    public static List<QuestSetup> QuestSelect { get; private set; } = new();
    public static int maxQuest { get; private set; } = 3; // Số lượng tối đa quest được nhận. 

    
    private void Start()
    {
        QuestLists = Resources.LoadAll<QuestSetup>("Quest Custom").ToList();
        RegisterEvent();
        
        var _lastDay = DateTime.Parse(PlayerPrefs.GetString(behaviourID.ID, DateTime.Now.ToString()));
        var _currentDay = DateTime.Today;
        if (_lastDay <= _currentDay)
        {
            ResetQuest();
        }
        else
        {
            CheckQuest();
        }
    }
    private void OnDestroy()
    {
        UnRegisterEvent();
    }
    
    
    private void RegisterEvent()
    {
        RewardManager.OnItemCollectionEvent += OnItemCollection;
        OnQuestStartedEvent += OnQuestStarted;
        OnQuestCompletedEvent += OnQuestCompleted;
        OnQuestCancelEvent += OnQuestCancel;
    }
    private void UnRegisterEvent()
    {
        RewardManager.OnItemCollectionEvent -= OnItemCollection;
        OnQuestStartedEvent -= OnQuestStarted;
        OnQuestCompletedEvent -= OnQuestCompleted;
        OnQuestCancelEvent -= OnQuestCancel;
    }
    private void ResetQuest()
    {
        foreach (var questSetup in QuestLists)
        {
            var _keyPP = behaviourID.ID + questSetup.GetIndex();
            if (PlayerPrefs.HasKey(_keyPP))
                PlayerPrefs.DeleteKey(_keyPP);
        }
    }
    private void CheckQuest()
    {
        List<QuestSetup> _questTemp = new();
        foreach (var questSetup in QuestLists)
        {
            var _keyPP = behaviourID.ID + questSetup.GetIndex();
            if (PlayerPrefs.HasKey(_keyPP)) continue;
            _questTemp.Add(questSetup);
        }
        QuestLists.Clear();
        QuestLists = _questTemp;
    }
    

    private void OpenPanel(InputAction.CallbackContext _context) => OnPanelOpenEvent?.Invoke();
    public static void ClosePanel() => OnPanelCloseEvent?.Invoke();
    
    public void OnEnterPlayer()
    {
        GUI_Inputs.InputAction.UI.CollectItem.performed += OpenPanel;
        NoticeManager.Instance.CreateNoticeT3("[F] Open Quest.");
    }
    public void OnExitPlayer()
    {
        GUI_Inputs.InputAction.UI.CollectItem.performed -= OpenPanel;
        NoticeManager.Instance.CloseNoticeT3();
    }

    
    private void OnItemCollection(ItemNameCode _itemNameCode)
    {
        Debug.Log("Reward: "+ _itemNameCode);
    } 
    private void OnQuestStarted(QuestSetup _questSetup)
    {
        if (!QuestSelect.Contains(_questSetup)) QuestSelect.Add(_questSetup);
    }
    private void OnQuestCompleted(QuestSetup _questSetup)
    {
        var _keyPP = behaviourID.ID + _questSetup.GetIndex();
        if (PlayerPrefs.HasKey(_keyPP)) PlayerPrefs.DeleteKey(_keyPP);
    }
    private void OnQuestCancel(QuestSetup _questSetup)
    {
        PlayerPrefs.SetInt(behaviourID.ID + _questSetup.GetIndex(), _questSetup.GetIndex());
        
        if (QuestLists.Contains(_questSetup)) QuestLists.Remove(_questSetup);
        if (QuestSelect.Contains(_questSetup)) QuestSelect.Remove(_questSetup);
        OnPanelOpenEvent?.Invoke();
    }
    
    
    public static void CallQuestStartedEvent(QuestSetup _questSetup) => OnQuestStartedEvent?.Invoke(_questSetup);
    public static void CallQuestCompletedEvent(QuestSetup _questSetup) => OnQuestCompletedEvent?.Invoke(_questSetup);
    public static void CallQuestCancelEvent(QuestSetup _questSetup) => OnQuestCancelEvent?.Invoke(_questSetup);
    
    private void OnApplicationQuit() => PlayerPrefs.SetString(behaviourID.ID, DateTime.Now.ToString());
}
