using NaughtyAttributes;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField, Required] private PlayerController player;

    [SerializeField] private NameAndLevelText nameAndLevelText;
    
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private ProgressBar staminaBar;
    
    [SerializeField] private CooldownTime skillCooldownTime;
    [SerializeField] private CooldownTime specialCooldownTime;
    
    private void Start()
    {
        player.Health.E_OnValueChanged += healthBar.ChangedValue;
        player.Stamina.E_OnValueChanged += staminaBar.ChangedValue;
        
        player.E_SkillCD += skillCooldownTime.StartCd;
        player.E_SpecialCD += specialCooldownTime.StartCd;
        
        nameAndLevelText.ChangeNameText(player.PlayerConfig.Name);
        nameAndLevelText.ChangeLevelText(player.PlayerConfig.Level);
        healthBar.Init(player.PlayerConfig.MaxHealth);
        staminaBar.Init(player.PlayerConfig.MaxStamina);
    }
    private void OnDestroy()
    {
        if (player == null) return;
        
        player.Health.E_OnValueChanged -= healthBar.ChangedValue;
        player.Stamina.E_OnValueChanged -= staminaBar.ChangedValue;
        
        player.E_SkillCD -= skillCooldownTime.StartCd;
        player.E_SpecialCD -= specialCooldownTime.StartCd;
    }

}
