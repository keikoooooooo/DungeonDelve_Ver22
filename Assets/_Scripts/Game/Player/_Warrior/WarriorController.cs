using System.Collections;
using UnityEngine;

public class WarriorController : PlayerController
{
    [Header("SubClass -------")] 
    [Tooltip("Thời gian chuyển đổi dạng anim: từ cầm vũ khí sang không cầm vũ khí"), SerializeField]
    private float conversionTime;

    [Tooltip("SkinMesh weapon: Sau lưng"), SerializeField]
    private GameObject swordOnShoulder;

    [Tooltip("SkinMesh weapon: Đang cầm"), SerializeField]
    private GameObject sword;
    

    [HideInInspector] private float moveSpeedTemp;
    private float _conversionTimeTemp;
    private bool _characterControllerNull;
    private Coroutine _weaponUnEquippedCoroutine;

    
    
    protected override void Update()
    {
        base.Update();
        
        HandleAttack();
    }
    
    private void BuffSkill()
    {
        moveSpeedTemp = PlayerConfig.runSpeed;
        PlayerConfig.runSpeed += PlayerConfig.runSpeed * .4f;
        
    }
    private void UnBuffSkill()
    {
        PlayerConfig.runSpeed = moveSpeedTemp;
    }

    
    #region Kiểm tra và thay đổi animation sang cầm và không cầm vũ khí
    private IEnumerator WeaponUnEquippedCoroutine()
    {
        while (true)
        {
            _conversionTimeTemp = _conversionTimeTemp > 0 ? _conversionTimeTemp - Time.deltaTime : 0;

            if (_conversionTimeTemp <= 0 && IsIdle && !swordOnShoulder.activeInHierarchy)
            {
                animator.SetBool(IDWeaponEquip, false);
                CanMove = false;
                CanRotation = false;
            }
            
            else if (!_characterControllerNull || !IsIdle || !IsGrounded)
            {
                _conversionTimeTemp = conversionTime;
            }
            
            yield return null;
        }
    }
    private void WeaponEquipped() // Cầm vũ khí
    {
        _conversionTimeTemp = conversionTime;
        animator.SetBool(IDWeaponEquip, true);
        sword.SetActive(true);
        swordOnShoulder.SetActive(false);
    }
    private void WeaponUnEquipped() // Không cầm vũ khí
    {
        CanMove = true;
        CanRotation = true;
        swordOnShoulder.SetActive(true);
        sword.SetActive(false);
    }
    #endregion
    
    
    // OverridingMethods
    protected override void SetVariables()
    {
        base.SetVariables();
 
        _conversionTimeTemp = conversionTime;
         
        sword.SetActive(false);
        swordOnShoulder.SetActive(true);
        animator.SetBool(IDWeaponEquip, false);
         
        if(_weaponUnEquippedCoroutine !=null)
            StopCoroutine(_weaponUnEquippedCoroutine);
        _weaponUnEquippedCoroutine = StartCoroutine(WeaponUnEquippedCoroutine());
    }

    protected override void SetReference()
    {
        base.SetReference();

        _characterControllerNull = CharacterController != null;
    }


    protected override void AttackCombo()
    {
        WeaponEquipped();
        
        base.AttackCombo();
    }
    protected override void AttackHolding()
    {
        WeaponEquipped();
        
        animator.SetTrigger(IDAttackHolding);
        CanMove = false;
        CanRotation = false;
    }
    protected override void Skill()
    {
        if(!IsSkillPressed) return;
        
        WeaponEquipped();
        base.Skill();
    }
    protected override void Special()
    {
        if(!IsSpecialPressed) return;
        
        WeaponEquipped();
        base.Special();
    }
    
}
