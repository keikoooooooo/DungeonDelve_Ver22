using System;
using NaughtyAttributes;
using NodeCanvas.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour, IDamageable, IPooled<EnemyController>
{
    // Ref
    [field: SerializeField, Required] public Blackboard Blackboard { get; private set; }
    [SerializeField] private SO_EnemyConfiguration enemyConfig;
    public SO_EnemyConfiguration EnemyConfig { get; private set; }
    
    [Space]
    [SerializeField, Tooltip("Layer chính: chịu ảnh hưởng của va chạm trong game")]
    private LayerMask mainLayer;
    [SerializeField, Tooltip("Layer phụ: không chịu ảnh hưởng bởi va chạm, khi Enemy die sẽ chuyển về layer này")]
    private LayerMask ignoreLayer;


    // Get & Set Property 
    public StatusHandle Health { get; private set; }
    public Vector3 PlayerPosition => _player.transform.position;
    private readonly List<int> _enemyLevel = new() { 11, 21, 31, 41, 51, 61, 71, 81, 91, 101};
    
    // Events
    [Space]
    public UnityEvent<EnemyController> OnTakeDamageEvent;
    public UnityEvent<EnemyController> OnDieEvent;
    
    // Variables
    private PlayerController _player;
    private int _attackCount;
    
    
    private void Awake()
    {
        Health = new StatusHandle();
        EnemyConfig = Instantiate(enemyConfig);
    }
    private void OnEnable()
    {
        DamageableData.Add(gameObject, this);
        gameObject.SetObjectLayer(mainLayer.value);
        
        if(!_player && GameManager.Instance)
        {
            _player = GameManager.Instance.Player;
            _player.OnDieEvent += HandlePlayerDie;
            SetRefPlayer(_player.gameObject);
        }
        
        UpdateConfig();
        SetDie(false);
        SetTakeDMG(false);
        SetChaseSensor(false);
        SetAttackSensor(false);
        SetRootPosition(transform.position);
    }
    private void Start()
    {
        var _maxHP = EnemyConfig.GetHP();
        Health.InitValue(_maxHP, _maxHP);
        SetRunSpeed(EnemyConfig.GetRunSpeed());
        SetWalkSpeed(EnemyConfig.GetWalkSpeed());
        SetCDNormalAttack(EnemyConfig.GetNormalAttackCD());
        SetCDSkillAttack(EnemyConfig.GetSkillAttackCD());
        SetCDSpecialAttack(EnemyConfig.GetSpecialAttackCD());
    }
    private void OnDisable()
    {
        DamageableData.Remove(gameObject);
        _player.OnDieEvent -= HandlePlayerDie;
    }

    
    public void UpdateConfig()
    {
        var _multiplier = 0f;   // Tỷ lệ tăng của enemy so với người chơi
        var _currentValue = 0;  // Giá trị hiện tại
        var _newValue = 0;      // Giá trị mới
        
        // Set HP
        _currentValue = _player.PlayerConfig.GetHP();
        _multiplier = EnemyConfig.GetHPRatio();
        _newValue = Mathf.RoundToInt(_currentValue * _multiplier);
        EnemyConfig.SetHP(_newValue);
        
        // Set DEF
        _currentValue = _player.PlayerConfig.GetDEF();
        _multiplier = EnemyConfig.GetDEFRatio();
        _newValue = Mathf.RoundToInt(_currentValue * _multiplier);
        EnemyConfig.SetDEF(_newValue);
        
        // Set Level
        _currentValue = _player.PlayerConfig.GetLevel();
        _multiplier = EnemyConfig.GetLevelRatio();
        _newValue = Mathf.RoundToInt(_currentValue * _multiplier);
        EnemyConfig.SetLevel(_newValue);

        var _maxHP = EnemyConfig.GetHP();
        Health.InitValue(_maxHP, _maxHP);
    }
    private void HandlePlayerDie()
    {
        SetChaseSensor(false);
        SetAttackSensor(false);
    }
    

    #region Set BehaviorTrees Variables
    private void SetRefPlayer(GameObject _value) => Blackboard.SetVariableValue("Player", _value);
    private void SetRootPosition(Vector3 _value) => Blackboard.SetVariableValue("RootPosition", _value);
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
    #endregion
    
    #region HandleDMG
    public void CauseDMG(GameObject _gameObject, AttackType _attackType)
    {
        if (!DamageableData.Contains(_gameObject, out var iDamageable)) return;

        var _damage = _attackType switch
        {
            AttackType.NormalAttack => PercentDMG_NA(),
            AttackType.ChargedAttack => PercentDMG_CA(),
            AttackType.ElementalSkill => PercentDMG_ES(),
            AttackType.ElementalBurst => PercentDMG_EB(),
            _ => 1
        };
       
        var _isCrit = false;  // Có kích CRIT không ?
        if (Random.value <= EnemyConfig.GetCRITRate() / 100)
        {
            var critDMG = (EnemyConfig.GetCRITDMG() + 100.0f) / 100.0f; // vì là DMG cộng thêm nên cần phải +100%DMG vào
            _damage *= critDMG;
            _isCrit = true;
        } 
        iDamageable.TakeDMG( Mathf.CeilToInt(_damage), _isCrit);
    }
    public void TakeDMG(int _damage, bool _isCRIT)
    {  
        // Nếu đòn đánh là CRIT thì sẽ nhận Random DEF từ giá trị 0 -> DEF ban đầu / 2, nếu không sẽ lấy 100% DEF ban đầu
        var _valueDef = _isCRIT ? Random.Range(0, EnemyConfig.GetDEF() * 0.5f) : EnemyConfig.GetDEF();
        
        // Tính lượng DMG thực nhận vào sau khi trừ đi lượng DEF
        var _finalDmg = (int)Mathf.Max(0, _damage - Mathf.Max(0, _valueDef));
        
        Health.Decreases(_finalDmg);
        DMGPopUpGenerator.Instance.Create(transform.position, _finalDmg, _isCRIT, true);
        
        if (Health.CurrentValue <= 0)
        {
            Die();
        }
        else
        {
            SetTakeDMG(true);
            OnTakeDamageEvent?.Invoke(this);
        }
    }
    private void Die()
    {
        gameObject.SetObjectLayer(ignoreLayer.value);
        EnemyTracker.Remove(transform);
        OnDieEvent?.Invoke(this);
        SetDie(true);
        SetChaseSensor(false);
        SetAttackSensor(false);
        SetTakeDMG(false);
    }

    public void SetAttackCount(int _value) => _attackCount = _value;
    public float PercentDMG_NA() => enemyConfig.GetNormalAttackMultiplier()[_attackCount].GetMultiplier()[FindMultiplierLevelIndex()];
    public float PercentDMG_CA() => enemyConfig.GetChargedAttackMultiplier()[0].GetMultiplier()[FindMultiplierLevelIndex()];
    public float PercentDMG_ES() => enemyConfig.GetElementalSkillMultiplier()[0].GetMultiplier()[FindMultiplierLevelIndex()];
    public float PercentDMG_EB() => enemyConfig.GetElementalBurstMultiplier()[0].GetMultiplier()[FindMultiplierLevelIndex()];
    private int FindMultiplierLevelIndex()  //Tìm Index của %ATK cộng thêm dựa trên level hiện tại của enemy 
    {
        var _level = _enemyLevel.Count - 1;
        for (var i = 0; i < _enemyLevel.Count; i++)
        {
            if (EnemyConfig.GetLevel() >= _enemyLevel[i]) continue;
            _level = i;
            break;
        }
        return _level;
    }
    public int CalculationDMG(float _percent)
    {
        var _playerATK = _player.PlayerConfig.GetATK();
        var _currentATK = EnemyConfig.GetATK();
        
        // Set ATK
        var modifiedEnemyATK = Mathf.CeilToInt(_currentATK * (_percent / 100.0f));
        return modifiedEnemyATK * (_playerATK / _currentATK);
    }
    #endregion

    
    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<EnemyController> ReleaseCallback { get; set; }
}
