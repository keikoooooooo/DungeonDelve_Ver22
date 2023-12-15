using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Quest : Singleton<MonoBehaviour>
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private Animator noticeQuestPanel;
    [SerializeField] private TextMeshProUGUI noticeQuestText;
    [Space]
    [SerializeField] private TextMeshProUGUI titleQuest;
    [SerializeField] private TextMeshProUGUI descriptionQuest;
    [SerializeField] private TextMeshProUGUI questProgressText;
    [SerializeField] private Button cancelBtt;
    [SerializeField] private Button acceptBtt;
    [Space]
    [SerializeField] private QuestBoxText textBarPrefab;
    [SerializeField] private Transform contentQuest;
    [Space]
    [SerializeField] private UI_Item itemPrefab;
    [SerializeField] private Transform contentItem;
    
    private static ObjectPooler<QuestBoxText> _poolQuestBoxText;
    private static ObjectPooler<UI_Item> _poolUIItem;
   
    private SO_GameItemData _itemData;
    private QuestBoxText _questBox;

    private int _currentReceivedQuest;
    private bool _isAccept;
    
    private void Start()
    {
        Init();
        RegisterEvent();
    }
    private void OnDestroy()
    {
        UnRegisterEvent();
    }
    
    private void Init()
    {
        _itemData = GameManager.Instance.GameItemData;
        _poolQuestBoxText = new ObjectPooler<QuestBoxText>(textBarPrefab, contentQuest, 10);
        _poolUIItem = new ObjectPooler<UI_Item>(itemPrefab, contentItem, 10);
        questPanel.gameObject.SetActive(false);
        titleQuest.text = "";
        descriptionQuest.text = "";
    }
    private void RegisterEvent()
    {
        QuestManager.OnPanelOpenEvent += OpenPanel;
        acceptBtt.onClick.AddListener(OnClickNoticeAcceptQuest);
        cancelBtt.onClick.AddListener(OnClickNoticeCancelQuest);
        _poolQuestBoxText.List.ForEach(box => box.OnQuestSelectEvent += OnSelectQuest);
    }
    private void UnRegisterEvent()
    {
        QuestManager.OnPanelOpenEvent -= OpenPanel;
        acceptBtt.onClick.RemoveListener(OnClickNoticeAcceptQuest);
        cancelBtt.onClick.RemoveListener(OnClickNoticeCancelQuest);
        _poolQuestBoxText.List.ForEach(box => box.OnQuestSelectEvent -= OnSelectQuest);
    }
    
    
    public void OpenPanel()
    {
        CursorHandle.NoneLocked();
        GUI_Inputs.InputAction.UI.OpenBag.Disable();
        GUI_Inputs.InputAction.UI.OpenMenu.Disable();
        Time.timeScale = 0;
        
        questPanel.SetActive(true);
        noticeQuestPanel.Play("Panel_Disable");
        
        ShowQuest();
        SetQuestProgressText();
    }
    public void ClosePanel()
    {
        QuestManager.ClosePanel();
        CursorHandle.Locked();
        GUI_Inputs.InputAction.UI.OpenBag.Enable();
        GUI_Inputs.InputAction.UI.OpenMenu.Enable();
        Time.timeScale = 1;
        
        questPanel.SetActive(false);
    }
    private void ShowQuest()
    {
        titleQuest.text = "???";
        descriptionQuest.text = "???";
        _poolUIItem.List.ForEach(item => item.Release());
        _poolQuestBoxText.List.ForEach(box => box.Release());

        var _quests = QuestManager.QuestLists;
        foreach (var questSetup in _quests)
        {
            var textBar = _poolQuestBoxText.Get();
            textBar.SetQuestBox(questSetup);
        }

        var _questActive = _poolQuestBoxText.List.Where(x => x.gameObject.activeSelf);

        for (var i = 0; i < QuestManager.maxQuest; i++)
        {
            
        }
        
    }

    
    public void OnSelectQuest(QuestBoxText _questBox, QuestSetup _questSetup)
    {
        titleQuest.text = _questSetup.GetTitle();
        descriptionQuest.text = _questSetup.GetDescription();
        
        SpawnItemReward(_questSetup);
        SelectQuestBox(_questBox);
        SetButton(_questBox);
    }
    private void SpawnItemReward(QuestSetup _questSetup)
    {
        var _rewards = _questSetup.GetReward();
        _rewards.Sort((x1, x2) => x1.GetNameCode().CompareTo(x2.GetNameCode()));
        
        _poolUIItem.List.ForEach(item => item.Release());
        for (var i = 0; i < _rewards.Count; i++)
        {
            _poolUIItem.Get();
        }

        var _count = 0;
        var _items = _poolUIItem.List.Where(item => item.gameObject.activeSelf).ToList();
        foreach (var uiItem in _items)
        {
            var _itemCustom = _itemData.GetItemCustom(_rewards[_count].GetNameCode());
            var _itemValue = _rewards[_count].GetValue();
            
            uiItem.SetItem(_itemCustom, _itemValue);
            uiItem.SetValueText($"{_itemValue}");
            _count++;
        }
    }
    private void SelectQuestBox(QuestBoxText _questBox)
    {
        this._questBox = _questBox;
        var _currentList = _poolQuestBoxText.List.Where(box => box.gameObject.activeSelf);
        foreach (var questBoxTest in _currentList)
        {
            var _animator = questBoxTest.animator;
            if (_animator.IsTag("Selected"))
                questBoxTest.animator.Play(SwitchPanelControl.NameHashID_NonTrigger);
            else if (!_animator.IsTag("Selected") && _animator == this._questBox.animator)
                questBoxTest.animator.Play(SwitchPanelControl.NameHashID_Selected);
        }
        SetButton(_questBox);
    }
    public static void IsTriggerQuestBox(QuestBoxText _questBox, bool _hasTrigger)
    {
        var _currentList = _poolQuestBoxText.List.Where(box => box.gameObject.activeSelf).ToList()
            .Where(box => !box.animator.IsTag("Selected", 0) && !box.animator.IsTag("SelectedQuest", 0));

        int _hashID;
        if (_hasTrigger)
        {
            foreach (var questBoxTest in _currentList)
            {
                _hashID = questBoxTest == _questBox ? SwitchPanelControl.NameHashID_Trigger : SwitchPanelControl.NameHashID_NonTrigger;
                questBoxTest.animator.Play(_hashID);
            }   
        }
        else
        {
            foreach (var questBoxTest in _currentList)
            {
                _hashID = SwitchPanelControl.NameHashID_NonTrigger;
                questBoxTest.animator.Play(_hashID);
            }
        }
    }
    
    
    
    private void OnClickNoticeCancelQuest()
    {
        if (!_questBox || !_questBox.IsAcceptQuest) return;
        _isAccept = false;
        OpenNoticeQuestPanel("Would you like to abandon this quest? When canceled, this quest will not be selectable for the remainder of today.\nContinue?");
    }
    private void OnClickNoticeAcceptQuest()
    {
        if (!_questBox || _questBox.IsAcceptQuest || _currentReceivedQuest >= QuestManager.maxQuest) return;
        _isAccept = true;
        OpenNoticeQuestPanel("Would you like to accept this quest? \nContinue?");
    }
    private void OpenNoticeQuestPanel(string _noticeText)
    {
        noticeQuestPanel.Play("Panel_IN");
        noticeQuestText.text = _noticeText;
    }
    public void OnClickCancelQuest()
    {
        noticeQuestPanel.Play("Panel_OUT");
    }
    public void OnClickAcceptQuest()
    {
        if (_isAccept)
        {
            _currentReceivedQuest++;
            _questBox.OnAcceptQuest();
            QuestManager.CallQuestStartedEvent(_questBox.questSetup);
        }
        else
        {
            _currentReceivedQuest--;
            _questBox.OnCancelQuest();
            QuestManager.CallQuestCancelEvent(_questBox.questSetup);
        }
        
        _isAccept = false;
        SetButton(_questBox);
        OnClickCancelQuest();
        SetQuestProgressText();
    }
    

    private void SetQuestProgressText() => questProgressText.text = $"In Progress: {_currentReceivedQuest}/{QuestManager.maxQuest}";
    private void SetButton(QuestBoxText _questBox)
    {
        acceptBtt.interactable = !_questBox.IsAcceptQuest && _currentReceivedQuest < QuestManager.maxQuest;
        cancelBtt.interactable = _questBox.IsAcceptQuest;
    }

}
