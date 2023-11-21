using UnityEngine;

[CreateAssetMenu(menuName = "Buff Effect/Stamina Buff", fileName = "Stamina_BU")]
public class StaminaBuff : BuffEffect
{
    public override void Apply(PlayerStateMachine _player)
    {   
        _player.Stamina.Increase(Mathf.CeilToInt(Value));
    }
    
}
