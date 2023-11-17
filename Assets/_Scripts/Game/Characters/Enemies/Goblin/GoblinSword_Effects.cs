using System;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSword_Effects : MonoBehaviour
{
    public EnemyController enemyController;
    
    [Tooltip("Điểm xuất hiện effect")]
    public Transform effectPoint;

    [Tooltip("Script kiểm tra va chạm"), SerializeField]
    private PhysicsDetection physicsDetection;
    
    [Header("Prefab projectile")] 
    public Reference sword1SlashPrefab;
    public Reference sword2SlashPrefab;
    public Reference hitPrefab;
    
    [Tooltip("Offset góc quay và vị trí xuất hiện effect")] 
    public List<EffectOffset> effectOffsets;
    
    private ObjectPooler<Reference> _poolSword1Slash;
    private ObjectPooler<Reference> _poolSword2Slash;
    private ObjectPooler<Reference> _poolHit;
    private Transform slotsVFX;

    private Vector3 _posEffect;    // vị trí effect
    private Quaternion _rotEffect; // góc quay effect

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
        _poolSword1Slash = new ObjectPooler<Reference>(sword1SlashPrefab, slotsVFX, 5);
        _poolSword2Slash = new ObjectPooler<Reference>(sword2SlashPrefab, slotsVFX, 5);
        _poolHit = new ObjectPooler<Reference>(hitPrefab, slotsVFX, 5);
    }
    private void RegisterEvents()
    {
        physicsDetection.CollisionEnterEvent.AddListener(enemyController.CauseDMG);
    }
    private void UnRegisterEvents()
    {
        physicsDetection.CollisionEnterEvent.RemoveListener(enemyController.CauseDMG);
    }

    
    private void EffectAttack(AnimationEvent eEvent)
    {
        switch (eEvent.intParameter)
        {
            case 0: 
                _posEffect = effectPoint.position + transform.rotation *  effectOffsets[0].position;  
                _rotEffect = Quaternion.Euler(effectOffsets[0].rotation.x,          
                                           effectOffsets[0].rotation.y + effectPoint.eulerAngles.y,
                                             effectOffsets[0].rotation.z);
                _poolSword1Slash.Get(_posEffect, _rotEffect);
                break;
            
            case 1:
                _posEffect = effectPoint.position + transform.rotation *  effectOffsets[1].position;
                _rotEffect = Quaternion.Euler(effectOffsets[1].rotation.x, 
                                           effectOffsets[1].rotation.y + effectPoint.eulerAngles.y,
                                             effectOffsets[1].rotation.z);
                _poolSword2Slash.Get(_posEffect, _rotEffect);
                break;
        }
    }
    public void EffectHit(Vector3 _position) => _poolHit.Get(_position);
    
    
    public void CheckCollision() => physicsDetection.CheckCollision(); // gọi trên event Animation
    
}
