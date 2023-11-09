using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

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
        var playerPos = enemyController.PlayerPosition;
        playerPos.y += Random.Range(1f, 1.5f);
        var rotation = Quaternion.LookRotation(playerPos - effectPoint.position);
        var projectile = _poolProjectile.Get(effectPoint.position, rotation);
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

        var state = (SharedBool)enemyController.BehaviorTree.GetVariable("IsAttackRange");
        if (!state.Value) yield break;
        
        effectHolding.gameObject.SetActive(true);
        effectHolding.Play();
    }
    
    
    
}
