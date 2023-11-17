using UnityEngine;

[CreateAssetMenu(menuName = "Buff Effect/Health Buff", fileName = "Health_BU")]
public class HealthBuff : BuffEffect
{
    
    public override void Apply(PlayerStateMachine _player)
    {
        _player.StatusHandle.Increase(Mathf.CeilToInt(Value), StatusHandle.StatusType.Health);    
    }
}
