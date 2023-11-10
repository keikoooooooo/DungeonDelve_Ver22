using System.Collections.Generic;
using UnityEngine;

public class ParticleDetection : DetectionBase
{
    [Space]
    public ParticleSystem particle;
    
    private readonly List<ParticleCollisionEvent> _particleEvent = new();
    
    private void OnParticleCollision(GameObject other)
    {
        _particleEvent.Clear();
        var nums = particle.GetCollisionEvents(other, _particleEvent);
        if(nums <= 0) 
            return;
        
        CollisionEnterEvent?.Invoke(other);
        PositionEnterEvent?.Invoke(_particleEvent[0].intersection);
    }

    
    
    
}
