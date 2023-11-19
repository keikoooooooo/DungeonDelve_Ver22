using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using DG.Tweening;

public abstract class SetMaterial : MonoBehaviour
{
    public Material material;
    
    public bool useList;
    [Tooltip("Danh sách các Metarial cần set"), ShowIf("useList")]
    public List<Material> metarialsList;

    [Space, Tooltip("Tên biến cần set trong Shader")] 
    public string propertyToID = "";
    [Tooltip("Thời gian set từ giá trị hiện tại đến giá trị cần set"), SerializeField]
    protected float durationApply;
    [Space]
    protected Tween _applyTween;
    
    
    protected int nameID;
    private void Awake() => nameID = Shader.PropertyToID(propertyToID);


    public abstract void Apply();
}
