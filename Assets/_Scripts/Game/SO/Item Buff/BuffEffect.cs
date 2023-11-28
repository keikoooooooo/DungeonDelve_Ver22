using UnityEngine;

public abstract class BuffEffect : ScriptableObject
{
    [Tooltip("Cấu hình Item: Type, Sprite, ...")]
    public ItemCustom ItemCustom;
    
    [Tooltip("Mô tả Buff")]
    public string Description;
    
    [Tooltip("Giá trị Buff cộng vào")]
    public float Value;
    
    public abstract void Apply(PlayerController _player);
}
