using System.Linq;
using TMPro;
using UnityEngine;

public class GUI_Quest : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TextMeshProUGUI titleQuest;
    [SerializeField] private TextMeshProUGUI descriptionQuest;
    [Space]
    [SerializeField] private QuestBoxText textBarPrefab;
    [SerializeField] private Transform contentQuest;
    [Space]
    [SerializeField] private UI_Item itemPrefab;
    [SerializeField] private Transform contentItem;

    
    private static ObjectPooler<QuestBoxText> _poolQuestBoxText;
    private static ObjectPooler<UI_Item> _poolUIItem;
    
    private static readonly int NameHashID_SelectedQuest = Animator.StringToHash("SelectedQuest");
    private static readonly int NameHashID_DeSelectedQuest = Animator.StringToHash("DeSelectedQuest");
    private static readonly int NameHashID_Trigger = Animator.StringToHash("Trigger");
    private static readonly int NameHashID_NonTrigger = Animator.StringToHash("NonTrigger");
    private SO_GameItemData _itemData;
    
    
    private void Start()
    {
        Init();
        QuestManager.OnQuestOpenEvent += OpenQuestPanel;
        _poolQuestBoxText.List.ForEach(box => box.OnQuestSelectEvent += OnSelectQuest);      
    }
    private void OnDestroy()
    {
        QuestManager.OnQuestOpenEvent -= OpenQuestPanel;
        _poolQuestBoxText.List.ForEach(box => box.OnQuestSelectEvent -= OnSelectQuest);
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

    
    public void OpenQuestPanel(QuestSetup[] _questList)
    {
        CursorHandle.NoneLocked();
        GUI_Inputs.InputAction.UI.OpenBag.Disable();
        GUI_Inputs.InputAction.UI.OpenMenu.Disable();
        Time.timeScale = 0;
        
        questPanel.SetActive(true);
        ShowQuest(_questList);
    }
    public void CloseQuestPanel()
    {
        CursorHandle.Locked();
        GUI_Inputs.InputAction.UI.OpenBag.Enable();
        GUI_Inputs.InputAction.UI.OpenMenu.Enable();
        Time.timeScale = 1;
        
        questPanel.SetActive(false);
    }
    private void ShowQuest(QuestSetup[] _questList)
    {
        foreach (var questSetup in _questList)
        {
            var textBar = _poolQuestBoxText.Get();
            textBar.SetBoxQuest(questSetup);
        }
    }

    
    private void OnSelectQuest(QuestBoxText _questBox, QuestSetup _questSetup)
    {
        titleQuest.text = _questSetup.GetTitle();
        descriptionQuest.text = _questSetup.GetDescription();
        
        _poolUIItem.List.ForEach(item => item.Release());
        foreach (var itemReward in _questSetup.GetReward())
        {
            var _valueReward = itemReward.GetValue();
            var itemCustom = _itemData.GetItemCustom(itemReward.GetNameCode());
            var uiItem = _poolUIItem.Get();
            uiItem.SetItem(itemCustom, _valueReward);
            uiItem.SetValueText($"{_valueReward}");
        }
        _questBox.animator.Play(NameHashID_SelectedQuest);
    }
    public static void IsTriggerBoxQuest(QuestBoxText _questBox, bool _hasTrigger)
    {
        var _currentList = _poolQuestBoxText.List.Where(box => box.gameObject.activeSelf);
        foreach (var questBoxTest in _currentList)
        {
            if (!questBoxTest.animator.IsTag("Selected") && !questBoxTest.animator.IsTag("SelectedQuest"))
            {
                _questBox.animator.Play(_hasTrigger ? NameHashID_Trigger : NameHashID_NonTrigger);
            }
        }
    }
    
}
