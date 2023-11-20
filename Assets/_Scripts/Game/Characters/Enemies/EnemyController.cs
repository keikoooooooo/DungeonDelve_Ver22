using NaughtyAttributes;
using NodeCanvas.Framework;
using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour, IDamageable
{
    // Ref
    [field: SerializeField, Required] public EnemyConfiguration EnemyConfig { get; private set; }
    [field: SerializeField, Required] public Blackboard Blackboard { get; private set; }
    
    // Get & Set Property 
    public StatusHandle StatusHandle { get; private set; }
    public Vector3 PlayerPosition => _player.transform.position;
    public int CalculatedDamage { get; set; }
    
    private readonly List<int> _enemyLevel = new() { 11, 21, 31, 41, 51, 61, 71, 81, 91, 101};
    
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
    public void SetTakeDMG(bool _value) => Blackboard.SetVariableValue("TakeDMG", _value);
    public void SetDie(bool _value) => Blackboard.SetVariableValue("Die", _value);
    
    
    
    #region HandleDMG
    public void CauseDMG(GameObject _gameObject)
    {
        if (!DamageableData.Contains(_gameObject, out var iDamageable)) return;

        // Có kích CRIT không ?
        var critRateRandom = Random.value;
        var _isCrit = false;
        if (critRateRandom <= EnemyConfig.CRITRate / 100)
        {
            var critDMG = (EnemyConfig.CRITDMG + 100.0f) / 100.0f; // vì là DMG cộng thêm nên cần phải +100%DMG vào
            
            CalculatedDamage = Mathf.CeilToInt(CalculatedDamage * critDMG);
            _isCrit = true;
        } 
        
        iDamageable.TakeDMG(CalculatedDamage, _isCrit);
    }
    public void TakeDMG(int _damage, bool _isCRIT)
    {   
        SetTakeDMG(true);
        
        // Nếu đòn đánh là CRIT thì sẽ nhận Random DEF từ giá trị 0 -> DEF ban đầu / 2, nếu không sẽ lấy 100% DEF ban đầu
        var _valueDef = _isCRIT ? Random.Range(0, EnemyConfig.DEF * 0.5f) : EnemyConfig.DEF;
        
        // Tính lượng DMG thực nhận vào sau khi trừ đi lượng DEF
        var _def = Mathf.CeilToInt(_damage * (_valueDef / 100.0f));
        _damage -= _def;
        
        StatusHandle.Subtract(_damage, StatusHandle.StatusType.Health);
        DMGPopUpGenerator.Instance.Create(transform.position, _damage, _isCRIT, true);
    }
    public void Die()
    {
        SetDie(true);
    }
    
        
    // Damage Calculation - Tính và gán sát thường đầu ra 
    public void CalculateDMG_NA(AnimationEvent eEvent)
    {
        var _level = FindLevelIndex();
        var _percent = EnemyConfig.NormalAttackMultiplier[eEvent.intParameter].Multiplier[_level];
        CalculatedDamage = Calculation(_percent);
    }
    public void CalculateDMG_EK(AnimationEvent eEvent)
    {
        var _level = FindLevelIndex();
        var _percent = EnemyConfig.SkillMultiplier[eEvent.intParameter].Multiplier[_level];
        CalculatedDamage = Calculation(_percent);
    }
    public void CalculateDMG_EB(AnimationEvent eEvent)
    {
        var _level = FindLevelIndex();
        var _percent = EnemyConfig.SpecialMultiplier[eEvent.intParameter].Multiplier[_level];
        CalculatedDamage = Calculation(_percent);
    }
    private int FindLevelIndex() // Tìm %ATK cộng thêm của dựa trên level hiện tại của enemy 
    {
        var _level = 0;
        for (var i = 0; i < _enemyLevel.Count; i++)
        {
            if (EnemyConfig.Level >= _enemyLevel[i]) continue;
            _level = i;
            break;
        }
        return _level;
    }
    
    /// <summary>
    /// Tính sát thương đầu ra theo phần trăm * ATK của player
    /// </summary>
    /// <param name="_percent"> Phần trăm sát thương. </param>
    /// <returns></returns>
    private int Calculation(float _percent) => Mathf.CeilToInt(EnemyConfig.ATK * (_percent / 100.0f));
    
    #endregion

 

}
