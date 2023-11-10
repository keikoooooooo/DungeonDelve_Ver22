using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FloatMultiplier
{
    public string MultiplierTypeName;
    public List<float> Multiplier;
}

public class CharacterConfiguration : ScriptableObject
{
    [Header("INFORMATION")]
    [Tooltip("Tên nhân vật")]
    public string Name;

    [Tooltip("Cấp nhân vật")] 
    public int Level;
    
    [Tooltip("Giới thiệu nhân vật")] 
    public string Infor;
    
    
    [Header("CHARACTER STATS")]
    [Tooltip("Máu tối đa")] 
    public int MaxHealth;
    
    [Tooltip("Sát thương tấn công")] 
    public int ATK;

    [Tooltip("Sức phòng thủ")] 
    public int DEF;
    
    [Tooltip("Tốc độ đi bộ")] 
    public float WalkSpeed = 2.5f;
    
    [Tooltip("Tốc độ chạy")]  
    public float RunSpeed = 4f;
    

}
