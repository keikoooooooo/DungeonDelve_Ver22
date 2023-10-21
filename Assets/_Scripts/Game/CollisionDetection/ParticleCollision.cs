using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour, IPooled<ParticleCollision>
{
    public new ParticleSystem particleSystem;
    
    public event Action<Vector3> OnCollisionEvent;

    
    private readonly List<ParticleCollisionEvent> _particleEvent = new();

   
    private void OnParticleCollision(GameObject other)
    {
        var nums = particleSystem.GetCollisionEvents(other, _particleEvent);
        if(nums <= 0) 
            return;
        
        for (var i = 0; i < nums; i++)
        {
            var point = _particleEvent[i].intersection;
            OnCollisionEvent?.Invoke(point);
        }
        
        _particleEvent.Clear();
        Release();
    }

    
    
    
    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<ParticleCollision> ReleaseCallback { get; set; }
}
