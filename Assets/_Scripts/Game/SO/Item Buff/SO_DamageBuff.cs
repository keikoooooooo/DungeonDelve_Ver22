using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff Effect/Damage Buff", fileName = "DMG_BU")]
public class SO_DamageBuff : SO_BuffEffect
{
    [Tooltip("Thời gian hủy Buff"), SerializeField]
    private float buffTimeOut;

    private Coroutine _buffCoroutine;
    private int _currentDMG;
    
    public override void Apply(PlayerController _player)
    {
        _currentDMG = _player.PlayerConfig.GetST();
        
        if (_buffCoroutine != null)
        {
            _player.StopCoroutine(_buffCoroutine);
        }
        else
        {
            _player.PlayerConfig.SetST(_currentDMG + Mathf.CeilToInt(Value));
        }
        _buffCoroutine = _player.StartCoroutine(CooldownDeBuff(_player));
    }

    private IEnumerator CooldownDeBuff(PlayerStateMachine _player)
    {
        yield return new WaitForSeconds(buffTimeOut);
        _player.PlayerConfig.SetST(_currentDMG);
        _buffCoroutine = null;
    }
    
}
