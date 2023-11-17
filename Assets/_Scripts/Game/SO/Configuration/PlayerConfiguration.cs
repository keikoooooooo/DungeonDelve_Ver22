using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerUpgrade
{
    [Header("Level Upgrade")]
    public int LevelUpgrade;
    [Tooltip("Chi phí nâng cấp")]
    public int UpgradeCost;
    [Tooltip("Kinh nghiệm nâng cấp tối đa của Level tiếp theo")]
    public int MaxExperienceUpgrade;
    [Tooltip("Vật liệu nâng cấp")]
    public List<ItemUpgrade> MaterialsUpgrade;
}


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


    [Header("Level Upgrade")] 
    public List<PlayerUpgrade> CharacterUpgrade;
    public List<PlayerUpgrade> WeaponUpgrade;
    
    
    // [Tooltip("Chi phí nâng cấp")]
    // public int UpgradeCost;
    // [Tooltip("Kinh nghiệm hiện tại")]
    // public int CurrentExperience;
    // [Tooltip("Kinh nghiệm tối đa")]
    // public int MaxExperience;
    // [Tooltip("Vật liệu nâng cấp")]
    // public List<ItemUpgrade> UpgradeMaterials;

}
