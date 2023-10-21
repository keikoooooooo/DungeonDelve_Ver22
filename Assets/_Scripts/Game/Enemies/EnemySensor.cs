using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
/// <summary>
/// Gọi Event về vị trí của Player, nếu Player đứng trong bán kính (SphereCollider)
/// </summary>
public class EnemySensor : MonoBehaviour
{
    
    [Tooltip("LayerMask cần kiểm tra va chạm")]
    public LayerMask tagetMask;
    
    public event Action E_PlayerEnter;
    public event Action E_PlayerExit;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if(!tagetMask.Contains(other.gameObject))
            return;
        
        E_PlayerEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if(!tagetMask.Contains(other.gameObject))
            return;
        
        E_PlayerExit?.Invoke();
    }
    
}
