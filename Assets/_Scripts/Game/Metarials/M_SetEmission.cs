using DG.Tweening;
using UnityEngine;

public class M_SetEmission : SetMaterial
{
    [Tooltip("Màu cần set")]
    public Color colorSetTo;
    
    [Tooltip("Cường độ hiện tại của màu")]
    public float currentIntensity;
    
    [Tooltip("Cường độ cần set của màu")]
    public float intensitySetTo;
    
    public override void Apply()
    {
        _applyTween?.Kill();
        _applyTween = !useList ? DOVirtual.Float(currentIntensity, intensitySetTo, durationApply, Set)
                               : DOVirtual.Float(currentIntensity, intensitySetTo, durationApply, Sets);
    }

    private void Set(float _value) => material.SetColor(nameID, colorSetTo * _value);
    private void Sets(float _value) => metarialsList.ForEach(x => x.SetColor(nameID, colorSetTo * _value));
    
}
