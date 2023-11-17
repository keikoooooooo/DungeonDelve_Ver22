using UnityEngine;

public abstract class BuffEffect : ScriptableObject
{
    [Tooltip("Avatar của Buff")]
    public Sprite Icon;
    
    [Tooltip("Mô tả Buff")]
    public string Description;
    
    [Tooltip("Giá trị Buff cộng vào")]
    public float Value;
    
    public abstract void Apply(PlayerStateMachine _player);
}
