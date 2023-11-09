using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ContinuousDamage : PhysicsDetection
{
    
    [Space, Tooltip("Sát thương mỗi giây")]
    public int damagePerSecond; 
    [Tooltip("Thời gian giữa các lần gây sát thương"), Range(0.1f, 5f), SerializeField]
    private float damageCooldown;
    
    [Space, Tooltip("Có gây sát thương ở lần đầu tiên va chạm?"), SerializeField]
    private bool isFirstCollisionDamage;
    [Tooltip("Sat thương ở lần va chạm đầu?"), ShowIf("isFirstCollisionDamage")]
    public int firstDamage;


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
            
            yield return new WaitForSeconds(damageCooldown);
        }
    }


    
    
}
