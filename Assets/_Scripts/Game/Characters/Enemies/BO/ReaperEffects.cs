using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class ReaperEffects : MonoBehaviour
{
    public GameObject indicatorSkill;
    public GameObject indicatorNormalAttack;
    public Transform effectPoint;
    
    [Tooltip("Góc xoay của từng Effect Slash trên Normal Attack")] 
    public List<EffectOffset> effectOffsets;
    
    [Space] 
    public Reference slashPrefab;
    public Reference hitPrefab;

    private ObjectPooler<Reference> _poolSlash;
    private ObjectPooler<Reference> _poolHit;
    private Transform slotVFX;

    private Vector3 _posEffect;
    private Quaternion _rotEffect;
    
    private void Start()
    {
        slotVFX = GameObject.FindGameObjectWithTag("SlotsVFX").transform;

        _poolSlash = new ObjectPooler<Reference>(slashPrefab, slotVFX, 10);
        //_poolHit = new ObjectPooler<Reference>(hitPrefab, slotVFX, 10);
        indicatorSkill.transform.SetParent(slotVFX);
        indicatorNormalAttack.transform.SetParent(slotVFX);
    }

    private void EffectSlashNA(AnimationEvent eEvent)
    {
        _posEffect = effectPoint.position + effectPoint.rotation * effectOffsets[eEvent.intParameter].position;
        _rotEffect = Quaternion.Euler(effectOffsets[eEvent.intParameter].rotation.x ,
                                    effectOffsets[eEvent.intParameter].rotation.y + effectPoint.eulerAngles.y,
                                      effectOffsets[eEvent.intParameter].rotation.z );
        
        _poolSlash.Get(_posEffect, _rotEffect);
    }
    
    
}
