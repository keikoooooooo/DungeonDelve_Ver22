using UnityEngine;

public interface IDamageable
{
    
    /// <summary>
    /// Áp dụng lượng DMG vừa tính và gọi TakeDMG(?) trên _gameObject
    /// </summary>
    /// <param name="_gameObject"> Đối tượng bị TakeDMG </param>
    public void CauseDMG(GameObject _gameObject);
    
    
    /// <summary>
    /// Nhận sát thương vào
    /// </summary>
    /// <param name="_damage"> Lượng sát thương nhận vào </param>
    /// <param name="_isCRIT"> Sát thương có kích bạo không ? </param>
    public void TakeDMG(int _damage, bool _isCRIT);
    
    
    /// <summary>
    /// Nhân vật Die
    /// </summary>
    public void Die();
    

}
