using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Config", menuName = "Characters Configuration/Enemy")]
public class EnemyConfiguration : CharacterConfiguration
{
    
    [Header("COOLDOWN")]
    [Tooltip("Thời gian chờ lần tấn công tiếp theo")]
    public float AttackCD;
    
    [Tooltip("Thời gian chờ lần Skill tiếp theo")] 
    public float SkillCD;
    
    [Tooltip("Thời gian chờ lần Special tiếp theo")] 
    public float SpecialCD;
    
    
}
