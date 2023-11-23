using UnityEngine;

[CreateAssetMenu(menuName = "Buff Effect/Experience Buff", fileName = "Experience_BU")]
public class ExperienceBuff : BuffEffect
{
    
    [Tooltip("Chi phí sử dụng Item nâng cấp")]
    public int UpgradeCost;
    
    public override void Apply(PlayerStateMachine _player)
    {
        if(_player.PlayerData.UserData.GalacticGems < UpgradeCost) return;

        _player.PlayerData.UserData.GalacticGems -= UpgradeCost;
        _player.PlayerConfig.CurrentEXP += (int)Value;
    }
}
