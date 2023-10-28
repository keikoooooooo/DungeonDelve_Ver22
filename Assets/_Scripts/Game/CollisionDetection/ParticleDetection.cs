using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ParticleDetection : MonoBehaviour
{
    public ParticleSystem particle;
    
    [Space] public UnityEvent<Vector3> OnParticleEnterEvent;

    
    private readonly List<ParticleCollisionEvent> _particleEvent = new();

   
    private void OnParticleCollision(GameObject other)
    {
        var nums = particle.GetCollisionEvents(other, _particleEvent);
        if(nums <= 0) 
            return;
        
        for (var i = 0; i < nums; i++)
        {
            var point = _particleEvent[i].intersection;
            OnParticleEnterEvent?.Invoke(point);
        }
        
        _particleEvent.Clear();
    }

    
    
    
}
