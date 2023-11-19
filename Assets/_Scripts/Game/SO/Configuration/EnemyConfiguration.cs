using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Config", menuName = "Characters Configuration/Enemy")]
public class EnemyConfiguration : CharacterConfiguration
{
    
    [Header("COOLDOWN")]
    [Tooltip("Thời gian chờ lần tấn công tiếp theo")]
    public float NormalAttackCD;
    
    [Tooltip("Thời gian chờ lần Skill tiếp theo")] 
    public float SkillAttackCD;
    
}
