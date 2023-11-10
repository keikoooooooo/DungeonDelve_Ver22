using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Config", menuName = "Characters Configuration/Player")]
public class PlayerConfiguration : CharacterConfiguration
{
    
    [Tooltip("Tốc độ chạy nhanh")] 
    public float RunFastSpeed = 8f;
    [Tooltip("Sức bền tối đa")] 
    public int MaxStamina = 100;
    [Tooltip("Năng lượng cho mỗi lần lướt")]
    public int DashEnergy = 25;
    [Tooltip("Độ cao khi nhảy")] 
    public float JumpHeight = 1.2f;


    [Header("WEAPON STATS")]
    [Tooltip("Cấp độ của vũ khí"), Range(1, 10)]
    public int WeaponLevel;
    
    
    [Header("COOLDOWN")]
    [Tooltip("Thời gian nhảy cho lần tiếp theo")] 
    public float JumpCD;
    [Tooltip("Thời gian hồi kỹ năng (s)")] 
    public float SkillCD;
    [Tooltip("Thời gian hồi kỹ năng đặc biệt (s)")] 
    public float SpecialCD;
    
    
    [Header("ATTACK MULTIPLIER")]
    public List<FloatMultiplier> NormalAttackMultiplier;
    public List<FloatMultiplier> ChargedAttackMultiplier;
    public List<FloatMultiplier> SkillMultiplier;
    public List<FloatMultiplier> SpecialMultiplier;
    
}