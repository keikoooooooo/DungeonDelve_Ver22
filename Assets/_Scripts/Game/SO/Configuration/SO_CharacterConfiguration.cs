using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
public class FloatMultiplier
{
    public string MultiplierTypeName;
    [SerializeField, JsonProperty] private List<float> Multiplier;
    
    public FloatMultiplier() { }
    public FloatMultiplier(string _multiplierTypeName, List<float> _multiplier)
    {
        MultiplierTypeName = _multiplierTypeName;
        Multiplier = _multiplier;
    }
    
    
    public List<float> GetMultiplier() => Multiplier;
    public void SetMultiplier(List<float> _value) => Multiplier = _value;
}


[Serializable]
public class SO_CharacterConfiguration : ScriptableObject
{
    [Header("INFORMATION")]
    [Tooltip("Tên nhân vật"), SerializeField, JsonProperty]
    private string Name;

    [Tooltip("Cấp nhân vật"), SerializeField, JsonProperty]
    private int Level;

    [Tooltip("Kinh nghiệm hiện tại của nhân vật"), SerializeField, JsonProperty]
    private int CurrentEXP;
    
    [Tooltip("Giới thiệu nhân vật"), SerializeField, JsonProperty]
    private string Infor;
    
    
    [Header("CHARACTER STATS")]
    [Tooltip("Máu tối đa"), SerializeField, JsonProperty] 
    private int MaxHP;
    
    [Tooltip("Sát thương tấn công"), SerializeField, JsonProperty] 
    private int ATK;
   
    [Tooltip("Tỷ lệ bạo kích (%)"), SerializeField, JsonProperty] 
    private float CRITRate = 5f; // Mặc định Char = 5%CRIT
    
    [Tooltip("Sát thương bạo kích (%)"), SerializeField, JsonProperty] 
    private int CRITDMG = 50; // 50CRIT DMG <=> +150% DMG
    
    [Tooltip("Sức phòng thủ"), SerializeField, JsonProperty]
    private int DEF;
    
    [Tooltip("Tốc độ đi bộ"), SerializeField, JsonProperty]
    private float WalkSpeed = 2.5f;
    
    [Tooltip("Tốc độ chạy"), SerializeField, JsonProperty] 
    private float RunSpeed = 4f;


    public string GetName() => Name;
    public string SetName(string _value) => Name = _value;
    
    public int GetLevel() => Level;
    public int SetLevel(int _value) => Level = _value;
    
    public int GetCurrentEXP() => CurrentEXP;
    public int SetCurrentEXP(int _value) => CurrentEXP = _value;
    
    public string GetInfor() => Infor;
    public string SetInfor(string _value) => Infor = _value;
    
    public int GetHP() => MaxHP;
    public int SetHP(int _value) => MaxHP = _value;
    
    public int GetATK() => ATK;
    public int SetATK(int _value) => ATK = _value;
    
    public float GetCRITRate() => CRITRate;
    public float SetCRITRate(float _value) => CRITRate = _value;
    
    public int GetCRITDMG() => CRITDMG;
    public int SetCRITDMG(int _value) => CRITDMG = _value;
    
    public int GetDEF() => DEF;
    public int SetDEF(int _value) => DEF = _value;
    
    public float GetWalkSpeed() => WalkSpeed;
    public float SetWalkSpeed(float _value) => WalkSpeed = _value;
    
    public float GetRunSpeed() => RunSpeed;
    public float SetRunSpeed(float _value) => RunSpeed = _value;
    
}
