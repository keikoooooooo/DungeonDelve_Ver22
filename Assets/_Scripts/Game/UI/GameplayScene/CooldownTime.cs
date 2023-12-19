using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTime : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI valueText;

    private bool _valueTextNotNull;
    private float _durationTemp;
    private float _lastDuration;
    private Coroutine _cooldownCoroutine;

    private void Start()
    {
        _valueTextNotNull = valueText != null;
        ActiveFill(false);
    }
    
    public void OnCooldownTime(float duration)
    {
        _durationTemp = duration;
        _lastDuration = duration;
        if(_cooldownCoroutine != null) StopCoroutine(_cooldownCoroutine);
        _cooldownCoroutine = StartCoroutine(CooldownCoroutine());
    }
    public void ContinueCooldownTime()
    {
        if (_lastDuration == 0)
            return;
        if(_cooldownCoroutine != null) StopCoroutine(_cooldownCoroutine);
        _cooldownCoroutine = StartCoroutine(CooldownCoroutine());
    }
    private IEnumerator CooldownCoroutine()
    {
        ActiveFill(true);
        while (_lastDuration >= 0)
        {
            var _currentTime = _lastDuration / _durationTemp;
            SetFill(_currentTime);
            SetValueText();
            _lastDuration -= Time.deltaTime;
            yield return null;
        }
        _lastDuration = 0;
        ActiveFill(false);
    }
    
    private void ActiveFill(bool _canActive)
    {
        if (fill) 
            fill.gameObject.SetActive(_canActive);
        if (_valueTextNotNull)
            valueText.gameObject.SetActive(_canActive);
    }
    private void SetFill(float _amount)
    {
        if (fill) fill.fillAmount = _amount;
    }
    private void SetValueText()
    {
        if (_valueTextNotNull) valueText.text = _lastDuration.ToString("F1");
    }
    
}
