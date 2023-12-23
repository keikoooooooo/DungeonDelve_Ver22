using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using QuestInGame;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviourID behaviourID;
    //
    public static event Action OnPanelOpenEvent;
    public static event Action OnPanelCloseEvent;
    //
    public static List<QuestSetup> QuestLists { get; } = new();
    
    public static int currentQuest { get; private set; } // Số lượng quest đang nhận.
    public static int maxQuest => 3; // Số lượng tối đa quest được nhận/1 ngày. 
    //
    private static readonly string _folderSave = "q_save";
    
    
    private void OnApplicationQuit() => PlayerPrefs.SetString(behaviourID.ID, DateTime.Now.ToString());
    private void Start()
    {
        var _quests = Resources.LoadAll<QuestSetup>("Quest Custom");
        QuestLists.Clear();
        foreach (var questSetup in _quests)
        {
            QuestLists.Add(Instantiate(questSetup));
        }

        currentQuest = 0;
        var _lastDay = DateTime.Parse(PlayerPrefs.GetString(behaviourID.ID, DateTime.Now.ToString()));
        if (_lastDay <= DateTime.Today)
            LoadNewQuest();
        else
            LoadOldQuest();
    }
    private static void LoadNewQuest()
    {
        foreach (var questSetup in QuestLists)
        {
            var _task = questSetup.GetTask();
            _task.SetCompleted(false);
            FileHandle.Delete(_folderSave, _task.GetID);
        }
    }
    private static void LoadOldQuest()
    {
        foreach (var questSetup in QuestLists)
        {
            var _task = questSetup.GetTask();
            var _checkQuest = FileHandle.Load(_folderSave, _task.GetID, out Task _taskSave);
            if (!_checkQuest) continue;
            
            _task.SetCompleted(_taskSave.IsCompleted);
            _task.SetState(_taskSave.IsLocked);
            _task.SetReceived(_taskSave.IsReceived);
            currentQuest++;
        }
    }
    
    
    public static void OnStartedQuest(QuestSetup _quest)
    {
        currentQuest = Mathf.Clamp(currentQuest + 1, 0, maxQuest);
        
        var _task = _quest.GetTask();
        _task.SetCompleted(false);
        _task.SetState(false);
        _task.SetReceived(true);
        FileHandle.Save(_task, _folderSave, _task.GetID);
    }
    public static void OnCompletedQuest(QuestSetup _quest)
    {
        _quest.GetReward().ForEach(x => RewardManager.Instance.SetReward(x));
        
        var _task = _quest.GetTask();
        _task.SetCompleted(true);
        _task.SetState(false);
        _task.SetReceived(true);
        FileHandle.Save(_task, _folderSave, _task.GetID);
        ClosePanel(default);
        AudioManager.PlayOneShot(FMOD_Events.Instance.rewards, Vector3.zero);
    }
    public static void OnCancelQuest(QuestSetup _quest)
    {
        var _task = _quest.GetTask();
        _task.SetCompleted(false);
        _task.SetState(true);
        _task.SetReceived(true);
        FileHandle.Save(_task, _folderSave, _task.GetID);
    }
    
    
    private static void OpenPanel(InputAction.CallbackContext _context) => OnPanelOpenEvent?.Invoke();
    public static void ClosePanel(InputAction.CallbackContext _context) => OnPanelCloseEvent?.Invoke();
    public void OnEnterPlayer()
    {
        GUI_Inputs.InputAction.UI.CollectItem.performed += OpenPanel;
        GUI_Inputs.InputAction.UI.OpenMenu.performed += ClosePanel;
        NoticeManager.Instance.CreateNoticeT3("[F] Open Quest.");
    }
    public void OnExitPlayer()
    {
        GUI_Inputs.InputAction.UI.CollectItem.performed -= OpenPanel;
        GUI_Inputs.InputAction.UI.OpenMenu.performed -= ClosePanel;
        NoticeManager.Instance.CloseNoticeT3();
    }
}
