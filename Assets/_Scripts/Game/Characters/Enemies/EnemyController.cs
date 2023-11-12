using BehaviorDesigner.Runtime;
using NaughtyAttributes;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorTree))]
public class EnemyController : MonoBehaviour, IDamageable
{
    // Ref
    //[field: SerializeField] public BehaviorTree BehaviorTree { get; private set; }
    [field: SerializeField] public EnemyConfiguration EnemyConfig { get; private set; }
    [field: SerializeField, Required] public Blackboard Blackboard { get; private set; }
    
    
    // Get & Set Property 
    public StatusHandle StatusHandle { get; private set; }
    public Vector3 PlayerPosition => _player.transform.position;
    
    // Variables
    private GameObject _player;
    private float _attackCD;
    private float _skillCD;
    private float _specialCD;
    

    private void OnEnable()
    {
        SetVariables();
    }
    private void Start()
    {
        GetReference();
    }
    private void Update()
    {
        CheckAttack();
    }
    private void OnDisable()
    {
        ResetVariables();
    }
    
    private void SetVariables()
    {
        StatusHandle = new StatusHandle(EnemyConfig.MaxHealth);
        StatusHandle.E_Die += Die;
        DamageableData.Add(gameObject, this);

        SetWalkSpeed(EnemyConfig.WalkSpeed);
        SetRunSpeed(EnemyConfig.RunSpeed);
    }
    private void ResetVariables()
    { 
        StatusHandle.E_Die -= Die;
        DamageableData.Remove(gameObject);
    }
    private void GetReference()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        // var value = (SharedFloat)BehaviorTree.GetVariable("AttackCooldown");
        // _attackCooldownTemp = value.Value;
        // _attackCooldownTemp = EnemyConfig.
        
        Blackboard.SetVariableValue("Player", _player);
        
        // BehaviorTree.SetVariableValue("RootPosition", transform.position);
        // if (_player) 
        //     BehaviorTree.SetVariableValue("Player", _player);
    }
    
    
    private void CheckAttack()
    {
        //BehaviorTree.SetVariableValue("CanAttack", CanAttack);
        _attackCD = _attackCD > 0 ? _attackCD - Time.deltaTime : 0;
        SetNormalAttack(_attackCD <= 0);
    }
    
    
    // public void SetDie(bool isType) => BehaviorTree.SetVariableValue("Die", isType);
    // public void SetTakeDMG(bool isType) => BehaviorTree.SetVariableValue("TakeDMG", isType);
    // public void SetRootSensor(bool isType) => BehaviorTree.SetVariableValue("IsRootRange", isType);
    // public void SetChaseSensor(bool isType) => BehaviorTree.SetVariableValue("IsChaseRange", isType);
    // public void SetAttackSensor(bool isType) => BehaviorTree.SetVariableValue("IsAttackRange", isType);
    
    
    // Set BehaviorTrees Variables
    public void ResetAttackCD() => _attackCD = EnemyConfig.AttackCD; // Đặt lại thời gian tấn công
    public void ResetSkillCD() => _skillCD = EnemyConfig.SkillCD;
    public void ResetSpecialCD() => _specialCD = EnemyConfig.SpecialCD;
    private void SetWalkSpeed(float _value) => Blackboard.SetVariableValue("WalkSpeed", _value);
    private void SetRunSpeed(float _value) => Blackboard.SetVariableValue("RunSpeed", _value);
    
    public void SetDie(bool _isType) => Blackboard.SetVariableValue("Die", _isType);
    public void SetTakeDMG(bool _isType) => Blackboard.SetVariableValue("TakeDMG", _isType);
    public void SetRootSensor(bool _isType) => Blackboard.SetVariableValue("RootRange", _isType);
    public void SetChaseSensor(bool _isType) => Blackboard.SetVariableValue("ChaseRange", _isType);
    public void SetAttackSensor(bool _isType) => Blackboard.SetVariableValue("AttackRange", _isType);
    
    private void SetNormalAttack(bool _isType) => Blackboard.SetVariableValue("NormalAttack", _isType);
    private void SetSkillAttack(bool _isType) => Blackboard.SetVariableValue("SkillAttack", _isType);
    private void SetSpecialAttack(bool _isType) => Blackboard.SetVariableValue("SpecialAttack", _isType);
    
    // public float RandomSliderValue(float _minValue, float _maxValue) => Random.Range(_minValue, _maxValue);
    // public Vector3 RandomUnitCirclePosition(float _radius)
    // {
    //     var point = Random.insideUnitCircle * _radius;
    //     return transform.position + new Vector3(point.x, 0, point.y);
    // }

    
    
    #region HandleDMG
    public void CauseDMG(GameObject _gameObject)
    {
        
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
