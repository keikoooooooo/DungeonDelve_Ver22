using System;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    [Tooltip("Layer cần kiểm tra va chạm")]
    public LayerMask layerToCheck;
    
    [Space]
    public UnityEvent<Collision> OnCollisionEnterEvent;

    private void OnCollisionEnter(Collision other)
    {
        if(!layerToCheck.Contains(other.gameObject)) return;
        
        OnCollisionEnterEvent?.Invoke(other);
    }
}
