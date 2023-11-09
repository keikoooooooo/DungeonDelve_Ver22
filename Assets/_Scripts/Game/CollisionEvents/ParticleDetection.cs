using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ParticleDetection : MonoBehaviour
{
    public ParticleSystem particle;
    [Space]
    public UnityEvent<Vector3, GameObject> OnParticleEnterEvent; // trả về vị trí va chạm (vector3) và dối tượng vừa va chạm (gameObject)

    
    private readonly List<ParticleCollisionEvent> _particleEvent = new();
    
    private void OnParticleCollision(GameObject other)
    {
        _particleEvent.Clear();
        var nums = particle.GetCollisionEvents(other, _particleEvent);
        if(nums <= 0) 
            return;
        
        var point = _particleEvent[0].intersection;
        OnParticleEnterEvent?.Invoke(point, other);
    }

    
    
    
}
