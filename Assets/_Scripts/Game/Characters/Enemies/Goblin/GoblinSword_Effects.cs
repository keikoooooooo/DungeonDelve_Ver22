using System.Collections.Generic;
using UnityEngine;

public class GoblinSword_Effects : MonoBehaviour, IAttack
{
    public EnemyController enemyController;
    
    [Tooltip("Điểm xuất hiện effect")]
    public Transform effectPoint;

    [Tooltip("Script kiểm tra va chạm"), SerializeField]
    private PhysicsDetection NA_Detection;
    
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

    private int _attackCounter;
    private Vector3 _posEffect;    // vị trí effect
    private Quaternion _rotEffect; // góc quay effect

    
    private void Start()
    {
        InitValue();
    }
    private void InitValue()
    {
        slotsVFX = GameObject.FindWithTag("SlotsVFX").transform;
        _poolSword1Slash = new ObjectPooler<Reference>(sword1SlashPrefab, slotsVFX, 5);
        _poolSword2Slash = new ObjectPooler<Reference>(sword2SlashPrefab, slotsVFX, 5);
        _poolHit = new ObjectPooler<Reference>(hitPrefab, slotsVFX, 5);
    }


    
    public void SetAttackCounter(int count) => _attackCounter = count; // Set đòn đánh thứ (x), gọi trên animationEvent
    private void EffectAttack(AnimationEvent eEvent)
    {
        var index = eEvent.intParameter;
        _posEffect = effectPoint.position + transform.rotation *  effectOffsets[index].position;
        _rotEffect = Quaternion.Euler(effectOffsets[index].rotation.x, 
                                    effectOffsets[index].rotation.y + effectPoint.eulerAngles.y,
                                      effectOffsets[index].rotation.z);
        _poolSword2Slash.Get(_posEffect, _rotEffect);
    }
    public void EffectHit(Vector3 _position) => _poolHit.Get(_position);
    
    public void CheckNACollision() => NA_Detection.CheckCollision(); // gọi trên event Animation
    
    
    public void Detection_NA(GameObject _gameObject) => enemyController.CauseDMG(_gameObject, AttackType.NormalAttack);
    public void Detection_CA(GameObject _gameObject) { }
    public void Detection_EK(GameObject _gameObject) { }
    public void Detection_EB(GameObject _gameObject) { }
}
