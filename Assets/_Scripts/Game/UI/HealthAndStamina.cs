using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HealthAndStamina : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Slider hpSlider;
    [Space]
    [SerializeField] private TextMeshProUGUI stText;
    [SerializeField] private Slider stSlider;
    
    
    [Tooltip("Thời gian chạy 1 Tween của Fill"), SerializeField, Space]
    private float duration = .25f;
    
    private Tween _hpSliderTween;
    private Tween _stSliderTween;

    private int hpMax, stMax;

    
    private void Start()
    {
        Initialized(100, 100);
    }


    public void Initialized(int _hpMax, int _stMax)
    {
        hpMax = _hpMax;
        hpSlider.value = hpMax;
        hpSlider.maxValue = hpMax;
        
        stMax = _stMax;
        stSlider.value = stMax;
        stSlider.maxValue = stMax;

        UpdateText();
    }
    private void UpdateText()
    {
        hpText.text = $"{hpSlider.value} / {hpMax}";
        stText.text = $"{stSlider.value} / {stMax}";
    }
    public void CurrentHP(int val)
    {
        _hpSliderTween?.Kill();
        _hpSliderTween = hpSlider.DOValue(val, duration);

        val = Mathf.Clamp(val, (int)hpSlider.minValue, (int)hpSlider.maxValue);
        hpText.text = $"{val} / {hpMax}";
    }
    public void CurrentST(int val)
    {
        _stSliderTween?.Kill();
        _stSliderTween = stSlider.DOValue(val, duration);
        
        val = Mathf.Clamp(val, (int)stSlider.minValue, (int)stSlider.maxValue);
        stText.text = $"{val} / {stMax}";
    }
    




}
