using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OnSelectCharacterManager : MonoBehaviour
{
    [SerializeField] private LoadSceneButton loadSceneButton;
    [Space]
    [SerializeField] private GameObject nameChar_01;
    [SerializeField] private RawImage char_01RawImage;
    [SerializeField] private Animator char_01Animator;
    [SerializeField] private PlayerConfiguration char_01Config;
    [Space]
    [SerializeField] private GameObject nameChar_02;
    [SerializeField] private RawImage char_02RawImage;
    [SerializeField] private Animator char_02Animator;
    [SerializeField] private PlayerConfiguration char_02Config;
    [Space] 
    [SerializeField] private GameObject animatedLoadPanel;
    //
    private Tween colorChar_01Tween;
    private Tween colorChar_02Tween;
    private Tween textChar_01Tween;
    private Tween textChar_02Tween;
    private readonly float _durationTween = .2f;
    private readonly Vector3 _selectScale = new(1.15f, 1.15f, 1f);
    private readonly Color _unSelectColor = new(0.8f, 0.8f, 0.8f, 1);
    private readonly int IDTalkingSelect = Animator.StringToHash("TalkingSelect");
    private readonly int IDTalkingDeSelect = Animator.StringToHash("TalkingDeSelect");
    
    private Coroutine _loadPanelCoroutine;

    private UserData _userData;
    private PlayerConfiguration _playerConfig;


    private void OnEnable()
    {
        OpenPanelLoad(.55f);
    }
    private void Start()
    {
        char_01RawImage.color = _unSelectColor;
        char_02RawImage.color = _unSelectColor;
        textChar_01Tween = nameChar_02.transform.DOScale(Vector3.one, _durationTween);
        textChar_02Tween = nameChar_02.transform.DOScale(Vector3.one, _durationTween);
    }
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        ClearDotTween();
        _playerConfig = null;
        char_01RawImage.DOColor(_unSelectColor, _durationTween);
        char_02RawImage.DOColor(_unSelectColor, _durationTween);
        textChar_01Tween = nameChar_01.transform.DOScale(Vector3.one, _durationTween);
        textChar_02Tween = nameChar_02.transform.DOScale(Vector3.one, _durationTween);
    }
    public void SelectChar(RawImage _charRawImage)
    {
        ClearDotTween();
        
        if (char_01RawImage == _charRawImage)
        {
            _playerConfig = char_01Config;
            colorChar_01Tween = char_01RawImage.DOColor(Color.white, _durationTween);
            colorChar_02Tween = char_02RawImage.DOColor(_unSelectColor, _durationTween);
            textChar_01Tween = nameChar_01.transform.DOScale(_selectScale, _durationTween);
            textChar_02Tween = nameChar_02.transform.DOScale(Vector3.one, _durationTween);
            
            if (char_01Animator.IsTag("select")) return;
            char_01Animator.SetTrigger(IDTalkingSelect);
            char_02Animator.SetTrigger(IDTalkingDeSelect);
        }
        else
        {           
            _playerConfig = char_02Config;
            colorChar_01Tween = char_01RawImage.DOColor(_unSelectColor, _durationTween);
            colorChar_02Tween = char_02RawImage.DOColor(Color.white, _durationTween);
            textChar_01Tween = nameChar_01.transform.DOScale(Vector3.one, _durationTween);
            textChar_02Tween = nameChar_02.transform.DOScale(_selectScale, _durationTween);
            
            if (char_02Animator.IsTag("select")) return;
            char_01Animator.SetTrigger(IDTalkingDeSelect);
            char_02Animator.SetTrigger(IDTalkingSelect);
        }
    }
    private void ClearDotTween()
    {
        colorChar_01Tween ?.Kill();
        colorChar_02Tween ?.Kill();
        textChar_01Tween ?.Kill();
        textChar_02Tween ?.Kill();
    }
    
    

    public void UpdateUserData()
    {
        if (!PlayFabHandleUserData.Instance || !_playerConfig) return;
        
        _userData = new UserData(PlayFabController.Instance.username, 1000);
        PlayFabHandleUserData.Instance.SetUserData(_userData, PlayFabHandleUserData.PF_Key.UserData_Key);
        PlayFabHandleUserData.Instance.SetUserData(_playerConfig, PlayFabHandleUserData.PF_Key.PlayerConfigData_Key);
        OpenPanelLoad(2.5f);
    }


    private void OpenPanelLoad(float _disableTime)
    {
        if(_loadPanelCoroutine != null)
            StopCoroutine(_loadPanelCoroutine);
        _loadPanelCoroutine = StartCoroutine(AnimatedPanelCoroutine(_disableTime));
    }
    private IEnumerator AnimatedPanelCoroutine(float _disableTime)
    {
        animatedLoadPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(_disableTime);
        animatedLoadPanel.gameObject.SetActive(false);
        
        if (!PlayFabHandleUserData.Instance || !_playerConfig)
            yield break;
        
        loadSceneButton.LoadScene();
    }

}
