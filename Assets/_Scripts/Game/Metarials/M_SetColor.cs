using DG.Tweening;
using UnityEngine;

public class M_SetColor : SetMaterial
{
    [Tooltip("Màu cần set"), SerializeField]
    private Color colorSetTo;

    private Color currentColor;
    public override void Apply()
    {
        currentColor = !useList ? material.GetColor(nameID) : metarialsList[0].GetColor(nameID);

        _applyTween?.Kill();
        _applyTween = !useList ? DOVirtual.Color(currentColor, colorSetTo, durationApply, Set)
                               : DOVirtual.Color(currentColor, colorSetTo, durationApply, Sets);
    }

    public void ChangeColorSet(Color _value) => colorSetTo = _value;
    private void Set(Color _value) => material.SetColor(nameID, _value);
    private void Sets(Color _value) => metarialsList.ForEach(x => x.SetColor(nameID, _value));
    
}
