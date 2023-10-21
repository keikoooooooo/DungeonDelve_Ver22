using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public enum PushDirectionEnum
{
    Forward,
    Behind
}

[Serializable]
public class AttackCustom
{
    
    [Tooltip("Các lực đẩy mỗi khi attack, số lực tương ứng với số animation attackCombo")]
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
    protected bool IsSkillPressed => inputs.e && _skillCooldown <= 0;
    protected bool IsSpecialPressed => inputs.q && _specialCooldown <= 0;
    
    
    // Player
    protected bool CanAttack = true;                       // được phép attack ? 
    [HideInInspector] private Vector3 _pushVelocity;       // hướng đẩy
    [HideInInspector] protected int _attackCounter;        // số lần attackCombo
    [HideInInspector] protected bool _isAttackPressed;     // có nhấn attack k ?
    [HideInInspector] protected float _lastClickedAttack;  // thời gian nhấn -> nếu giữ hơn .3s -> attackHolding,

    private int _directionPushVelocity; // hướng đẩy 
    
    // Coroutine
    private Coroutine _pushVelocityCoroutine;
    private Tween pushVelocityTween;


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
        animator.ResetTrigger(IDAttackCombo);
        
        if (animator.IsTag("Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            AttackEnd();
        }
        
        if(!CanAttack || !IsGrounded) return;
        
        Attack();
        Skill();
        Special();
    }

    private void Attack()
    {
        if (IsAttackPressed)
        {
            _isAttackPressed = true;
            _lastClickedAttack += Time.deltaTime;
        }
        
        switch (_isAttackPressed)
        {
            case true when !IsAttackPressed && _lastClickedAttack <= .25f:
                AttackCombo();
                break;
            
            case true when IsAttackPressed && _lastClickedAttack >= .3f:
                _isAttackPressed = false;
                _lastClickedAttack = 0;
                CanAttack = false;
                AttackHolding();
                break;
        }
    }
    protected virtual void AttackCombo()
    {
        _lastClickedAttack = 0;
        
        CanMove = false;
        CanRotation = false;
        _isAttackPressed = false;
        
        animator.SetTrigger(IDAttackCombo);
        //playerSensor.RotateToTarget();
    }
    protected abstract void AttackHolding();// Định nghĩa lại đòn tấn công của nhân vật.
    protected virtual void Skill()
    {
        if(!IsSkillPressed) return;

        animator.SetTrigger(IDSkill);
        CanAttack = false;
        CanMove = false;
        CanRotation = false;
        
        _skillCooldown = Stats.skillCooldown;
        OnSkillCooldownEvent();
    }
    protected virtual void Special()
    {
        if(!IsSpecialPressed) return;
        
        animator.SetTrigger(IDSpecial);
        CanAttack = false;
        CanMove = false;
        CanRotation = false;
        
        _specialCooldown = Stats.specialCooldown;
        OnSpecialCooldownEvent();
    }

    public void AddForceAttack()
    {
        _pushVelocity = model.forward * (attackCustom.pushForce[_attackCounter] * _directionPushVelocity);
        
        pushVelocityTween?.Kill();
        pushVelocityTween = transform.DOMove(transform.position + _pushVelocity, attackCustom.pushTime);
    }


    
    protected virtual void AttackEnd()
    {
        CanAttack = true;
        CanMove = true;
        CanRotation = true;
    }
    private void SkillEnd()
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
        inputs.e = false;
        inputs.q = false;
        AttackEnd();
    }
    
    
    
}
