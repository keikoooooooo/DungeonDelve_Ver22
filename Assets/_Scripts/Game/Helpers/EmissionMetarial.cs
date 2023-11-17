using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EmissionMetarial : MonoBehaviour
{
    [Tooltip("Danh sách các Metarial cần set")]
    public List<Material> metarialsList;

    public Color setColor;
    public float maxIntensity;
    [Space]
    [Tooltip("Thời gian chạy từ giá trị 0 -> MaxIntensity")]
    public float durationEnable;
    [Tooltip("Thời gian chạy từ giá trị MaxIntensity -> 0")]
    public float durationDisable;

    [Space, Tooltip("Tên biến cần set trong Shader")] 
    public string propertyToID = "_EmissionColor";
    
    private static int _emissionColor;
    private Tween _emissionTween;

    private void Awake()
    {
        _emissionColor = Shader.PropertyToID(propertyToID);
    }
    
    public void EnableEmission()
    {
        _emissionTween?.Kill();
        _emissionTween = DOVirtual.Float(0f, maxIntensity, durationEnable, SetColor);
    }
    public void DisableEmission()
    {
        _emissionTween?.Kill();
        _emissionTween = DOVirtual.Float(maxIntensity, 0f, durationDisable, SetColor);
    }
    
    private void SetColor(float _value)
    {
        metarialsList.ForEach(x => x.SetColor(_emissionColor, setColor * _value));
    }
    
    
}
