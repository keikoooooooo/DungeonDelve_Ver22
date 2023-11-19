using System;
using UnityEngine;
using System.Collections.Generic;

public class ReaperEffects : MonoBehaviour
{
    [Tooltip("Effect Ping NormalAttack")]
    public GameObject indicatorNormalAttack;
    [Tooltip("Effect Ping Skill")]
    public GameObject indicatorSkill;
    
    [Tooltip("Vị trí xuất hiện Attack Effect")]
    public Transform effectPosition;
    
    [Tooltip("Góc xoay của từng Effect Slash trên Normal Attack")] 
    public List<EffectOffset> effectOffsets;

    [Header("Prefab projectile")] 
    public Reference indicatorPrefab;
    public Reference slashPrefab;
    public Reference specialPrefab;
    public Reference hitPrefab;

    [Space, Header("Visual Effect")]
    public ParticleSystem skillEffect;

    private ObjectPooler<Reference> _poolIndicator;
    private ObjectPooler<Reference> _poolSlash;
    private ObjectPooler<Reference> _poolSpecial;
    private ObjectPooler<Reference> _poolHit;

    private Transform slotVFX;
    private Vector3 _posEffect;
    private Quaternion _rotEffect;
    
    
    private void Start()
    {
        slotVFX = GameObject.FindGameObjectWithTag("SlotsVFX").transform;

        _poolIndicator = new ObjectPooler<Reference>(indicatorPrefab, slotVFX, 15);
        _poolSpecial = new ObjectPooler<Reference>(specialPrefab, slotVFX, 15);
        _poolSlash = new ObjectPooler<Reference>(slashPrefab, slotVFX, 10);
        
        skillEffect.transform.SetParent(slotVFX);
        indicatorSkill.transform.SetParent(slotVFX);
        indicatorNormalAttack.transform.SetParent(slotVFX);
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
    
    
    
    
}
