using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PushDirectionEnum
{
    Forward,
    Behind
}

[Serializable]
public class AttackCustom
{
    
    [Tooltip("Các lực đẩy mỗi khi attack, lực áp dụng tương ứng thứ n của animationClip Attack, vd: Attack2 -> lấy giá trị thứ 2 áp dụng vào")]
    public List<float> pushForce;
    
    [Tooltip("Đẩy đi trong bao lâu"), Range(0f, 1f)]
    public float pushTime;
    
    [Tooltip("Hướng đẩy: Forward -> đẩy tới trước, Behind -> đẩy ra sau")]
    public PushDirectionEnum pushDirectionEnum;
}


public abstract class PlayerController : PlayerStateMachine
{
    
    [Space, Tooltip("Thêm lực đẩy vào character khi tấn công"), SerializeField] 
    private AttackCustom attackCustom;
    
    protected bool IsAttackPressed => inputs.leftMouse;
    protected bool IsSkillPressed => inputs.e && _skillCD_Temp <= 0;
    protected bool IsSpecialPressed => inputs.q && _specialCD_Temp <= 0;
    public float MouseHoldTime { get; private set; }       // thời gian giữ chuột -> nếu hơn .3s -> attackHolding,
    public bool DetectionEnemy => _enemies.Count > 0;

    private readonly List<GameObject> _enemies = new();
    
    
    // Player
    [HideInInspector] private Vector3 _pushVelocity;       // vận tốc đẩy 
    [HideInInspector] private int _directionPushVelocity;  // hướng đẩy 
    [HideInInspector] protected int _attackCounter;        // số lần attackCombo
    [HideInInspector] protected bool _isAttackPressed;     // có nhấn attack k ?
    
    private Coroutine _pushVelocityCoroutine;
    private Coroutine _pushMoveCoroutine;
    private Coroutine _rotateToTargetCoroutine;

    protected override void SetVariables()
    {
        base.SetVariables();
        
        CanAttack = true;
        
        _directionPushVelocity = attackCustom.pushDirectionEnum switch // tìm hướng đẩy mỗi khi tấn công?
        {
            PushDirectionEnum.Forward => 1, 
            PushDirectionEnum.Behind => -1,   
            _ => 0
        };
    }
    protected void HandleAttack()
    {
        animator.SetFloat(IDStateTime, Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
        animator.ResetTrigger(IDNormalAttack);
        
        if (animator.IsTag("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            AttackEnd();
        }
        
        if(!CanAttack || !IsGrounded) return;
        
        Attack();
        ElementalSkill();
        ElementalBurst();
    }

    private void Attack()
    {
        if (IsAttackPressed)
        {
            _isAttackPressed = true;
            MouseHoldTime += Time.deltaTime;
        }
        
        switch (_isAttackPressed)
        {
            case true when !IsAttackPressed && MouseHoldTime < .5f:
                NormalAttack();
                break;
            
            case true when IsAttackPressed && MouseHoldTime >= .5f:
                CanAttack = false;
                _isAttackPressed = false;
                ChargedAttack();
                break;
        }
    }
    protected virtual void NormalAttack()
    {
        CanMove = false;
        CanRotation = false;
        _isAttackPressed = false;
        
        MouseHoldTime = 0;
        animator.SetTrigger(IDNormalAttack);

        CalculateDMG_NA();
        RotateToEnemy();
    }
    protected virtual void ChargedAttack()
    {
        animator.SetTrigger(IDChargedAttack);
        CalculateDMG_CA();
    }
    protected virtual void ElementalSkill()
    {
        if(!IsSkillPressed) return;

        animator.SetTrigger(IDSkill);
        CanAttack = false;
        CanMove = false;
        CanRotation = false;
        _skillCD_Temp = PlayerConfig.SkillCD;
        
        CalculateDMG_EK();
        OnSkillCooldownEvent();
    }
    protected virtual void ElementalBurst()
    {
        if(!IsSpecialPressed) return;
        animator.SetTrigger(IDSpecial);
        CanAttack = false;
        CanMove = false;
        CanRotation = false;
        _specialCD_Temp = PlayerConfig.SpecialCD;

        CalculateDMG_EB();
        OnSpecialCooldownEvent();
    }

    

    public void SetAttackCounter(int count) => _attackCounter = count; // gọi trên event animaiton
    public void AddForceAttack()
    {
        if(_pushMoveCoroutine != null) StopCoroutine(PushMoveCoroutine());
        _pushMoveCoroutine = StartCoroutine(PushMoveCoroutine());
    }
    private IEnumerator PushMoveCoroutine()
    {
        var timePush = attackCustom.pushTime;
        while (timePush > 0)
        {
            _pushVelocity = model.forward * (attackCustom.pushForce[_attackCounter] * _directionPushVelocity);
            characterController.Move(_pushVelocity * Time.deltaTime + new Vector3(0f, -9.81f, 0f) * Time.deltaTime);
            timePush -= Time.deltaTime;
            yield return null;
        }
    }
    
    #region Xử lí xoay nhân vật về phía Enemy mỗi khi Attack
    public Transform FindClosestEnemy()
    {
        _enemies.Sort((a, b)
            => Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
        return _enemies[0].transform;
    }
    private void RotateToEnemy()
    {
        if(!DetectionEnemy) 
            return;
        
        if (_rotateToTargetCoroutine != null) StopCoroutine(RotateToTargetCoroutine());
        _rotateToTargetCoroutine = StartCoroutine(RotateToTargetCoroutine());
    }
    private IEnumerator RotateToTargetCoroutine()
    {
        var target = FindClosestEnemy();
        var direction = Quaternion.LookRotation(target.position - transform.position);

        var directionLocal = Mathf.Floor(transform.eulerAngles.y);
        var directionTaget = Mathf.Floor(direction.eulerAngles.y);

        var rotation = Quaternion.Euler(0, direction.eulerAngles.y, 0);
        while (Mathf.Abs(directionTaget - directionLocal) > .2f)
        {
            model.rotation = Quaternion.RotateTowards(model.rotation, rotation, 20f);
            directionLocal = Mathf.Floor(model.eulerAngles.y);
            yield return null;
        }
    }
    public void AddEnemy(GameObject _gameObject)
    {
        if(_enemies.Contains(_gameObject)) 
            return;
        _enemies.Add(_gameObject);
    }
    public void RemoveEnemy(GameObject _gameObject)
    {
        if(!_enemies.Contains(_gameObject)) 
            return;
        _enemies.Remove(_gameObject);
    }
    #endregion

    
    #region Damage Calculation
    public override void CalculateDMG_NA()
    {
        // tìm %DMG dựa theo cấp của vũ khí trên đòn đánh thứ n
        var _percent = PlayerConfig.NormalAttackMultiplier[_attackCounter].Multiplier[PlayerConfig.WeaponLevel - 1]; // List bắt đầu từ 0, Vũ khí từ 1 -> nên trừ 1
        _calculatedDamage = Calculation(_percent); 
    }
    public override void CalculateDMG_CA()
    {
        var _percent = PlayerConfig.ChargedAttackMultiplier[0].Multiplier[PlayerConfig.WeaponLevel - 1];
        _calculatedDamage = Calculation(_percent);
    }
    public override void CalculateDMG_EK()
     {
         var _percent = PlayerConfig.SkillMultiplier[0].Multiplier[PlayerConfig.WeaponLevel - 1]; 
         _calculatedDamage = Calculation(_percent); 
     }   
    public override void CalculateDMG_EB()
    {
        var _percent = PlayerConfig.SpecialMultiplier[0].Multiplier[PlayerConfig.WeaponLevel - 1];
        _calculatedDamage = Calculation(_percent); 
    }
    
    
    /// <summary>
    /// Tính sát thương đầu ra theo phần trăm * ATK của player
    /// </summary>
    /// <param name="_percent"> Phần trăm sát thương. </param>
    /// <returns></returns>
    protected int Calculation(float _percent) => Mathf.CeilToInt(PlayerConfig.ATK * (_percent / 100.0f)); 
    #endregion

    
    protected virtual void AttackEnd()
    {
        MouseHoldTime = 0;
        CanAttack = true;
        CanMove = true;
        CanRotation = true;
    }
    protected virtual void SkillEnd()
    {
        inputs.e = false;
        
        AttackEnd();
    }
    protected virtual void SpecialEnd()
    {
        inputs.q = false;
        
        AttackEnd();
    }
    
    
    public override void ReleaseAction()
    {
        inputs.leftMouse = false;
        inputs.e = false;
        inputs.q = false;
        AttackEnd();
    }
    
}
