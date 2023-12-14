using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestManager : MonoBehaviour
{
    private static QuestSetup[] questSetups;
    public static event Action<QuestSetup[]> OnQuestOpenEvent; 
    
    private void Start()
    {
        questSetups = Resources.LoadAll<QuestSetup>("Quest Custom");
    }
    private void OnCollectInput(InputAction.CallbackContext _context)
    {
        OnQuestOpenEvent?.Invoke(questSetups);
    }
    public void OnEnterPlayer()
    {
        GUI_Inputs.InputAction.UI.CollectItem.performed += OnCollectInput;
        NoticeManager.Instance.CreateNoticeT3("[F] Open Quest.");
    }
    public void OnExitPlayer()
    {
        GUI_Inputs.InputAction.UI.CollectItem.performed -= OnCollectInput;
        NoticeManager.Instance.CloseNoticeT3();
    }
}
