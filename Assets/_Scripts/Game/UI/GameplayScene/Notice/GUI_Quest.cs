using TMPro;
using UnityEngine;

public class GUI_Quest : MonoBehaviour
{
    
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TextMeshProUGUI titleQuest;
    [SerializeField] private TextMeshProUGUI inforQuest;
    [Space]
    [SerializeField] private TextBar textBarPrefab;
    [SerializeField] private Transform contentQuest;

    private ObjectPooler<TextBar> _poolTextBar;
    
    private void OnEnable()
    {
        QuestManager.OnQuestOpenEvent += OpenQuestPanel;
    }
    private void Start()
    {
        _poolTextBar = new ObjectPooler<TextBar>(textBarPrefab, contentQuest, 15);
        titleQuest.text = "";
        inforQuest.text = "";
    }
    private void OnDisable()
    {
        QuestManager.OnQuestOpenEvent -= OpenQuestPanel;
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
            var textBar = _poolTextBar.Get();
            textBar.SetTitleText(questSetup.GetTitle());
        }
    }
    
    
    
}
