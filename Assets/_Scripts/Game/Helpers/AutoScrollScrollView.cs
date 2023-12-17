using UnityEngine;
using UnityEngine.UI;

public class AutoScrollScrollView : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField, Tooltip("Tốc độ cuộn")]
    private float speedScroll = 15f;
    [SerializeField, Tooltip("Khi bắt đầu Scroll có tự động cuộn không ?")]
    private bool isScrollStart;
    
    private bool _canScroll => scrollRect.verticalNormalizedPosition > 0 && isScrollStart;

    private void FixedUpdate()
    {
        if (!_canScroll) return;
        scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, 0f, Time.deltaTime * speedScroll);
    }



    /// <summary>
    /// Set giá trị để ScrollRect thực hiện việc Auto Scroll hay không.
    /// TRUE: Tự động Scroll.
    /// FALSE: Tắt việc tự động Scroll.
    /// </summary>
    /// <param name="_value"> Giá trị Set. </param>
    public void SetScroll(bool _value)
    {
        isScrollStart = _value;
        if (_value) scrollRect.verticalNormalizedPosition = 0f;
    }
    
}
