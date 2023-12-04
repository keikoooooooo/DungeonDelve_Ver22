using UnityEngine;

[CreateAssetMenu(menuName = "Buff Effect/Experience Buff", fileName = "Experience_BU")]
public class SO_ExperienceBuff : SO_BuffEffect
{
    
    [Tooltip("Chi phí sử dụng Item nâng cấp")]
    public int UpgradeCost;
    
    public override void Apply(PlayerController _player)
    {
        _player.PlayerConfig.SetCurrentEXP(_player.PlayerConfig.GetCurrentEXP() + (int)Value);
    }
}
