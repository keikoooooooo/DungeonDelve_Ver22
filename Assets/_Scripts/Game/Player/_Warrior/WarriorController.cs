using System.Collections;
using UnityEngine;

public class WarriorController : PlayerController
{
    [Header("SubClass -------")] 
    [Tooltip("Script kiểm tra va chạm"), SerializeField]
    public PhysicsCollision physicsCollision;
    
    [Tooltip("Thời gian chuyển đổi dạng anim: từ cầm vũ khí sang không cầm vũ khí"), SerializeField]
    private float conversionTime;

    [Tooltip("SkinMesh weapon: Sau lưng"), SerializeField]
    private GameObject swordOnShoulder;

    [Tooltip("SkinMesh weapon: Đang cầm"), SerializeField]
    private GameObject sword;
    

    [HideInInspector] private float moveSpeedTemp;
    private float conversionTimeTemp;
    
    private Coroutine _weaponUnEquippedCoroutine;

    
    
    protected override void Update()
    {
        base.Update();
        
        HandleAttack();
    }


    protected override void SetVariables()
    {
        base.SetVariables();

        conversionTimeTemp = conversionTime;
        
        sword.SetActive(false);
        swordOnShoulder.SetActive(true);
        animator.SetBool(IDWeaponEquip, false);
        
        if(_weaponUnEquippedCoroutine !=null)
            StopCoroutine(_weaponUnEquippedCoroutine);
        _weaponUnEquippedCoroutine = StartCoroutine(WeaponUnEquippedCoroutine());
    }
    

    private void BuffSkill()
    {
        moveSpeedTemp = Stats.runSpeed;
        Stats.runSpeed += Stats.runSpeed * .4f;
        
    }
    private void UnBuffSkill()
    {
        Stats.runSpeed = moveSpeedTemp;
    }

    
    #region Kiểm tra và thay đổi animation sang cầm và không cầm vũ khí
    private IEnumerator WeaponUnEquippedCoroutine()
    {
        while (true)
        {
            conversionTimeTemp = conversionTimeTemp > 0 ? conversionTimeTemp - Time.deltaTime : 0;

            if (conversionTimeTemp <= 0 && IsIdle && !swordOnShoulder.activeInHierarchy)
            {
                animator.SetBool(IDWeaponEquip, false);
                CanMove = false;
                CanRotation = false;
            }
            
            else if (!IsIdle || !IsGrounded)
            {
                conversionTimeTemp = conversionTime;
            }
            
            yield return null;
        }
    }
    private void WeaponEquipped() // Cầm vũ khí
    {
        conversionTimeTemp = conversionTime;
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

    
    private void CheckCollision() // gọi trên event Animation
    {
        physicsCollision.CheckCollision();
    }
    
    // OverridingMethods
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
