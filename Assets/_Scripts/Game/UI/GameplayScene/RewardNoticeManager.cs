using System.Collections;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class RewardNoticeManager : Singleton<RewardNoticeManager>
{
    [SerializeField, BoxGroup("Notice type 1")] private TextMeshProUGUI titleText;
    [SerializeField, BoxGroup("Notice type 1")] private TextBar_3 textBar1Prefab;
    [SerializeField, BoxGroup("Notice type 1")] private Transform content1;

    [SerializeField, BoxGroup("Notice type 2")] private TextBar_3 textBar2Prefab;
    [SerializeField, BoxGroup("Notice type 2")] private Transform content2;

    private static ObjectPooler<TextBar_3> _pooltextBar1;
    private static ObjectPooler<TextBar_3> _pooltextBar2;
    
    private Tween _titleTween;
    private readonly float _tweenDuration = .2f;
    private Coroutine _disableNoticeCoroutine;
    private readonly YieldInstruction _yieldInstruction = new WaitForSeconds(2f);
    
    private void Start()
    {
        _pooltextBar1 = new ObjectPooler<TextBar_3>(textBar1Prefab, content1, 15);
        _pooltextBar2 = new ObjectPooler<TextBar_3>(textBar2Prefab, content2, 15);
        
        titleText.color = new Color(1, 1, 1, 0);
    }


    /// <summary>
    /// Tạo 1 text thông báo với giá trị value item nhận được và icon hiển thị tương ứng item đó
    /// Thông báo này sẽ xuất hiện khi đã thu thập vật phẩm
    /// </summary>
    /// <param name="_value"> Giá trị Text trong TextBar </param>
    /// <param name="_spriteIcon"> Icon hiển thị trên TextBar </param>
    public static void CreateNoticeT1(string _value, Sprite _spriteIcon)
    {
        var textBar = _pooltextBar1.Get();
        textBar.SetTextBar(_value, _spriteIcon);
    }
    
    /// <summary>
    /// Tạo 1 text thông báo với giá trị value item nhận được và icon hiển thị tương ứng item đó
    /// Thông báo này sẽ xuất hiện để thu thập vật phẩm khi player đứng gần 1 item
    /// </summary>
    /// <param name="_value"> Giá trị Text trong TextBar </param>
    /// <param name="_spriteIcon"> Icon hiển thị trên TextBar </param>
    public static void CreateNoticeT2(string _value, Sprite _spriteIcon)
    {
        var count = _pooltextBar2.List.Count(textBar3 => textBar3.gameObject.activeSelf);
        
        var textBar = _pooltextBar2.Get();
        textBar.SetTextBar(_value, _spriteIcon);
        textBar.animator.Play(count == 0 ? "TextBar02_IN" : "TextBar02_WAIT");
    }

    public static void ReleaseAllNoticeT2() => _pooltextBar2.List.ForEach(t => t.Release());
    
    
    /// <summary>
    /// Bật tiêu đề trên bản thông báo khi nhận vật phẩm
    /// </summary>
    public void EnableTitleNoticeT1()
    {
        _titleTween?.Kill();
        _titleTween = titleText.DOColor(new Color(1, 1, 1, 1), _tweenDuration);
        if(_disableNoticeCoroutine != null) 
            StopCoroutine(_disableNoticeCoroutine);
        _disableNoticeCoroutine = StartCoroutine(DisableNoticeCoroutine());
    }
    private IEnumerator DisableNoticeCoroutine()
    {
        yield return _yieldInstruction;
        _titleTween?.Kill();
        _titleTween = titleText.DOColor(new Color(1, 1, 1, 0), _tweenDuration);
        
        if(PlayFabHandleUserData.Instance)
            PlayFabHandleUserData.Instance.SaveData();
    }
    
    
    
    
    
}
