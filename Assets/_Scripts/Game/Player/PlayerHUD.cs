using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHUD : MonoBehaviour
{
    private PlayerStateMachine _player;
    
    [SerializeField] private HealthAndStamina healthAndStamina;
    [FormerlySerializedAs("skillCooldown")] [SerializeField] private CooldownTime skillCooldownTime;
    [FormerlySerializedAs("specialCooldown")] [SerializeField] private CooldownTime specialCooldownTime;
    
    
    
    private void Awake()
    {
        _player = GetComponentInParent<PlayerStateMachine>();
    }


    private void Start()
    {
        if (_player == null) return;
        
        _player.E_CurrentHP += healthAndStamina.CurrentHP;
        _player.E_CurrentST += healthAndStamina.CurrentST;
        _player.E_SkillCooldown += skillCooldownTime.StartCooldown;
        _player.E_SpecialCooldown += specialCooldownTime.StartCooldown;
    }
    private void OnDestroy()
    {
        if (_player == null) return;
        
        _player.E_CurrentHP -= healthAndStamina.CurrentHP;
        _player.E_CurrentST -= healthAndStamina.CurrentST;
        _player.E_SkillCooldown -= skillCooldownTime.StartCooldown;
        _player.E_SpecialCooldown -= specialCooldownTime.StartCooldown;
    }
    
    
}
