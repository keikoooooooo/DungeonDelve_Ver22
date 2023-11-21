using DG.Tweening;
using UnityEngine;

public class M_SetColor : SetMaterial
{
    [Tooltip("Màu cần set"), SerializeField]
    private Color colorSetTo;

    private Color currentColor;
    public override void Apply()
    {
        _applyTween?.Kill();
        _applyTween = !useList ? DOVirtual.Color(currentColor, colorSetTo, durationApply, Set)
                               : DOVirtual.Color(currentColor, colorSetTo, durationApply, Sets);
    }

    /// <summary>
    /// Thay đổi màu hiện tại 
    /// </summary>
    /// <param name="_value"> Giá trị của màu hiện tại </param>
    public void ChangeCurrentColor(Color _value) => currentColor = _value;
    
    
    /// <summary>
    /// Thay đổi màu của cần set tới
    /// </summary>
    /// <param name="_value"> Giá trị của màu mới cần set </param>
    public void ChangeColorSet(Color _value) => colorSetTo = _value;
    
    private void Set(Color _value) => material.SetColor(nameID, _value);
    private void Sets(Color _value) => metarialsList.ForEach(x => x.SetColor(nameID, _value));
    
}
