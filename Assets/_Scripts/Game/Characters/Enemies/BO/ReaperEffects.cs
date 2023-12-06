using System.Collections;
using UnityEngine;
using System.Collections.Generic;



public class ReaperEffects : MonoBehaviour, ICalculateDMG
{
    [SerializeField] private EnemyController enemyController;
    public PhysicsDetection physicsDetection;
    
    [Tooltip("Effect Ping NormalAttack")] public GameObject indicatorNormalAttack;
    [Tooltip("Effect Ping Skill")] public GameObject indicatorSkill;
    
    [Tooltip("Vị trí xuất hiện Attack Effect")]
    public Transform effectPosition;
    
    [Tooltip("Góc xoay của từng Effect Slash trên Normal Attack")] 
    public List<EffectOffset> effectOffsets;

    [Header("Prefab projectile")] 
    public Reference indicatorPrefab;
    public Reference slashPrefab;
    public Reference hitPrefab;
    public PhysicsDetection specialPrefab;

    [Header("Visual Effect")]
    public ParticleSystem skillEffect;
    
    private ObjectPooler<Reference> _poolIndicator;
    private ObjectPooler<Reference> _poolSlash;
    private ObjectPooler<PhysicsDetection> _poolSpecial;
    private ObjectPooler<Reference> _poolHit;

    private int _attackCounter;
    private Transform slotVFX;
    private Vector3 _posEffect;
    private Quaternion _rotEffect;
    private Coroutine _enableDissolve;


    private void Start()
    {
        Initialized();
        RegisterEvents();
    }
    private void OnDestroy()
    {
        UnRegisterEvents();
    }
    

    private void Initialized()
    {
        slotVFX = GameObject.FindGameObjectWithTag("SlotsVFX").transform;

        _poolIndicator = new ObjectPooler<Reference>(indicatorPrefab, slotVFX, 15);
        _poolSlash = new ObjectPooler<Reference>(slashPrefab, slotVFX, 10);
        _poolHit = new ObjectPooler<Reference>(hitPrefab, slotVFX, 35);
        _poolSpecial = new ObjectPooler<PhysicsDetection>(specialPrefab, slotVFX, 50);
        
        skillEffect.transform.SetParent(slotVFX);
        indicatorSkill.transform.SetParent(slotVFX);
        indicatorNormalAttack.transform.SetParent(slotVFX);
    }

    private void RegisterEvents()
    {
        foreach (var VARIABLE in _poolSpecial.List)
        {
            VARIABLE.CollisionEnterEvent.AddListener(enemyController.CauseDMG);
            VARIABLE.PositionEnterEvent.AddListener(EffectHit);
        }
    }
    private void UnRegisterEvents()
    {
        foreach (var VARIABLE in _poolSpecial.List)
        {
            VARIABLE.CollisionEnterEvent.RemoveListener(enemyController.CauseDMG);
            VARIABLE.PositionEnterEvent.RemoveListener(EffectHit);
        }
    }
    
    
    private void EffectSlashNA(AnimationEvent eEvent)
    {
        _posEffect = effectPosition.position + effectPosition.rotation * effectOffsets[eEvent.intParameter].position;
        _rotEffect = Quaternion.Euler(effectOffsets[eEvent.intParameter].rotation.x ,
                                    effectOffsets[eEvent.intParameter].rotation.y + effectPosition.eulerAngles.y,
                                      effectOffsets[eEvent.intParameter].rotation.z );
        
        _poolSlash.Get(_posEffect, _rotEffect);
    }
    public void GetIndicator(Vector3 _position) => _poolIndicator.Get(_position);
    public void GetSpecialEffect(Vector3 _position) => _poolSpecial.Get(_position);
    
    public void CheckCollision() => physicsDetection.CheckCollision(); // gọi trên event Animation
    public void EffectHit(Vector3 _pos) => _poolHit.Get(_pos + new Vector3(Random.Range(-.1f, .1f), Random.Range(.5f, 1f), 0));
    

    public void SetAttackCounter(int count) => _attackCounter = count; // Set đòn đánh thứ (x), gọi trên animationEvent
    public void CalculateDMG_NA()
    {
        var _level = enemyController.FindLevelIndex();
        var _percent = enemyController.EnemyConfig.GetNormalAttackMultiplier()[_attackCounter].GetMultiplier()[_level];
        enemyController.ConvertDMG(_percent);
    }
    public void CalculateDMG_CA() { }
    public void CalculateDMG_EK()
    {
        var _level = enemyController.FindLevelIndex();
        var _percent = enemyController.EnemyConfig.GetElementalSkillMultiplier()[0].GetMultiplier()[_level];
        enemyController.ConvertDMG(_percent);
    }
    public void CalculateDMG_EB()
    {
        var _level = enemyController.FindLevelIndex();
        var _percent = enemyController.EnemyConfig.GetElementalBurstMultiplier()[0].GetMultiplier()[_level];
        enemyController.ConvertDMG(_percent);
    }
}
