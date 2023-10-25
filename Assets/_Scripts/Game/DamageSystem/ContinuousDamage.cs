using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousDamage : PhysicsDetection
{
    
    [Space, Tooltip("Sát thương mỗi giây")]
    public int damagePerSecond; 
    [Tooltip("Thời gian giữa các lần gây sát thương"), Range(0.1f, 5f), SerializeField]
    private float damageCooldown;
    
    [Space, Tooltip("Có gây sát thương ở lần đầu tiên va chạm?"), SerializeField]
    private bool isFirstCollisionDamage;
    [Tooltip("Sat thương ở lần va chạm đầu?")]
    public int firstDamage;


    private readonly List<Collider> _targetHitList = new List<Collider>();
    private Coroutine _repeatedDamageCoroutine;

    
    private void OnEnable()
    {        
        if(_repeatedDamageCoroutine != null)
            StopCoroutine(_repeatedDamageCoroutine);
        _repeatedDamageCoroutine = StartCoroutine(RepeatedDamageCoroutine());
    }
    private IEnumerator RepeatedDamageCoroutine()
    {
        while (true)
        {
            // _targetHitList.ForEach(target => CheckCollision(target, damagePerSecond));
            yield return new WaitForSeconds(damageCooldown);
        }
    }


    
    
}
