using System;
using System.Collections;
using UnityEngine;

public class GoblinSlingshot_Effects : MonoBehaviour
{
    public EnemyController enemyController;
    
    [Tooltip("Điểm xuất hiện effect")]
    public Transform effectPoint;

    [Header("Prefab projectile")] 
    public EffectBase projectilePrefab;

    [Header("VisualEffect")]
    public ParticleSystem effectHolding;
    
    private ObjectPooler<EffectBase> _poolProjectile;
    private Transform slotsVFX;

    private Coroutine _activeEffectCoroutine;
    
    private void Start()
    {
        slotsVFX = GameObject.FindWithTag("SlotsVFX").transform;
        _poolProjectile = new ObjectPooler<EffectBase>(projectilePrefab, slotsVFX, 5);
    }

    public void EffectAttack()
    {
        var projectile = _poolProjectile.Get(effectPoint.position, effectPoint.rotation);
        projectile.FIRE();
        
        effectHolding.gameObject.SetActive(false);
        effectHolding.Stop();
    }

    private void ActiveEffect()
    {
        if(_activeEffectCoroutine != null) 
            StopCoroutine(_activeEffectCoroutine);
        _activeEffectCoroutine = StartCoroutine(ActiveEffectCoroutine());
    }
    private IEnumerator ActiveEffectCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        
        if (!enemyController.IsAttackRange)
        {
            yield break;
        }
        effectHolding.gameObject.SetActive(true);
        effectHolding.Play();
    }
    
    
    
}
