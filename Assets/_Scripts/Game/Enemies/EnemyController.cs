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
    [Tooltip("Bộ phát animation"), SerializeField]
    private Animator animator;
    
    [Tooltip("Phạm vi phát hiện và đuổi theo Player"), SerializeField]
    private EnemySensor chaseSensor;

    [Tooltip("Phạm vi attack Player"), SerializeField]
    private EnemySensor attackSensor;


    // Variables
    private bool _isChaseRange;
    private bool _isAttackRange;
    private bool _canAttack;

    private float _attackCooldown;

    private NavMeshAgent _navMeshAgent;
    private GameObject _player;
    private BehaviorTree _behaviorTree;
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
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _behaviorTree = GetComponent<BehaviorTree>();
        _player = GameObject.FindGameObjectWithTag("Player");
        
        _behaviorTree.SetVariableValue("RootPosition", transform.position);
        if (_player)
            _behaviorTree.SetVariableValue("Player", _player);
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
            transform.rotation = Quaternion.Euler(Vector3.zero);
            CheckAttack();

            if (_behaviorTree)
            {
                _behaviorTree.SetVariableValue("IsChaseRange", _isChaseRange);
                _behaviorTree.SetVariableValue("IsAttackRange", _isAttackRange);
                _behaviorTree.SetVariableValue("CanAttack", _canAttack);
            }
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




    // Gọi bằng Invoke trên BehaviorTree
    public void Attack()
    {
        animator.SetTrigger(_animIDAttack);
        animator.SetFloat(_animIDHorizontal, 0);
        var data = (SharedFloat)_behaviorTree.GetVariable("AttackCooldown");
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
        _behaviorTree.SetVariableValue("IsRoot", true); // nếu Enemy đi vào vị trí Root 

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PositionSpawn")) return;    
        _behaviorTree.SetVariableValue("IsRoot", false); // nếu Enemy đi ra khỏi vị trí Root 
    }


}
