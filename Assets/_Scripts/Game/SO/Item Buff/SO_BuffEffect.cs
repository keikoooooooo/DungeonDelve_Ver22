using UnityEngine;

public abstract class SO_BuffEffect : ScriptableObject
{
    [Tooltip("Namecode của item")]
    public ItemNameCode NameCode;
    
    [Tooltip("Giá trị Buff cộng vào")]
    public float Value;
    
    public abstract void Apply(PlayerController _player);
}
