using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArcherEffects : MonoBehaviour
{
    [Tooltip("Script điều khiển chính"), SerializeField]
    private ArcherController _archerController;
    
    [Space(10), Tooltip("Góc độ bắn mũi tên lên"), SerializeField, Range(-40, 0)] 
    private float angleXAttack;  
    
    [Tooltip("Vị trí sẽ xuất hiện projectile")] 
    public Transform attackPoint;
    
    [Header("Prefab projectile")] 
    [SerializeField] private ParticleCollision arrowComboPrefab;
    [SerializeField] private ParticleCollision arrowHoldingPrefab;
    [Space]
    [SerializeField] private Reference flashPrefab;
    [SerializeField] private Reference hitPrefab;
    [Space]
    [SerializeField] private ParticleSystem arrowSpecial_Flash;
    [SerializeField] private ParticleSystem arrowSpecial_IN;
    [SerializeField] private ParticleSystem arrowSpecial_OUT;

    
    [Header("Effect holding attack")]
    [Tooltip("Tại vị trí bắn"), SerializeField]
    private ParticleSystem fxHoldAttack1;
    [Tooltip("Tại vị trí nhân vật"), SerializeField]
    private ParticleSystem fxHoldAttack2;

    
    private GameObject slotsProjectile;
    private ObjectPooler<ParticleCollision> _poolArrowCombo;
    private ObjectPooler<ParticleCollision> _poolArrowHold;
    
    private ObjectPooler<Reference> _poolFlash;
    private ObjectPooler<Reference> _poolHit;

    

    private float angleYAttack => _archerController.model.eulerAngles.y;
    private bool isEnemy => _archerController.playerSensor.target != null;
    
    
    // Coroutine
    private Coroutine _specialCoroutine;
    
    
    private void Start()
    {
        Initialized();
        
        RegisterEvent();
    }

    private void OnDestroy()
    {
        UnRegisterEvent();
    }

    private void Initialized()
    {
        slotsProjectile = new GameObject();
        _poolArrowCombo = new ObjectPooler<ParticleCollision>(arrowComboPrefab, slotsProjectile.transform, 10);
        _poolArrowHold = new ObjectPooler<ParticleCollision>(arrowHoldingPrefab, slotsProjectile.transform, 5);
        _poolFlash = new ObjectPooler<Reference>(flashPrefab, slotsProjectile.transform, 7);
        _poolHit = new ObjectPooler<Reference>(hitPrefab, slotsProjectile.transform, 7);

        arrowSpecial_IN.transform.SetParent(slotsProjectile.transform);
        arrowSpecial_OUT.transform.SetParent(slotsProjectile.transform);
    }

    private void RegisterEvent()
    {
        foreach (var particle in _poolArrowCombo.Pool)
        {
            particle.OnCollisionEvent += EffectHit;
        }
        foreach (var particle in _poolArrowHold.Pool)
        {
            particle.OnCollisionEvent += EffectHit;
        }

    }
    private void UnRegisterEvent()
    {
        foreach (var particle in _poolArrowCombo.Pool)
        {
            particle.OnCollisionEvent -= EffectHit;
        }
        foreach (var particle in _poolArrowHold.Pool)
        {
            particle.OnCollisionEvent -= EffectHit;
        }
    }
    


    private void EffectArrowCombo(AnimationEvent eEvent)
    {
        var arrow = _poolArrowCombo.Get(attackPoint.position);
        //arrow.transform.rotation = attackPoint.rotation;
        arrow.transform.rotation = isEnemy ? RandomDirection() : Quaternion.Euler(angleXAttack , angleYAttack, 0f);
        //arrow.transform.rotation = Quaternion.Euler(angleXAttack , angleYAttack, 0f);

        _poolFlash.Get(attackPoint.position, arrow.transform.rotation);
    }
    private void EffectArrowHold(AnimationEvent eEvent)
    {
        //TurnOffFxHold();
        
        var arrow = _poolArrowHold.Get(attackPoint.position);
        arrow.transform.rotation = attackPoint.rotation;
   
    }
    public void TurnOnFxHold()
    {
        fxHoldAttack1.gameObject.SetActive(true);
        fxHoldAttack1.Play();
        fxHoldAttack2.gameObject.SetActive(true);
        fxHoldAttack2.Play();
    }
    public void TurnOffFxHold()
    {
        fxHoldAttack1.Stop();
        fxHoldAttack1.gameObject.SetActive(false);
        fxHoldAttack2.Stop();
        fxHoldAttack2.gameObject.SetActive(false);
    }

    private void VisualEffect_Skill(AnimationEvent eEvent)
    {
        var arrow = _poolArrowCombo.Get(attackPoint.position);
        if (isEnemy)
        {
            arrow.transform.rotation = RandomDirection();
        }
        else
        {
            var eulerAngles = attackPoint.eulerAngles;
            var rotY = eulerAngles.y + eEvent.floatParameter;
            arrow.transform.rotation  = Quaternion.Euler(angleXAttack, rotY, eulerAngles.z);
        }
        
    }
    
    private void Special(AnimationEvent eEvent)
    {
        if (_specialCoroutine != null) 
            StopCoroutine(_specialCoroutine);
        _specialCoroutine = StartCoroutine(ActiveFXSpecial());
    }
    private IEnumerator ActiveFXSpecial()
    {
        // Flash
        arrowSpecial_Flash.gameObject.SetActive(true);
        arrowSpecial_Flash.Play();
        yield return new WaitForSeconds(.15f);
        
        // In
        arrowSpecial_IN.gameObject.SetActive(true);
        arrowSpecial_IN.Play();
        yield return new WaitForSeconds(.3f);
        arrowSpecial_IN.transform.position = attackPoint.position;
        arrowSpecial_IN.transform.rotation = Quaternion.Euler(-75f, angleYAttack, 0);
        yield return new WaitForSeconds(.7f);
        
        // Out
        arrowSpecial_OUT.gameObject.SetActive(true);
        // var newPos = new Vector3(_archerController.targetMarkerQ.transform.position.x, _archerController.transform.position.y, _archerController.targetMarkerQ.transform.position.z);
        // arrowSpecial_OUT.gameObject.transform.position = newPos;
        arrowSpecial_OUT.gameObject.transform.position = _archerController.targetMarkerQ.transform.position;
        arrowSpecial_OUT.Play();
        yield return new WaitForSeconds(3f);
        
        // Release
        arrowSpecial_Flash.gameObject.SetActive(false);
        arrowSpecial_IN.gameObject.SetActive(false);
        arrowSpecial_OUT.gameObject.SetActive(false);
    }
    
    

    private void EffectHit(Vector3 _pos)
    {
        _poolHit.Get(RandomPosition(_pos, -.15f, .15f));
    }
    private static Vector3 RandomPosition(Vector3 _posCurrent, float minVal, float maxVal)
    {
        return _posCurrent + new Vector3(Random.Range(minVal, maxVal), 
                                         Random.Range(minVal, maxVal), 
                                         Random.Range(minVal, maxVal));
    }
    
    
    private Quaternion RandomDirection()
    {
        var posTarget = _archerController.playerSensor.target.transform.position;
        posTarget.y += 1.5f;
        var randRotX = Random.Range(-2f, 2f);
        var randRotY = Random.Range(-1.5f, 1.5f);
        //return Quaternion.LookRotation(posTarget - attackPoint.transform.position) * Quaternion.Euler(randRotX, randRotY, 0);
        return Quaternion.LookRotation(posTarget - attackPoint.transform.position);
    }

}