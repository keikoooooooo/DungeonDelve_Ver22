using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTime : MonoBehaviour
{
    
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI valueText;

    private bool valueTextNotNull;
    private Coroutine _cooldownCoroutine;


    private void Start()
    {
        valueTextNotNull = valueText != null;
        ActiveFill(false);
    }

    public void StartCd(float duration)
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
            
            if (valueTextNotNull)
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
        if (valueTextNotNull)
            valueText.gameObject.SetActive(_canActive);
    }
    
    

}
