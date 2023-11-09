using System;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Tooltip("Slider tiến trình chính"), SerializeField] 
    private Slider mainProgressSlider;
    
    [Tooltip("Slider tiến trình phụ"), ShowIf("UseBackFill"), SerializeField] 
    private Slider backProgressSlider;
    [Tooltip("Có sử dụng Fill phụ không không ?")]
    public bool UseBackFill;


    [Space, Tooltip("Text hiển thị tiến trình"), ShowIf("ShowText"), SerializeField] 
    private TextMeshProUGUI progressText;
    [Tooltip("Có cập nhật giá trị vào Text không")]
    public bool ShowText;
    
    [Space, Tooltip("Thời gian chạy 1 Tween của Fill chính"), SerializeField]
    private float mainDuration = .25f;
    [Tooltip("Thời gian chạy 1 Tween của Fill phụ"), SerializeField]
    private float backDuration = .8f;
    
    private Tween mainProgressTween;
    private Tween backProgressTween;
    
    private void Start()
    {
        ShowText = progressText != null;
        UseBackFill = backProgressSlider != null;
    }


    /// <summary>
    /// Khởi tạo giá trị ban đầu
    /// </summary>
    /// <param name="maxValue"> Giá trị cần khởi tạo </param>
    public void Init(int maxValue)
    {
        mainProgressSlider.minValue = 0;
        mainProgressSlider.maxValue = maxValue;
        mainProgressSlider.value = maxValue;

        if (UseBackFill)
        {
            backProgressSlider.minValue = 0;
            backProgressSlider.maxValue = maxValue;
            backProgressSlider.value = maxValue;
        }
        
        if(ShowText)
            progressText.text = $"{mainProgressSlider.value} / {mainProgressSlider.maxValue}";
    }
    

    /// <summary>
    /// Khi giá trị thay đổi
    /// </summary>
    /// <param name="value"> Giá trị đã thay đổi </param>
    public void ChangeValue(int value)
    {
        value = (int)Mathf.Clamp(value, mainProgressSlider.minValue, mainProgressSlider.maxValue);
        
        mainProgressTween?.Kill();
        mainProgressTween = mainProgressSlider.DOValue(value, mainDuration);

        if (UseBackFill)
        {
            backProgressTween?.Kill();
            backProgressTween = backProgressSlider.DOValue(value, backDuration);
        }
        
        if(ShowText)
            progressText.text = $"{mainProgressSlider.value} / {mainProgressSlider.maxValue}";
    }
    

}
