using DG.Tweening;
using UnityEngine;

public class M_SetFloat : SetMaterial
{
    [Tooltip("Giá trị cần set")]
    public float valueSetTo;
    
    private float currentValue;
    public override void Apply()
    {
        currentValue = !useList ? material.GetFloat(nameID) : metarialsList[0].GetFloat(nameID);
        
        _applyTween?.Kill();
        _applyTween = !useList ? DOVirtual.Float(currentValue, valueSetTo, durationApply, Set)
                               : DOVirtual.Float(currentValue, valueSetTo, durationApply, Sets);
    }
    
    public void ChangeValueSet(float _value) => valueSetTo = _value;
    private void Set(float _value) => material.SetFloat(nameID, _value);
    private void Sets(float _value) => metarialsList.ForEach(x => x.SetFloat(nameID, _value));
}
