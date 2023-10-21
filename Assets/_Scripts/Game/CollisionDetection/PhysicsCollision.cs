using System;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCollision : MonoBehaviour
{
    
    [Tooltip("Bán kính kiểm tra va chạm"), Range(0.1f, 5f)]
    public float radiusCheck;
    
    [Tooltip("Layer cần kiểm tra va chạm")]
    public LayerMask layerToCheck;
    
    public event Action<Vector3> OnCollisionEvent;
    
    
    private readonly Collider[] hitColliders = new Collider[20];
    


    public void CheckCollision()
    {
        var numCol = Physics.OverlapSphereNonAlloc(transform.position, radiusCheck, hitColliders, layerToCheck);

        if (numCol != 0)
            isDetec = true;
        
        for (var i = 0; i < numCol; i++)
        {
            Debug.Log(hitColliders[i].gameObject.name);
            
            
            var point = hitColliders[i].ClosestPointOnBounds(transform.position);
            detecList.Add(point);
            
            OnCollisionEvent?.Invoke(point);
        }
    }
    
    
    #region Debug Detection
    private readonly List<Vector3> detecList = new();
    private bool isDetec;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radiusCheck);

        if (!isDetec) 
            return;
        
        foreach (var VARIABLE in detecList)
        {
            Gizmos.color = Color.red;   
            Gizmos.DrawWireSphere(VARIABLE, .1f);
        }
        Invoke(nameof(Test), 1.2f);
    }
    private void Test()
    {
        isDetec = false;
        detecList.Clear();
    }
    #endregion
}
