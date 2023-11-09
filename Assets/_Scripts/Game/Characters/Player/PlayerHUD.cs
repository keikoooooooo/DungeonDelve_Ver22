using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    private PlayerStateMachine _player;

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
        
        _player.E_CurrentHP += healthBar.ChangeValue;
        _player.E_CurrentST += staminaBar.ChangeValue;
        
        _player.E_SkillCooldown += skillCooldownTime.StartCooldown;
        _player.E_SpecialCooldown += specialCooldownTime.StartCooldown;
        
        staminaBar.Init(_player.PlayerConfig.MaxStamina);
    }
    private void OnDestroy()
    {
        if (_player == null) return;
        
        _player.E_CurrentHP -= healthBar.ChangeValue;
        _player.E_CurrentST -= staminaBar.ChangeValue;
        
        _player.E_SkillCooldown -= skillCooldownTime.StartCooldown;
        _player.E_SpecialCooldown -= specialCooldownTime.StartCooldown;
    }
    
    
}
