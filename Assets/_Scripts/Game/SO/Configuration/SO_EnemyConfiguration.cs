using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Config", menuName = "Characters Configuration/Enemy")]
public class SO_EnemyConfiguration : SO_CharacterConfiguration
{
    
    [Header("COOLDOWN")]
    [Tooltip("Thời gian chờ lần tấn công tiếp theo"), SerializeField, JsonProperty]
    private float NormalAttackCD;
    
    [Tooltip("Thời gian chờ lần Skill tiếp theo"), SerializeField, JsonProperty]
    private float SkillAttackCD;
        
    [Tooltip("Thời gian chờ lần Special tiếp theo"), SerializeField, JsonProperty]
    private float SpecialAttackCD;
    
    [Header("ATTACK MULTIPLIER")]
    [SerializeField, JsonProperty] private List<FloatMultiplier> NormalAttackMultiplier;
    [SerializeField, JsonProperty] private List<FloatMultiplier> SkillMultiplier;
    [SerializeField, JsonProperty] private List<FloatMultiplier> SpecialMultiplier;


    public float GetNormalAttackCD() => NormalAttackCD;
    public void SetNormalAttackCD(float _value) => NormalAttackCD = _value;
    
    public float GetSkillAttackCD() => SkillAttackCD;
    public void SetSkillAttackCD(float _value) => SkillAttackCD = _value;  
    
    public float GetSpecialAttackCD() => SpecialAttackCD;
    public void SetSpecialAttackCD(float _value) => SpecialAttackCD = _value;
    
    public List<FloatMultiplier> GetNormalAttackMultiplier() => NormalAttackMultiplier;
    public List<FloatMultiplier> GetElementalSkillMultiplier() => SkillMultiplier;
    public List<FloatMultiplier> GetElementalBurstMultiplier() => SpecialMultiplier;
    public void AddNormalAttackMultiplier() => NormalAttackMultiplier.Add(new FloatMultiplier("", new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0} )); 
    public void AddElementalSkillMultiplier() => SkillMultiplier.Add(new FloatMultiplier("", new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0} ));
    public void AddElementalBurstMultiplier() => SpecialMultiplier.Add(new FloatMultiplier("", new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0} ));
    public void RemoveNormalAttackMultiplier() => NormalAttackMultiplier.Remove(NormalAttackMultiplier[^1]);
    public void RemoveElementalSkillMultiplier() => SkillMultiplier.Remove(SkillMultiplier[^1]);
    public void RemoveElementalBurstMultiplier() => SpecialMultiplier.Remove(SpecialMultiplier[^1]);
}
