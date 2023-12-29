using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DungeonDelve.Project;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] private MonoBehaviourID behaviourID;
    [field: SerializeField] public InteractiveUI interactiveUI { get; private set; }
    public static List<QuestSetup> QuestLists { get; } = new();
    
    public static int currentQuest { get; private set; } // Số lượng quest đang nhận.
    public static int maxQuest => 3; // Số lượng tối đa quest được nhận/1 ngày. 
    //
    private static readonly string _folderSave = "q_save";
    
    
    private void OnApplicationQuit() => PlayerPrefs.SetString(behaviourID.GetID, DateTime.Now.ToString());
    private void Start()
    {
        var _quests = Resources.LoadAll<QuestSetup>("Quest Custom");
        QuestLists.Clear();
        foreach (var questSetup in _quests)
        {
            QuestLists.Add(Instantiate(questSetup));
        }

        currentQuest = 0;
        var _lastDay = DateTime.Parse(PlayerPrefs.GetString(behaviourID.GetID, DateTime.MinValue.ToString()));
        if (_lastDay < DateTime.Today)
            LoadNewQuest(); 
        else
            LoadOldQuest();
    }
    private static void LoadNewQuest()
    {
        var _tasks = QuestLists.Select(x => x.GetTask());
        foreach (var _task in _tasks)
        {
            _task.SetCompleted(false);
            FileHandle.Delete(_folderSave, _task.GetID);
        }
        NoticeManager.Instance.OpenNewQuestNoticePanelT4();
    }
    private static void LoadOldQuest()
    {
        foreach (var questSetup in QuestLists)
        {
            var _task = questSetup.GetTask();
            var _checkFile = FileHandle.Load(_folderSave, _task.GetID, out Task _taskSave);
            if (!_checkFile) continue;
            
            _task.SetCompleted(_taskSave.IsCompleted);
            _task.SetTaskLocked(_taskSave.IsLocked);
            _task.SetReceived(_taskSave.IsReceived);
            currentQuest++;
        }
    }
    
    
    public static void OnStartedQuest(QuestSetup _quest)
    {
        currentQuest = Mathf.Clamp(currentQuest + 1, 0, maxQuest);
        
        var _task = _quest.GetTask();
        _task.SetCompleted(false);
        _task.SetTaskLocked(false);
        _task.SetReceived(true);
        FileHandle.Save(_task, _folderSave, _task.GetID);
    }
    public void OnCompletedQuest(QuestSetup _quest)
    {
        interactiveUI.ClosePanel(default);
        _quest.GetReward().ForEach(x => RewardManager.Instance.SetReward(x));
        var _task = _quest.GetTask();
        _task.SetCompleted(true);
        _task.SetTaskLocked(false);
        _task.SetReceived(true);
        FileHandle.Save(_task, _folderSave, _task.GetID);
        AudioManager.PlayOneShot(FMOD_Events.Instance.rewards_01, Vector3.zero);
    }
    public static void OnCancelQuest(QuestSetup _quest)
    {
        var _task = _quest.GetTask();
        _task.SetCompleted(false);
        _task.SetTaskLocked(true);
        _task.SetReceived(true);
        FileHandle.Save(_task, _folderSave, _task.GetID);
    }


#if UNITY_EDITOR
    [ContextMenu("Reset Quest Key")]
    private void OnResetQuestKey()
    {
        if (!PlayerPrefs.HasKey(behaviourID.GetID)) return;
        PlayerPrefs.DeleteKey(behaviourID.GetID);
        Debug.Log("Delete PlayerPrefs Key Success !. \nKey: " + behaviourID.GetID);
    }
#endif
    
    
}
