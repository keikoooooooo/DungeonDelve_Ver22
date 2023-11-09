using BehaviorDesigner.Runtime;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorTree))]
public class EnemyController : MonoBehaviour, IDamageable
{
    // Ref
    [field: SerializeField, Required] public BehaviorTree BehaviorTree { get; private set; }
    [field: SerializeField] public EnemyConfiguration EnemyConfig { get; private set; }
    
    
    // Get & Set Property 
    public HealthHandle HealthHandle { get; private set; }
    public bool CanAttack => _attackCooldown <= 0;
    public Vector3 PlayerPosition => _player.transform.position;
    
    
    // Variables
    private GameObject _player;
    private float _attackCooldown;
    private float _attackCooldownTemp;
    
    
    private void Start()
    {
        GetReference();
    }
    private void Update()
    {
        CheckAttack();
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
    
    
    
    public void TakeDamage(int damage)
    {
        Debug.Log("take damage");
    }
    
 

}
