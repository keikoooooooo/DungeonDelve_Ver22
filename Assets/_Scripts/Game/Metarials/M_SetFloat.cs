using DG.Tweening;
using UnityEngine;

public class M_SetFloat : SetMaterial
{
    [Tooltip("Giá trị cần set"), SerializeField]
    private float valueSetTo;

    private float CurrentValue;
    
    public override void Apply()
    {
        
        _applyTween?.Kill();
        _applyTween = !useList ? DOVirtual.Float(CurrentValue, valueSetTo, durationApply, Set)
                               : DOVirtual.Float(CurrentValue, valueSetTo, durationApply, Sets);
    }
    
    /// <summary>
    /// Thay đổi giá trị áp dụng tới
    /// </summary>
    /// <param name="_value"> Giá trị mới để áp dụng </param>
    public void ChangeValueSet(float _value) => valueSetTo = _value;
    
    /// <summary>
    /// Thay đổi giá trị hiện tại
    /// </summary>
    /// <param name="_value"> Giá trị hiện tại </param>
    public void ChangeCurrentValue(float _value) => CurrentValue = _value;
    
    private void Set(float _value) => material.SetFloat(nameID, _value);
    private void Sets(float _value) => metarialsList.ForEach(x => x.SetFloat(nameID, _value));
}
