using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Tooltip("Slider tiến trình chính"), Required] 
    public Slider mainProgressSlider;
    
    [Tooltip("Slider tiến trình phụ")] 
    public Slider backProgressSlider;

    [Space, Tooltip("Text hiển thị tiến trình"), ShowIf("ShowText")] 
    public TextMeshProUGUI progressText;
    [Tooltip("Có cập nhật giá trị vào Text không")]
    public bool ShowText;
    
    [Space, Tooltip("Thời gian chạy 1 Tween của Fill chính"), SerializeField]
    private float mainDuration = .25f;
    [Tooltip("Thời gian chạy 1 Tween của Fill phụ"), SerializeField]
    private float backDuration = .8f;
    
    private Tween mainProgressTween;
    private Tween backProgressTween;


    private void OnEnable()
    {
        if (!ShowText || !progressText) return;
        mainProgressSlider.onValueChanged.AddListener(SliderChangeValue);
    }
    private void OnDisable()
    {
        if (!ShowText || !progressText) return;
        mainProgressSlider.onValueChanged.RemoveListener(SliderChangeValue);
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

        backProgressSlider.minValue = 0;
        backProgressSlider.maxValue = maxValue;
        backProgressSlider.value = maxValue;
    }
    

    /// <summary>
    /// Khi giá trị thay đổi
    /// </summary>
    /// <param name="value"> Giá trị đã thay đổi </param>
    public void ChangedValue(int value)
    {
        value = (int)Mathf.Clamp(value, mainProgressSlider.minValue, mainProgressSlider.maxValue);
        
        mainProgressTween?.Kill();
        mainProgressTween = mainProgressSlider.DOValue(value, mainDuration);

        backProgressTween?.Kill();
        backProgressTween = backProgressSlider.DOValue(value, backDuration);
    }


    private void SliderChangeValue(float _value) => progressText.text = $"{_value} / {mainProgressSlider.maxValue}";
    

}
