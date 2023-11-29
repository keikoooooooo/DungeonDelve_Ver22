using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OnSelectCharacterManager : MonoBehaviour
{
    [SerializeField] private RawImage Char_01;
    [SerializeField] private RawImage Char_02;

    private Tween colorChar_01Tween;
    private Tween colorChar_02Tween;

    private readonly Color _unSelectColor = new(0.8f, 0.8f, 0.8f, 1);
    private readonly float _durationTween = .2f;

    private void Start()
    {
        Char_01.color = _unSelectColor;
        Char_02.color = _unSelectColor;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        colorChar_01Tween ?.Kill();
        colorChar_02Tween ?.Kill();
        Char_01.DOColor(_unSelectColor, _durationTween);
        Char_02.DOColor(_unSelectColor, _durationTween);
    }

    public void SelectChar(RawImage _charRawImage)
    {
        colorChar_01Tween ?.Kill();
        colorChar_02Tween ?.Kill();
        
        if (Char_01 == _charRawImage)
        {
            colorChar_01Tween = Char_01.DOColor(Color.white, _durationTween);
            colorChar_02Tween = Char_02.DOColor(_unSelectColor, _durationTween);
        }
        else
        {           
            colorChar_01Tween = Char_01.DOColor(_unSelectColor, _durationTween);
            colorChar_02Tween = Char_02.DOColor(Color.white, _durationTween);
        }
    }

}
