using UnityEngine;

public interface IDamageable
{
    
    /// <summary>
    /// Tính lượng DMG(x) của từng nhân vật và áp dụng lượng DMG này vào TakeDMG(x) trên _gameObject
    /// </summary>
    /// <param name="_gameObject"> Đối tượng bị TakeDMG (nếu có) </param>
    public void CauseDMG(GameObject _gameObject);
    
    
    /// <summary>
    /// Nhận sát thương vào
    /// </summary>
    /// <param name="_damage"> Lượng sát thương nhận vào </param>
    /// <param name="_isCRIT"> Sát thương có kích bạo không ? </param>
    public void TakeDMG(int _damage, bool _isCRIT);
    
}
