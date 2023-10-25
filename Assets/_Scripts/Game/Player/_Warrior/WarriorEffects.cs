using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class WarriorEffects : MonoBehaviour
{
    [Tooltip("Script điều khiển chính"), SerializeField] 
    private WarriorController warriorController;

    [Space, Tooltip("Vị trí sẽ xuất hiện effects")] 
    public Transform effectPoint;
    
    [Tooltip("Góc xoay của từng Effect chém")] 
    public List<Vector3> effectAngle;
    
    [Header("Prefab projectile")] 
    [SerializeField] private Reference swordSlashPrefab;
    [SerializeField] private Reference swordPrickPrefab;
    [SerializeField] private Reference swordHoldingPrefab;

    [Space]
    [SerializeField] private Reference hitPrefab;

    [Header("Visual Effects")] 
    [SerializeField] private ParticleSystem skill;
    [SerializeField] private ParticleSystem special;

    
    private GameObject slotsProjectile;
    private ObjectPooler<Reference> _poolSwordSlash;
    private ObjectPooler<Reference> _poolSwordPrick;
    private ObjectPooler<Reference> _poolSwordHolding;

    private ObjectPooler<Reference> _poolHit;
    
    private Vector3 _posEffect;
    private Quaternion _rotEffect;

    private Vector3 _posHit;
    
    // Coroutine
    private Coroutine _skillCoroutine;
    

    private void Start()
    {
        Initialized();
    }

    
    private void Initialized()
    {
        slotsProjectile = new GameObject();
        
        _poolSwordSlash = new ObjectPooler<Reference>(swordSlashPrefab, slotsProjectile.transform, 10);
        _poolSwordPrick = new ObjectPooler<Reference>(swordPrickPrefab, slotsProjectile.transform, 5);
        _poolSwordHolding = new ObjectPooler<Reference>(swordHoldingPrefab, slotsProjectile.transform, 5);
        _poolHit = new ObjectPooler<Reference>(hitPrefab, slotsProjectile.transform, 20);
    }
    
    
    private void EffectSlash(AnimationEvent eEvent)
    {
        _posEffect = effectPoint.position;
        _rotEffect = Quaternion.Euler(effectAngle[eEvent.intParameter].x, 
                                    effectAngle[eEvent.intParameter].y + effectPoint.eulerAngles.y, 
                                      effectAngle[eEvent.intParameter].z);
        
        _poolSwordSlash.Get(_posEffect, _rotEffect);
    }
    private void EffectHolding(AnimationEvent eEvent)
    {
        _posEffect = effectPoint.position;
        _rotEffect = Quaternion.Euler(-66f, -105f + effectPoint.eulerAngles.y, -122f);
        
       _poolSwordHolding.Get(_posEffect, _rotEffect);
    }
    private void EffectPrick(AnimationEvent eEvent)
    {
        _posEffect = effectPoint.position;
        _rotEffect = effectPoint.rotation;
        
        _poolSwordPrick.Get(_posEffect, _rotEffect);
    }
    
    private void EffectSkill(AnimationEvent eEvent)
    {
        skill.gameObject.SetActive(true);
        skill.Play();
        
        if(_skillCoroutine != null)
            StopCoroutine(_skillCoroutine);
        _skillCoroutine = StartCoroutine(SkillCoroutine());
    }
    private IEnumerator SkillCoroutine()
    {
        //warriorController.Buff();
        yield return new WaitForSeconds(15f);
        skill.Stop();
        skill.gameObject.SetActive(false);
        //warriorController.UnBuff();
    }

    private void EffectSpecial(AnimationEvent eEvent)
    {
        special.gameObject.SetActive(true);
        special.Play();
    }


    public void EffectHit(Vector3 _pos) =>  _poolHit.Get(RandomPosition(_pos, -.15f, .15f));
    
    
    private static Vector3 RandomPosition(Vector3 _posCurrent, float minVal, float maxVal)
    {
        return _posCurrent + new Vector3(Random.Range(minVal, maxVal), 
                                         Random.Range(minVal, maxVal), 
                                         Random.Range(minVal, maxVal));
    }
    
    
}
