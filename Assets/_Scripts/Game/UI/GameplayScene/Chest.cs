using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RewardSetup))]
public class Chest : MonoBehaviour
{
    [SerializeField, Required] private RewardSetup rewardSetup;
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private BoxCollider chestCollider;
    [SerializeField] private ParticleSystem chestVFX;
    [SerializeField] private M_SetFloat setDissolve;
    
    private event Action OnOpenChestEvent;
    public IconIndicator Indicator { get; set; }
    
    private readonly int OpenChestID = Animator.StringToHash("OpenChest");
    private readonly int ActiveChestID = Animator.StringToHash("ActiveChest");

    private Coroutine _activeCoroutine;
    private Coroutine _openCoroutine;
    private Coroutine _closeCoroutine;
    private NoticeManager _notice;
    
    private bool _detectPlayer; // Có trigger với Player ?
    private bool _canReceived;  // Có thể nhận thưởng ?
    
    private void OnEnable()
    {
        GUI_Inputs.InputAction.UI.CollectItem.performed += OnClickOpenChest;
        OnOpenChestEvent += rewardSetup.SendRewardData;
    }
    private void Start()
    {
        chestCollider.enabled = false;
        chestVFX.gameObject.SetActive(false);
        _notice = NoticeManager.Instance;
        SetDissolve(0, 1, 0);
    }
    private void OnDisable()
    {
        GUI_Inputs.InputAction.UI.CollectItem.performed -= OnClickOpenChest;
        OnOpenChestEvent -= rewardSetup.SendRewardData;
    }
    
    
    /// <summary>
    /// Khi nhấn (Input) để mở rương
    /// </summary>
    /// <param name="_context"></param>
    private void OnClickOpenChest(InputAction.CallbackContext _context)
    {
        if(!_detectPlayer || !_canReceived) return;
        _canReceived = false;
        OpenChest();
        CloseChest();
        _notice.CloseNoticeT3();
    }
    
    
    /// <summary>
    /// Tạo rương
    /// </summary>
    public void CreateChest()
    {
        if(_activeCoroutine != null) 
            StopCoroutine(_activeCoroutine);
        _activeCoroutine = StartCoroutine(ActiveChestCoroutine());
    }
    private IEnumerator ActiveChestCoroutine()
    {
        _canReceived = true;
        chestCollider.enabled = true;
        chestAnimator.Rebind();
        SetDissolve(0, 1, 0);
        chestAnimator.SetTrigger(ActiveChestID);
        OpenIndicator();
        
        yield return new WaitForSeconds(.1f);
        SetDissolve(1, 0, 2f);
    }
    
    /// <summary>
    /// Mở rương
    /// </summary>
    private void OpenChest()
    {
        if(_openCoroutine != null) 
            StopCoroutine(_openCoroutine);
        _openCoroutine = StartCoroutine(OpenChestCoroutine());
    }
    private IEnumerator OpenChestCoroutine()
    {
        CloseIndicator();
        chestAnimator.SetBool(OpenChestID, true);
        
        yield return new WaitForSeconds(.8f);
        chestVFX.gameObject.SetActive(true);
        chestVFX.Play();
        OnOpenChestEvent?.Invoke();
    }
    
    /// <summary>
    /// Đóng rương
    /// </summary>
    private void CloseChest()
    {
        if(_closeCoroutine != null) 
            StopCoroutine(_closeCoroutine);
        _closeCoroutine = StartCoroutine(CloseChestCoroutine());
    }
    private IEnumerator CloseChestCoroutine()
    {
        yield return new WaitForSeconds(7f);
        SetDissolve(0, 1, 1.5f);
        
        yield return new WaitForSeconds(.5f);
        chestVFX.gameObject.SetActive(false);
        chestVFX.Stop();
        
        yield return new WaitForSeconds(.5f);
        chestCollider.enabled = false;
        chestAnimator.SetBool(OpenChestID, false);
    }
    
    private void SetDissolve(float _currentValue, float _setValue, float _duration)
    {
        setDissolve.ChangeCurrentValue(_currentValue);
        setDissolve.ChangeValueSet(_setValue);
        setDissolve.ChangeDurationApply(_duration);
        setDissolve.Apply();
    }

    
    #region Trigger Event
    /// <summary>
    /// Mở thông báo mở rương 
    /// </summary>
    public void OnEnterPlayerCollision()
    {
        _detectPlayer = true;
        if(!_canReceived) 
            _notice.CloseNoticeT3();
        else 
            _notice.CreateNoticeT3("[F] Open Chest.");
    }
    
    /// <summary>
    /// Đóng thông báo mở rương
    /// </summary>
    public void OnExitPlayerCollision() 
    {
        _detectPlayer = false;
        _notice.CloseNoticeT3();
    }

    /// <summary>
    /// Mở chỉ dẫn tới vị trí rương
    /// </summary>
    public void OpenIndicator()
    {
        if (!_canReceived) return;
        ChestNoticeManager.AddChest(this);
    }

    /// <summary>
    /// Đóng chỉ dẫn tới vị trí rương
    /// </summary>
    public void CloseIndicator()
    {
        ChestNoticeManager.RemoveChest(this);
    }
    #endregion

}
