using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTime : MonoBehaviour
{
    
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI valueText;

    private bool _valueTextNotNull;
    private Coroutine _cooldownCoroutine;
    private float _durationTemp;


    private void Start()
    {
        _valueTextNotNull = valueText != null;
        ActiveFill(false);
    }

    public void StartCooldownEvent(float duration)
    {
        if(_cooldownCoroutine != null)
            StopCoroutine(_cooldownCoroutine);
        _cooldownCoroutine = StartCoroutine(CooldownCoroutine(duration));
    }
    public void ContinueCooldownEvent()
    {
        if (_durationTemp == 0)
            return;
        if(_cooldownCoroutine != null)
            StopCoroutine(_cooldownCoroutine);
        _cooldownCoroutine = StartCoroutine(CooldownCoroutine(_durationTemp));
    }
    private IEnumerator CooldownCoroutine(float duration)
    {
        var _duration = duration;

        ActiveFill(true);
        while (_duration >= 0)
        {
            var _currentTime = _duration / duration;
            if (fill)
                fill.fillAmount = _currentTime;
            
            if (_valueTextNotNull)
                valueText.text = _duration.ToString("F1");

            _duration -= Time.deltaTime;
            _durationTemp = _duration;
            Debug.Log("Duration: " + duration);
            yield return null;
        }

        _durationTemp = 0;
        ActiveFill(false);
    }

    private void ActiveFill(bool _canActive)
    {
        if (fill) 
            fill.gameObject.SetActive(_canActive);
        if (_valueTextNotNull)
            valueText.gameObject.SetActive(_canActive);
    }
    
    
}
