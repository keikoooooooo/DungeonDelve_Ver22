using DG.Tweening;
using UnityEngine;

public class M_SetEmission : SetMaterial
{
    [Tooltip("Màu cần set"), SerializeField]
    private Color colorSetTo;
    
    [Tooltip("Cường độ hiện tại của màu"), SerializeField]
    private float currentIntensity;
    
    [Tooltip("Cường độ cần set của màu"), SerializeField]
    private float intensitySetTo;
    
    public override void Apply()
    {
        _applyTween?.Kill();
        _applyTween = !useList ? DOVirtual.Float(currentIntensity, intensitySetTo, durationApply, Set)
                               : DOVirtual.Float(currentIntensity, intensitySetTo, durationApply, Sets);
    }

    /// <summary>
    /// Thay đổi màu
    /// </summary>
    /// <param name="_value"> Màu mới cần áp dụng </param>
    public void ChangeColorSet(Color _value) => colorSetTo = _value;
    
    /// <summary>
    /// Thay đổi giá trị hiện tại
    /// </summary>
    /// <param name="_value"> Giá trị hiện tại </param>
    public void ChangeCurrentIntensity(float _value) => currentIntensity = _value;
    
    /// <summary>
    /// Thay đổi giá trị áp dụng tới
    /// </summary>
    /// <param name="_value"> Giá trị mới để áp dụng </param>
    public void ChangeIntensitySet(float _value) => intensitySetTo = _value;
    
     
    private void Set(float _value) => material.SetColor(nameID, colorSetTo * _value);
    private void Sets(float _value) => metarialsList.ForEach(x => x.SetColor(nameID, colorSetTo * _value));

    
}
