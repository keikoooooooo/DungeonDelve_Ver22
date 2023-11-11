using BehaviorDesigner.Runtime;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorTree))]
public sealed class EnemyController : MonoBehaviour, IDamageable
{
    // Ref
    [field: SerializeField, Required] public BehaviorTree BehaviorTree { get; private set; }
    [field: SerializeField] public EnemyConfiguration EnemyConfig { get; private set; }
    
    
    // Get & Set Property 
    public StatusHandle StatusHandle { get; private set; }
    public bool CanAttack => _attackCooldown <= 0;
    public Vector3 PlayerPosition => _player.transform.position;

    public Animator animator;
    
    // Variables
    private GameObject _player;
    private float _attackCooldown;
    private float _attackCooldownTemp;


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
        DamageableData.Add(gameObject, this);
    }
    private void ResetVariables()
    {
        DamageableData.Remove(gameObject);
    }
    private void GetReference()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        var value = (SharedFloat)BehaviorTree.GetVariable("AttackCooldown");
        _attackCooldownTemp = value.Value;
        
        BehaviorTree.SetVariableValue("RootPosition", transform.position);
        if (_player) BehaviorTree.SetVariableValue("Player", _player);
    }
    
    
    private void CheckAttack()
    {
        BehaviorTree.SetVariableValue("CanAttack", CanAttack);
        _attackCooldown = _attackCooldown > 0 ? _attackCooldown - Time.deltaTime : 0;
    }
    
    
    // Gọi bằng InvokeMethods trên Tree
    public void ResetAttackTime() => _attackCooldown = _attackCooldownTemp; // Đặt lại thời gian tấn công

        
    // Gọi bằng Events
    public void SetRootSensor(bool isType) => BehaviorTree.SetVariableValue("IsRootRange", isType);
    public void SetChaseSensor(bool isType) => BehaviorTree.SetVariableValue("IsChaseRange", isType);
    public void SetAttackSensor(bool isType) => BehaviorTree.SetVariableValue("IsAttackRange", isType);
    
    
    
    #region HandleDMG
    public void CauseDMG(GameObject _gameObject)
    {
        
    }
    public void TakeDMG(int _damage, bool _isCRIT)
    {   
        // Tính lượng DMG thực nhận vào sau khi trừ đi lượng DEF
        var _damageReceived = Mathf.CeilToInt(_damage * (EnemyConfig.DEF / 100.0f));
        
        DMGPopUpGenerator.Instance.CreateDMGPopUp(_damageReceived, transform.position, _isCRIT);
    }
    public void Die()
    {
        
    }

    #endregion

 

}
