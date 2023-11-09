using NaughtyAttributes;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField, Required] PlayerStateMachine _player;

    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private ProgressBar staminaBar;
    
    [SerializeField] private CooldownTime skillCooldownTime;
    [SerializeField] private CooldownTime specialCooldownTime;
    
    
    private void Awake()
    {
        _player = GetComponentInParent<PlayerStateMachine>();
    }


    private void Start()
    {
        if (_player == null) return;

        _player.StatusHandle.E_HealthChanged += healthBar.ChangeValue;
        _player.StatusHandle.E_StaminaChaged += staminaBar.ChangeValue;
        
        _player.E_SkillCD += skillCooldownTime.StartCd;
        _player.E_SpecialCD += specialCooldownTime.StartCd;
        
        healthBar.Init(_player.PlayerConfig.MaxHealth);
        staminaBar.Init(_player.PlayerConfig.MaxStamina);
    }
    private void OnDestroy()
    {
        if (_player == null) return;
        
        _player.StatusHandle.E_HealthChanged -= healthBar.ChangeValue;
        _player.StatusHandle.E_StaminaChaged -= staminaBar.ChangeValue;
        
        _player.E_SkillCD -= skillCooldownTime.StartCd;
        _player.E_SpecialCD -= specialCooldownTime.StartCd;
    }

}
