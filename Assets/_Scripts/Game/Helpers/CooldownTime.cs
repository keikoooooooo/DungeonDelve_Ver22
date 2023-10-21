using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTime : MonoBehaviour
{
    
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI valueText;

    private Coroutine _cooldownCoroutine;


    private void Start()
    {
        ActiveFill(false);
    }

    public void StartCooldown(float duration)
    {
        if(_cooldownCoroutine != null)
            StopCoroutine(_cooldownCoroutine);
        _cooldownCoroutine = StartCoroutine(CooldownCoroutine(duration));
    }
    private IEnumerator CooldownCoroutine(float duration)
    {
        var _duration = duration;
        var _currentTime = 0f;
        
        ActiveFill(true);
        
        while (_duration >= 0)
        {
            _currentTime = _duration / duration;
            if (fill)
                fill.fillAmount = _currentTime;
            
            if (valueText)
                valueText.text = _duration.ToString("F1");

            _duration -= Time.deltaTime;
            yield return null;
        }

        ActiveFill(false);
    }

    private void ActiveFill(bool _canActive)
    {
        if (fill) 
            fill.gameObject.SetActive(_canActive);
        if (valueText)
            valueText.gameObject.SetActive(_canActive);
    }
    
    

}
