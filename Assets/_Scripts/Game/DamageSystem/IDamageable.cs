using UnityEngine;

public enum AttackType
{
    NormalAttack,
    ChargedAttack,
    ElementalSkill,
    ElementalBurst
}

public interface IDamageable
{
    
    /// <summary>
    /// Tính lượng DMG(x) của từng nhân vật theo theo attackType và áp dụng lượng DMG này vào TakeDMG(x) trên _gameObject
    /// </summary>
    /// <param name="_gameObject"> Đối tượng bị TakeDMG (nếu có) </param>
    public void CauseDMG(GameObject _gameObject, AttackType _attackType);
    
    
    /// <summary>
    /// Nhận sát thương vào
    /// </summary>
    /// <param name="_damage"> Lượng sát thương nhận vào </param>
    /// <param name="_isCRIT"> Sát thương có kích bạo không ? </param>
    public void TakeDMG(int _damage, bool _isCRIT);
    
    
    /// <summary>
    /// Phần trăm sát thương của Normal Attack
    /// </summary>  
    public float PercentDMG_NA();
    
    
    /// <summary>
    /// Phần trăm sát thương của Charged Attack
    /// </summary>  
    public float PercentDMG_CA();
    
    
    /// <summary>
    /// Phần trăm sát thương của Elemental Skill
    /// </summary>  
    public float PercentDMG_ES();
    
    
    /// <summary>
    /// Phần trăm sát thương của Elemental Burst
    /// </summary>  
    public float PercentDMG_EB();
    
    
    /// <summary>
    /// Chuyển phần trăm sát thương thành sát thương đầu ra
    /// </summary>  
    public int CalculationDMG(float _percent);
    
}
