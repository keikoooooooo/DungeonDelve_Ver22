using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ReaperEffects : MonoBehaviour
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

    [Space, Header("Set Material")]
    public M_SetFloat edgeWidth;
    public M_SetFloat noiseScale;
    public M_SetFloat dissolve;
    
    private ObjectPooler<Reference> _poolIndicator;
    private ObjectPooler<Reference> _poolSlash;
    private ObjectPooler<PhysicsDetection> _poolSpecial;
    private ObjectPooler<Reference> _poolHit;

    private Transform slotVFX;
    private Vector3 _posEffect;
    private Quaternion _rotEffect;
    private Coroutine _enableDissolve;

    private void OnEnable()
    {
        if(_enableDissolve != null) StopCoroutine(_enableDissolve);
        _enableDissolve = StartCoroutine(EnableDissolveCoroutine());
    }
    private void Start()
    {
        Initialized();
        RegisterEvents();
    }
    private void OnDestroy()
    {
        UnRegisterEvents();
    }
    

    private IEnumerator EnableDissolveCoroutine()
    {
        // hòa tan: tắt
        edgeWidth.ChangeValueSet(.4f);
        edgeWidth.Apply();
        
        noiseScale.ChangeValueSet(22);
        noiseScale.Apply();
        
        dissolve.ChangeCurrentValue(0);
        dissolve.ChangeValueSet(1);
        dissolve.ChangeDurationApply(0);
        dissolve.Apply();

        yield return new WaitForSeconds(2.85f);
        
        // hòa tan: mở
        dissolve.ChangeCurrentValue(1);
        dissolve.ChangeValueSet(0);
        dissolve.ChangeDurationApply(3f);
        dissolve.Apply();
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
        foreach (var VARIABLE in _poolSpecial.Pool)
        {
            VARIABLE.CollisionEnterEvent.AddListener(enemyController.CauseDMG);
            VARIABLE.PositionEnterEvent.AddListener(EffectHit);
        }
    }
    private void UnRegisterEvents()
    {
        foreach (var VARIABLE in _poolSpecial.Pool)
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
    public void EffectHit(Vector3 _pos) => _poolHit.Get(RandomPosition(_pos, .15f, .4f));
    private static Vector3 RandomPosition(Vector3 _posCurrent, float minVal, float maxVal)
    {
        return _posCurrent + new Vector3(Random.Range(minVal, maxVal), 
            Random.Range(minVal, maxVal), 
            Random.Range(minVal, maxVal));
    }
}