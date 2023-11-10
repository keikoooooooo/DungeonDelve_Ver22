using NaughtyAttributes;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField, Required] private PlayerStateMachine player;

    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private ProgressBar staminaBar;
    
    [SerializeField] private CooldownTime skillCooldownTime;
    [SerializeField] private CooldownTime specialCooldownTime;
    
    private void Start()
    {
        player.StatusHandle.E_HealthChanged += healthBar.ChangeValue;
        player.StatusHandle.E_StaminaChaged += staminaBar.ChangeValue;
        
        player.E_SkillCD += skillCooldownTime.StartCd;
        player.E_SpecialCD += specialCooldownTime.StartCd;
        
        healthBar.Init(player.PlayerConfig.MaxHealth);
        staminaBar.Init(player.PlayerConfig.MaxStamina);
    }
    private void OnDestroy()
    {
        if (player == null) return;
        
        player.StatusHandle.E_HealthChanged -= healthBar.ChangeValue;
        player.StatusHandle.E_StaminaChaged -= staminaBar.ChangeValue;
        
        player.E_SkillCD -= skillCooldownTime.StartCd;
        player.E_SpecialCD -= specialCooldownTime.StartCd;
    }

}
