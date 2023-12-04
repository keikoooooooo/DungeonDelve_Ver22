using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoblinSlingshot_Effects : MonoBehaviour, ICalculateDMG
{
    public EnemyController enemyController;

    [Tooltip("Điểm xuất hiện effect")] public Transform effectPoint;

    [Header("Prefab projectile")] public EffectBase projectilePrefab;

    private ObjectPooler<EffectBase> _poolProjectile;
    private Transform slotsVFX;


    private void Awake()
    {
        InitValue();
    }
    private void OnEnable()
    {
        RegisterEvents();
    }
    private void OnDisable()
    {
        UnRegisterEvents();
    }


    private void InitValue()
    {
        slotsVFX = GameObject.FindWithTag("SlotsVFX").transform;
        _poolProjectile = new ObjectPooler<EffectBase>(projectilePrefab, slotsVFX, 5);
    }
    private void RegisterEvents()
    {
        foreach (var VARIABLE in _poolProjectile.Pool)
        {
            VARIABLE.detectionType.CollisionEnterEvent.AddListener(enemyController.CauseDMG);
        }
    }
    private void UnRegisterEvents()
    {
        foreach (var VARIABLE in _poolProjectile.Pool)
        {
            VARIABLE.detectionType.CollisionEnterEvent.RemoveListener(enemyController.CauseDMG);
        }
    }


    public void EffectAttack()
    {
        var playerPos = enemyController.PlayerPosition;
        playerPos.y += Random.Range(1f, 1.5f);
        var rotation = Quaternion.LookRotation(playerPos - effectPoint.position);
        var projectile = _poolProjectile.Get(effectPoint.position, rotation);
        projectile.FIRE();
    }
    
    public void CalculateDMG_NA()
    {
        var _level = enemyController.FindLevelIndex();
        var _percent = enemyController.EnemyConfig.GetNormalAttackMultiplier()[0].GetMultiplier()[_level];
        enemyController.ConvertDMG(_percent);
    }
    public void CalculateDMG_CA() { }
    public void CalculateDMG_EK() { }
    public void CalculateDMG_EB() { }
}
