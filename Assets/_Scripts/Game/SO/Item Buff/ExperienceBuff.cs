using UnityEngine;

[CreateAssetMenu(menuName = "Buff Effect/Experience Buff", fileName = "Experience_BU")]
public class ExperienceBuff : BuffEffect
{
    
    [Tooltip("Chi phí sử dụng Item nâng cấp")]
    public int UpgradeCost;
    
    public override void Apply(PlayerController _player)
    {
        if(GameManager.Instance.UserData.galacticGems < UpgradeCost) return;

        GameManager.Instance.UserData.galacticGems -= UpgradeCost;
        _player.PlayerConfig.CurrentEXP += (int)Value;
    }
}
