using System;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsDetection : MonoBehaviour
{
    
    [Tooltip("Bán kính kiểm tra va chạm"), Range(0.1f, 5f)]
    public float radiusCheck;
    
    [Tooltip("Layer cần kiểm tra va chạm")]
    public LayerMask layerToCheck;

    [Space]
    public UnityEvent<Vector3, GameObject> OnPhysicEnterEvent;
    
    private readonly Collider[] hitColliders = new Collider[20];
    


    public void CheckCollision()
    {
        var numCol = Physics.OverlapSphereNonAlloc(transform.position, radiusCheck, hitColliders, layerToCheck);
        for (var i = 0; i < numCol; i++)
        {
            var point = hitColliders[i].ClosestPointOnBounds(transform.position);
            OnPhysicEnterEvent?.Invoke(point, hitColliders[i].gameObject);
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusCheck);
    }
}
