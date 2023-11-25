using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player Config", menuName = "Characters Configuration/Player")]
public class PlayerConfiguration : CharacterConfiguration
{
    
    [Tooltip("Tốc độ chạy nhanh")] 
    public float RunFastSpeed = 8f;
    [Tooltip("Sức bền tối đa")] 
    public int MaxST = 100;
    [Tooltip("Năng lượng cho mỗi lần lướt")]
    public int DashEnergy = 25;
    [Tooltip("Độ cao khi nhảy")] 
    public float JumpHeight = 1.2f;
    
    [Header("COOLDOWN")]
    [Tooltip("Thời gian nhảy cho lần tiếp theo")] 
    public float JumpCD;
    [Tooltip("Thời gian hồi kỹ năng (s)")] 
    public float ElementalSkillCD;
    [Tooltip("Thời gian hồi kỹ năng đặc biệt (s)")] 
    public float ElementalBurstlCD;

    [Header("WEAPON")] 
    [Tooltip("Tên của vũ khí")]
    public string WeaponName;
    
    [Tooltip("Thông tin của vũ khí")]
    public string WeaponInfo;
    
    [Tooltip("Cấp độ của vũ khí"), Range(1, 10)]
    public int WeaponLevel;

    
    [Header("ATTACK MULTIPLIER")]
    public List<FloatMultiplier> NormalAttackMultiplier;
    public List<FloatMultiplier> ChargedAttackMultiplier;
    public List<FloatMultiplier> SkillMultiplier;
    public List<FloatMultiplier> SpecialMultiplier;
    
}
