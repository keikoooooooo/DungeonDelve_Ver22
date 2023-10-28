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
    
    [Tooltip("Phạm vi phát hiện Player"), SerializeField]
    private EnemySensor chaseSensor;

    [Tooltip("Phạm vi attack Player"), SerializeField]
    private EnemySensor attackSensor;
    

    // Variables
    private bool _isChaseRange;
    private bool _isAttackRange;
    private bool _canAttack;
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
        _isChaseRange = false;
        _isAttackRange = false;
        if (_updateCoroutine != null)
            StopCoroutine(_updateCoroutine);
        _updateCoroutine = StartCoroutine(UpdateCoroutine());
    }
    private void Start()
    {
        GetReference();
        RegisterEvent();
    }
    private void OnDestroy()
    {
        UnRegisterEvent();
    }


    private void GetReference()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        
        behaviorTree.SetVariableValue("RootPosition", transform.position);
        if (_player)
            behaviorTree.SetVariableValue("Player", _player);
    }

    private void RegisterEvent()
    {
        chaseSensor.E_PlayerEnter += OnChaseSensorEnter;
        chaseSensor.E_PlayerExit += OnChaseSensorExit;

        attackSensor.E_PlayerEnter += OnAttackSensorEnter;
        attackSensor.E_PlayerExit += OnAttackSensorExit;
    }
    private void UnRegisterEvent()
    {
        chaseSensor.E_PlayerEnter -= OnChaseSensorEnter;
        chaseSensor.E_PlayerExit -= OnChaseSensorExit;

        attackSensor.E_PlayerEnter -= OnAttackSensorEnter;
        attackSensor.E_PlayerExit -= OnAttackSensorExit;
    }

    private void OnChaseSensorEnter() => _isChaseRange = true;
    private void OnChaseSensorExit() => _isChaseRange = false;
    private void OnAttackSensorEnter() => _isAttackRange = true;
    private void OnAttackSensorExit() => _isAttackRange = false;

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            CheckAttack();
            transform.rotation = Quaternion.Euler(Vector3.zero);

            behaviorTree.SetVariableValue("IsChaseRange", _isChaseRange);
            behaviorTree.SetVariableValue("IsAttackRange", _isAttackRange);
            behaviorTree.SetVariableValue("CanAttack", _canAttack);
            yield return null;
        } 
    }
    private void CheckAttack()
    {
        if (_attackCooldown <= 0)
        {
            _canAttack = true;
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
        _canAttack = false;
    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log("take damage");
    }


    
    // Event Methods
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
