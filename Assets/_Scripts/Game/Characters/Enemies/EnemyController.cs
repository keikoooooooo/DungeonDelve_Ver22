using System;
using NaughtyAttributes;
using NodeCanvas.Framework;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    // Ref
    [field: SerializeField, Required] public EnemyConfiguration EnemyConfig { get; private set; }
    [field: SerializeField, Required] public Blackboard Blackboard { get; private set; }
    
    // Get & Set Property 
    public StatusHandle StatusHandle { get; private set; }
    public Vector3 PlayerPosition => _player.transform.position;
    
    // Variables
    private GameObject _player;
    private float _attackCD;
    private float _skillCD;
    private float _specialCD;


    private void Awake()
    {
        StatusHandle = new StatusHandle(EnemyConfig.MaxHealth);
    }
    private void OnEnable()
    {
        StatusHandle.E_Die += Die;
        DamageableData.Add(gameObject, this);
    }
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        SetRefPlayer(_player);
        
        SetRunSpeed(EnemyConfig.RunSpeed);
        SetWalkSpeed(EnemyConfig.WalkSpeed);
    }
    private void Update()
    {
        CheckNormalAttack();
        CheckSkillAttack();
        CheckSpecialAttack();
    }
    private void OnDisable()
    {
        StatusHandle.E_Die -= Die;
        DamageableData.Remove(gameObject);
    }
    
    
    private void CheckNormalAttack() //  Check có được tấn công không ?
    {
        _attackCD = _attackCD > 0 ? _attackCD - Time.deltaTime : 0;
        SetNormalAttack(_attackCD <= 0);
    }
    private void CheckSkillAttack()
    {
        _skillCD = _skillCD > 0 ? _skillCD - Time.deltaTime : 0;
        SetSkillAttack(_skillCD <= 0);
    }
    private void CheckSpecialAttack()
    {
        _specialCD = _specialCD > 0 ? _specialCD - Time.deltaTime : 0;
        SetSpecialAttack(_specialCD <= 0);
    }
    public void ResetAttackCD() => _attackCD = EnemyConfig.NormalAttackCD; // Đặt lại thời gian tấn công
    public void ResetSkillCD() => _skillCD = EnemyConfig.SkillAttackCD;
    public void ResetSpecialCD() => _specialCD = EnemyConfig.SpecialAttackCD;
    
    
    // Set BehaviorTrees Variables    
    private void SetRefPlayer(GameObject _value) => Blackboard.SetVariableValue("Player", _value);
    private void SetWalkSpeed(float _value) => Blackboard.SetVariableValue("WalkSpeed", _value);
    private void SetRunSpeed(float _value) => Blackboard.SetVariableValue("RunSpeed", _value);
    public void SetDie(bool _value) => Blackboard.SetVariableValue("Die", _value);
    public void SetTakeDMG(bool _value) => Blackboard.SetVariableValue("TakeDMG", _value);
    public void SetRootSensor(bool _value) => Blackboard.SetVariableValue("RootRange", _value);
    public void SetChaseSensor(bool _value) => Blackboard.SetVariableValue("ChaseRange", _value);
    public void SetAttackSensor(bool _value) => Blackboard.SetVariableValue("AttackRange", _value);
    private void SetNormalAttack(bool _value) => Blackboard.SetVariableValue("NormalAttack", _value);
    private void SetSkillAttack(bool _value) => Blackboard.SetVariableValue("SkillAttack", _value);
    private void SetSpecialAttack(bool _value) => Blackboard.SetVariableValue("SpecialAttack", _value);
    
    
    #region HandleDMG
    public void CauseDMG(GameObject _gameObject)
    {
        if (DamageableData.Contains(_gameObject, out var iDamageable))
        {
            
        }
    }
    public void TakeDMG(int _damage, bool _isCRIT)
    {   
        // Tính lượng DMG thực nhận vào sau khi trừ đi lượng DEF
        var _damageReceived = Mathf.CeilToInt(_damage * (EnemyConfig.DEF / 100.0f));
        
        StatusHandle.Subtract(_damageReceived, StatusHandle.StatusType.Health);

        SetTakeDMG(true);
        DMGPopUpGenerator.Instance.Create(transform.position, _damageReceived, _isCRIT, true);
    }
    public void Die()
    {
        SetDie(true);
    }
    #endregion

 

}
