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
    public Transform effectPoint;
    
    [Tooltip("Góc xoay của từng Effect Slash trên Normal Attack")] 
    public List<EffectOffset> effectOffsets;
    
    [Space] 
    public Reference slashPrefab;
    public Reference skillPrefab;
    public Reference hitPrefab;

    private ObjectPooler<Reference> _poolSlash;
    private ObjectPooler<Reference> _poolSkill;
    private ObjectPooler<Reference> _poolHit;
    private Transform slotVFX;

    private Vector3 _posEffect;
    private Quaternion _rotEffect;
    
    private void Start()
    {
        slotVFX = GameObject.FindGameObjectWithTag("SlotsVFX").transform;

        _poolSlash = new ObjectPooler<Reference>(slashPrefab, slotVFX, 10);
        _poolSkill = new ObjectPooler<Reference>(skillPrefab, slotVFX, 2);
        
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
