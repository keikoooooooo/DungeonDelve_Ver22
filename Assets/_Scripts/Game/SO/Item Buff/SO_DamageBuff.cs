using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff Effect/Damage Buff", fileName = "DMG_BU")]
public class SO_DamageBuff : SO_BuffEffect
{
    [Tooltip("Thời gian hủy Buff")]
    public float buffTimeOut;

    private Coroutine _buffCoroutine;
    private int _value;
    
    public override void Apply(PlayerController _player)
    {
        _value = Mathf.CeilToInt(Value);
        
        if (_buffCoroutine != null)
        {
            _player.StopCoroutine(_buffCoroutine);
        }
        else
        {
            _player.PlayerConfig.SetST(_player.PlayerConfig.GetST() + _value);
        }
        _buffCoroutine = _player.StartCoroutine(CooldownDeBuff(_player));
    }

    private IEnumerator CooldownDeBuff(PlayerStateMachine _player)
    {
        yield return new WaitForSeconds(buffTimeOut);
        _player.PlayerConfig.SetST(_player.PlayerConfig.GetST() - _value);
        _buffCoroutine = null;
    }
    
}
