using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Tooltip("Slider tiến trình"), SerializeField] 
    private Slider progressSlider;

    [Space, Tooltip("Có cập nhật giá trị vào Text không")]
    public bool ShowText;
    
    [Tooltip("Text hiển thị tiến trình"), ShowIf("ShowText"), SerializeField] 
    private TextMeshProUGUI progressText;
    
    [Tooltip("Thời gian chạy 1 Tween của Fill"), SerializeField, Space]
    private float duration = .25f;

    
    private Tween progressTween;
    
    private void OnEnable()
    {
        ShowText = progressText != null;
    }

    
    /// <summary>
    /// Khởi tạo giá trị ban đầu
    /// </summary>
    /// <param name="maxValue"> Giá trị cần khởi tạo </param>
    public void Init(int maxValue)
    {
        progressSlider.minValue = 0;
        progressSlider.maxValue = maxValue;
        progressSlider.value = maxValue;

        if(ShowText)
            progressText.text = $"{progressSlider.value} / {progressSlider.maxValue}";
    }
    

    /// <summary>
    /// Khi giá trị thay đổi
    /// </summary>
    /// <param name="value"> Giá trị đã thay đổi </param>
    public void ChangeValue(int value)
    {
        value = (int)Mathf.Clamp(value, progressSlider.minValue, progressSlider.maxValue);
        
        progressTween?.Kill();
        progressTween = progressSlider.DOValue(value, duration);

        if(ShowText)
            progressText.text = $"{progressSlider.value} / {progressSlider.maxValue}";
    }
    

}
