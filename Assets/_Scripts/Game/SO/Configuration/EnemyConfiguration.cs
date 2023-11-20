using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Config", menuName = "Characters Configuration/Enemy")]
public class EnemyConfiguration : CharacterConfiguration
{
    
    [Header("COOLDOWN")]
    [Tooltip("Thời gian chờ lần tấn công tiếp theo")]
    public float NormalAttackCD;
    
    [Tooltip("Thời gian chờ lần Skill tiếp theo")] 
    public float SkillAttackCD;
        
    [Tooltip("Thời gian chờ lần Special tiếp theo")] 
    public float SpecialAttackCD;
    
    [Header("ATTACK MULTIPLIER")]
    public List<FloatMultiplier> NormalAttackMultiplier;
    public List<FloatMultiplier> SkillMultiplier;
    public List<FloatMultiplier> SpecialMultiplier;
}
