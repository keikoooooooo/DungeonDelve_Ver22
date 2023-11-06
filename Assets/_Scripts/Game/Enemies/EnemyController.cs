using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorTree))]
public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Reference")]
    [Tooltip("Cây hành vi của Enemy"), SerializeField]
    private BehaviorTree behaviorTree;
        
    [Tooltip("Bộ phát animation"), SerializeField]
    private Animator animator;
    
    

    // Variables
    public bool IsChaseRange { get; private set; }
    public bool IsAttackRange { get; private set; }
    public bool CanAttack { get; private set; }
    private float _attackCooldown;
    
    
    private GameObject _player;
    private Vector3 _rootPosition;
    
    private readonly int _animIDSpeed = Animator.StringToHash("AI_Speed");
    private readonly int _animIDHorizontal = Animator.StringToHash("AI_Horizontal");
    private readonly int _animIDVertical = Animator.StringToHash("AI_Vertical");
    private readonly int _animIDAttack = Animator.StringToHash("AI_Attack");
    
    // Coroutine
    private Coroutine _updateCoroutine;

    
    private void OnEnable()
    {
        IsChaseRange = false;
        IsAttackRange = false;
        if (_updateCoroutine != null)
            StopCoroutine(_updateCoroutine);
        _updateCoroutine = StartCoroutine(UpdateCoroutine());
    }
    private void Start()
    {
        GetReference();
    }
  

    private void GetReference()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        
        behaviorTree.SetVariableValue("RootPosition", transform.position);
        if (_player)
            behaviorTree.SetVariableValue("Player", _player);
    }
    

    // Gọi bằng Events
    public void SetChaseSensor(bool isType) => IsChaseRange = isType;
    public void SetAttackSensor(bool isType) => IsAttackRange = isType;
    
    
    
    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            CheckAttack();
            transform.rotation = Quaternion.Euler(Vector3.zero);

            behaviorTree.SetVariableValue("IsChaseRange", IsChaseRange);
            behaviorTree.SetVariableValue("IsAttackRange", IsAttackRange);
            behaviorTree.SetVariableValue("CanAttack", CanAttack);
            yield return null;
        } 
    }
    private void CheckAttack()
    {
        if (_attackCooldown <= 0)
        {
            CanAttack = true;
            return;
        }
        _attackCooldown -= Time.deltaTime;
    }  




    // Gọi bằng InvokeMethod trên BehaviorTree
    public void Attack()
    {
        animator.SetTrigger(_animIDAttack);
        var data = (SharedFloat)behaviorTree.GetVariable("AttackCooldown");
        _attackCooldown = data.Value;
        CanAttack = false;
    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log("take damage");
    }


    
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PositionSpawn")) return;      
        behaviorTree.SetVariableValue("IsRoot", true); // nếu Enemy đi vào vị trí Root 

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PositionSpawn")) return;    
        behaviorTree.SetVariableValue("IsRoot", false); // nếu Enemy đi ra khỏi vị trí Root 
    }


}
