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
    public static List<QuestSetup> QuestSelected { get; private set; } = new();
    public static int maxQuest { get; private set; } = 3; // Số lượng tối đa quest được nhận. 
    //
    private readonly string _questFolderSave = "qs_";
    
    
    [Serializable]
    public class QuestSave
    {
        [SerializeField, Tooltip("ID nhiệm vụ")] 
        private string id;
        [SerializeField, Tooltip("Task đã hoàn thành chưa ?")] 
        private bool isCompleted;
        
        [SerializeField, Tooltip("Task có đang bị khóa không ?")] 
        private bool isLocked;
        public QuestSave() { }
        
        /// <summary>
        /// Tạo 1 Data để lưu trữ thông tin về Task/Quest đang nhận
        /// </summary>
        /// <param name="_id"> ID Quest </param>
        /// <param name="_isCompletedQuest"> Task đã hoàn thành chưa ? </param>
        public QuestSave(string _id, bool _isCompletedQuest, bool _isLocked)
        {
            id = _id;
            isCompleted = _isCompletedQuest;
            isLocked = _isLocked;
        }
        public string GetID => id;
        
        /// <summary>
        /// Trạng thái của Quest: Hoàn thành/Chưa hoàn thành
        /// </summary>
        public bool GetIsCompleted => isCompleted;
        
        /// <summary>
        /// Trạng thái của Quest: Bị khóa/Không bị khóa
        /// </summary>
        public bool GetQuestState => isLocked;
    }
    
    
    private void Start()
    {
        QuestLists = Resources.LoadAll<QuestSetup>("Quest Custom").ToList();
        RegisterEvent();
        
        var _lastDay = DateTime.Parse(PlayerPrefs.GetString(behaviourID.ID, DateTime.Now.ToString()));
        if (_lastDay <= DateTime.Today)
            LoadNewQuest();
        else
            LoadOldQuest();
    }
    private void OnDestroy()
    {
        UnRegisterEvent();
    }
    private void OnApplicationQuit() => PlayerPrefs.SetString(behaviourID.ID, DateTime.Now.ToString());

    
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
    private void LoadNewQuest()
    {
        foreach (var questSetup in QuestLists)
        {
            questSetup.SetCompletedQuest(false);
            FileHandle.Delete(_questFolderSave, questSetup.GetIDQuest());
        }
    }
    private void LoadOldQuest()
    {
        QuestSelected.Clear();
        foreach (var questSetup in QuestLists)
        {
            var _checkQuest = FileHandle.Load(_questFolderSave, questSetup.GetIDQuest(), out QuestSave _questSave);
            if (!_checkQuest)  continue;
            questSetup.SetCompletedQuest(_questSave.GetIsCompleted);
            questSetup.SetQuestState(_questSave.GetQuestState);
            QuestSelected.Add(questSetup);
        }
    }
    
    
    
    private void OnItemCollection(ItemNameCode _itemNameCode)
    {
        Debug.Log("Reward: "+ _itemNameCode);
    } 
    private void OnQuestStarted(QuestSetup _questSetup)
    {
        FileHandle.Save(new QuestSave(_questSetup.GetIDQuest(), false, false), _questFolderSave, _questSetup.GetIDQuest());
    }
    private void OnQuestCompleted(QuestSetup _questSetup)
    {
        _questSetup.SetCompletedQuest(true);
        var _questSave = new QuestSave(_questSetup.GetIDQuest(), true, false);
        FileHandle.Save(_questSave, _questFolderSave, _questSetup.GetIDQuest());
    }
    private void OnQuestCancel(QuestSetup _questSetup)
    {
        var _questSave = new QuestSave(_questSetup.GetIDQuest(), false, true);
        FileHandle.Save(_questSave, _questFolderSave, _questSetup.GetIDQuest());
    }
    //
    public static void CallQuestStartedEvent(QuestSetup _questSetup) => OnQuestStartedEvent?.Invoke(_questSetup);
    public static void CallQuestCompletedEvent(QuestSetup _questSetup) => OnQuestCompletedEvent?.Invoke(_questSetup);
    public static void CallQuestCancelEvent(QuestSetup _questSetup) => OnQuestCancelEvent?.Invoke(_questSetup);
    //
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
}
