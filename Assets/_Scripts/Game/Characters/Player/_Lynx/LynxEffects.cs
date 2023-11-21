using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LynxEffects : MonoBehaviour
{
    [Tooltip("Script điều khiển chính"), SerializeField]
    private LynxController lynxController;
    
    [Space(10), Tooltip("Góc độ bắn mũi tên lên"), SerializeField, Range(-40, 0)] 
    private float angleXAttack;  
    
    [Tooltip("Vị trí sẽ xuất hiện projectile")] 
    public Transform attackPoint;
    
    [Header("Prefab projectile")] 
    [SerializeField] private EffectBase arrowComboPrefab;
    [SerializeField] private EffectBase chargedPrefab;
    [SerializeField] private EffectBase chargedNoFullyPrefab;
    
    [Header("Visual Effect")]
    [SerializeField] private ParticleSystem effectHolding;
    [SerializeField] private ParticleSystem effectSpecial;
    
    private Transform slotsVFX;
    private ObjectPooler<EffectBase> _poolNormalArrow;
    private ObjectPooler<EffectBase> _poolChargedArrow;
    private ObjectPooler<EffectBase> _poolChargedNoFullyArrow;

    private Coroutine _mouseHoldTimeCoroutine;
    
    
    private float angleYAttack => lynxController.model.eulerAngles.y;
    private bool isEnemy => lynxController.DetectionEnemy;
    
    
    // Coroutine
    private Coroutine _specialCoroutine;

    

    private void Start()
    {
        InitValue();
        RegisterEvents();
    }
    private void OnDestroy()
    {
        UnRegisterEvents();
    }

    
    private void InitValue()
    {
        slotsVFX = GameObject.FindWithTag("SlotsVFX").transform; 
        _poolNormalArrow = new ObjectPooler<EffectBase>(arrowComboPrefab, slotsVFX, 15);
        _poolChargedArrow = new ObjectPooler<EffectBase>(chargedPrefab, slotsVFX, 8);
        _poolChargedNoFullyArrow = new ObjectPooler<EffectBase>(chargedNoFullyPrefab, slotsVFX, 8);
        effectSpecial.transform.SetParent(slotsVFX);
    }
    private void RegisterEvents()
    {
        foreach (var VARIABLE in _poolNormalArrow.Pool)
        {
            VARIABLE.detectionType.CollisionEnterEvent.AddListener(lynxController.CauseDMG);
        }
        foreach (var VARIABLE in _poolChargedArrow.Pool)
        {
            VARIABLE.detectionType.CollisionEnterEvent.AddListener(lynxController.CauseDMG);
        }
        foreach (var VARIABLE in _poolChargedNoFullyArrow.Pool)
        {
            VARIABLE.detectionType.CollisionEnterEvent.AddListener(lynxController.CauseDMG);
        }
    }
    private void UnRegisterEvents()
    {
        foreach (var VARIABLE in _poolNormalArrow.Pool)
        {
            VARIABLE.detectionType.CollisionEnterEvent.RemoveListener(lynxController.CauseDMG);
        }
        foreach (var VARIABLE in _poolChargedArrow.Pool)
        {
            VARIABLE.detectionType.CollisionEnterEvent.RemoveListener(lynxController.CauseDMG);
        }
        foreach (var VARIABLE in _poolChargedNoFullyArrow.Pool)
        {
            VARIABLE.detectionType.CollisionEnterEvent.RemoveListener(lynxController.CauseDMG);
        }
    }
    
    
    private void EffectArrowCombo(AnimationEvent eEvent)
    {
        var _quaternion = isEnemy ? RandomDirection() : Quaternion.Euler(angleXAttack , angleYAttack, 0f);
        var arrow = _poolNormalArrow.Get(attackPoint.position, _quaternion);
        arrow.FIRE();
        
        lynxController.AddForceAttack();
    }
    private void EffectArrowHold(AnimationEvent eEvent)
    {
        TurnOffFxHold();
        var arrow = lynxController.ChargedAttackTime >= 3.5f ? 
                            _poolChargedArrow.Get(attackPoint.position, attackPoint.rotation) : 
                            _poolChargedNoFullyArrow.Get(attackPoint.position, attackPoint.rotation) ;
        arrow.FIRE();
    }
    public void TurnOnFxHold()
    {
        effectHolding.gameObject.SetActive(true);
        effectHolding.Play();
    }
    public void TurnOffFxHold()
    {
        effectHolding.Stop();
        effectHolding.gameObject.SetActive(false);
    }

    private void Effect_Skill(AnimationEvent eEvent)
    {
        var position = attackPoint.position;
        var rotation = Quaternion.Euler(isEnemy ? -6f : angleXAttack, attackPoint.eulerAngles.y + eEvent.intParameter, attackPoint.eulerAngles.z);
        var arrow = _poolNormalArrow.Get(position, rotation);
        arrow.FIRE();
    }
    private void EffectSpecial(AnimationEvent eEvent)
    {
        if (_specialCoroutine != null) 
            StopCoroutine(_specialCoroutine);
        _specialCoroutine = StartCoroutine(ActiveFXSpecial());
    }
    private IEnumerator ActiveFXSpecial()
    {
        // In
        effectSpecial.Play();
        yield return new WaitForSeconds(.3f);
        effectSpecial.transform.position = attackPoint.position;
        effectSpecial.transform.rotation = Quaternion.Euler(-50f, angleYAttack, 0);
        effectSpecial.gameObject.SetActive(true);
        yield return new WaitForSeconds(.85f);
        
        // Out
        var maxRadius = 3f;
        for (var i = 0; i < 20; i++)
        {
            // lấy 1 vị tri ngẫu nhiên trong bán kính maxRadius
            var randomPoint = Random.insideUnitCircle * maxRadius;
            
            // Từ vị trí xuất hiện và vị trí mục tiêu tìm ngẫu nhiên 1 vị trí mới trong bk vừa tìm đc
            var currentPos = transform.position + new Vector3(randomPoint.x, 7f, randomPoint.y);
            var targetPos = lynxController.indicatorQ.transform.position + new Vector3(randomPoint.x, Random.Range(-.5f, .5f), randomPoint.y);
   
            var arrow = _poolChargedArrow.Get(currentPos, Quaternion.LookRotation(targetPos - currentPos));
            arrow.FIRE();
            yield return new WaitForSeconds(0.1f);
        }
        
        effectSpecial.gameObject.SetActive(false);
        effectSpecial.Stop();
    }
    
    private Quaternion RandomDirection()
    {
        var posTarget = lynxController.FindClosestEnemy().position;
        posTarget.y += 1.3f;
        
        var randRotX = Random.Range(-2f, 2f);
        var randRotY = Random.Range(-2f, 2f);
        return Quaternion.LookRotation(posTarget - attackPoint.transform.position) * Quaternion.Euler(randRotX, randRotY, 0);
    }

}