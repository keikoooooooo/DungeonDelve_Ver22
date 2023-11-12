using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HandleRagdoll : MonoBehaviour
{
    private List<Rigidbody> _rigidbodies;

    public UnityEvent OnEnableRagdoll;
    public UnityEvent OnDisableRagdoll;
    
    private void Awake()
    { 
        _rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
        DisableRagdoll();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                var forDirec = transform.position - Camera.main.transform.position;
                forDirec.y += 10;
                forDirec.Normalize();

                var force = 250f * forDirec;
                var hitRb = _rigidbodies.OrderBy(rb => Vector3.Distance(rb.position, hit.point)).First();

                EnableRagdoll();
                hitRb.AddForceAtPosition(force, hit.point, ForceMode.Impulse);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            DisableRagdoll();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            EnableRagdoll();
        }
    }


    public void EnableRagdoll()
    {
        _rigidbodies.ForEach(rb => rb.isKinematic = false);
        OnEnableRagdoll?.Invoke();
    }
    public void DisableRagdoll()
    {
        _rigidbodies.ForEach(rb => rb.isKinematic = true);
        OnDisableRagdoll?.Invoke();
    }
    
    
    
}
