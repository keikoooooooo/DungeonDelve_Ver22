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
        if(_player) SetRefPlayer(_player);
        SetRunSpeed(EnemyConfig.RunSpeed);
        SetWalkSpeed(EnemyConfig.WalkSpeed);
        SetCDNormalAttack(EnemyConfig.NormalAttackCD);
        SetCDSkillAttack(EnemyConfig.SkillAttackCD);
        SetCDSpecialAttack(EnemyConfig.SpecialAttackCD);
    }
    private void OnDisable()
    {
        StatusHandle.E_Die -= Die;
        DamageableData.Remove(gameObject);
    }
    
    
    // Set BehaviorTrees Variables    
    private void SetRefPlayer(GameObject _value) => Blackboard.SetVariableValue("Player", _value);
    private void SetWalkSpeed(float _value) => Blackboard.SetVariableValue("WalkSpeed", _value);
    private void SetRunSpeed(float _value) => Blackboard.SetVariableValue("RunSpeed", _value);
    private void SetCDNormalAttack(float _value) => Blackboard.SetVariableValue("NormalAttackCD", _value);
    private void SetCDSkillAttack(float _value) => Blackboard.SetVariableValue("SkillAttackCD", _value);
    private void SetCDSpecialAttack(float _value) => Blackboard.SetVariableValue("SpecialAttackCD", _value);
    public void SetRootSensor(bool _value) => Blackboard.SetVariableValue("RootSensor", _value);
    public void SetChaseSensor(bool _value) => Blackboard.SetVariableValue("ChaseSensor", _value);
    public void SetAttackSensor(bool _value) => Blackboard.SetVariableValue("AttackSensor", _value);
    private void SetTakeDMG(bool _value) => Blackboard.SetVariableValue("TakeDMG", _value);
    private void SetDie(bool _value) => Blackboard.SetVariableValue("Die", _value);
    
    
    
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
