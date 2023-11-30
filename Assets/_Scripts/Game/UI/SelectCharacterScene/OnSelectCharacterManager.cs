using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnSelectCharacterManager : MonoBehaviour
{
    [SerializeField] private RawImage Char_01;
    [SerializeField] private RawImage Char_02;

    [SerializeField] private TextMeshProUGUI NameChar_01;
    [SerializeField] private TextMeshProUGUI NameChar_02;

    private Tween colorChar_01Tween;
    private Tween colorChar_02Tween;
    private Tween textChar_01Tween;
    private Tween textChar_02Tween;
    private readonly Vector3 _selectScale = new(1.15f, 1.15f, 1f);
    private readonly Color _unSelectColor = new(0.8f, 0.8f, 0.8f, 1);
    private readonly float _durationTween = .2f;

    
    private void Start()
    {
        Char_01.color = _unSelectColor;
        Char_02.color = _unSelectColor;
        textChar_01Tween = NameChar_02.transform.DOScale(Vector3.one, _durationTween);
        textChar_02Tween = NameChar_02.transform.DOScale(Vector3.one, _durationTween);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        colorChar_01Tween ?.Kill();
        colorChar_02Tween ?.Kill();
        Char_01.DOColor(_unSelectColor, _durationTween);
        Char_02.DOColor(_unSelectColor, _durationTween);
        textChar_01Tween ?.Kill();
        textChar_02Tween ?.Kill();
        textChar_01Tween = NameChar_02.transform.DOScale(Vector3.one, _durationTween);
        textChar_02Tween = NameChar_02.transform.DOScale(Vector3.one, _durationTween);
    }

    public void SelectChar(RawImage _charRawImage)
    {
        colorChar_01Tween ?.Kill();
        colorChar_02Tween ?.Kill();
        textChar_01Tween ?.Kill();
        textChar_02Tween ?.Kill();
        
        if (Char_01 == _charRawImage)
        {
            colorChar_01Tween = Char_01.DOColor(Color.white, _durationTween);
            colorChar_02Tween = Char_02.DOColor(_unSelectColor, _durationTween);
            textChar_01Tween = NameChar_01.transform.DOScale(_selectScale, _durationTween);
            textChar_02Tween = NameChar_02.transform.DOScale(Vector3.one, _durationTween);
        }
        else
        {           
            colorChar_01Tween = Char_01.DOColor(_unSelectColor, _durationTween);
            colorChar_02Tween = Char_02.DOColor(Color.white, _durationTween);
            textChar_01Tween = NameChar_01.transform.DOScale(Vector3.one, _durationTween);
            textChar_02Tween = NameChar_02.transform.DOScale(_selectScale, _durationTween);
        }
    }

}
