using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPurchase : MonoBehaviour
{
    [SerializeField] private Animator panelAnimator;
    [Space]
    [SerializeField] private Slider quantitySlider;
    [SerializeField] private TextMeshProUGUI quantityUseText;
    [SerializeField] private TextMeshProUGUI minQuantityValueText;
    [SerializeField] private TextMeshProUGUI maxQuantityValueText;
    [SerializeField] private TextMeshProUGUI costText;
    private float _quantityPurchase;
    public event Action<ItemNameCode, int> OnConfirmPurchaseEvent;


    private void OnEnable()
    {
        panelAnimator.Play("Panel_Disable");
    }
    private void Start()
    {
        quantitySlider.onValueChanged.AddListener(SliderOnValueChange);
    }
    private void OnDestroy()
    {
        quantitySlider.onValueChanged.RemoveListener(SliderOnValueChange);
    }
    
    public void SetPurchasePanel(ShopItemSetup _shopItemSetup)
    {
        panelAnimator.Play("Panel_IN");
        SliderOnValueChange(1);
        var _minPurchase = 1;
        var _maxPurchase = _shopItemSetup.GetPurchaseMax();
        minQuantityValueText.text = $"{_minPurchase}";
        maxQuantityValueText.text = $"{_maxPurchase}";
        quantitySlider.minValue = _minPurchase;
        quantitySlider.maxValue = _maxPurchase;
        costText.text = $"{_shopItemSetup.GetPrice() * _quantityPurchase}";
    }
    public void SliderOnValueChange(float _value)
    {
        _quantityPurchase = _value;
        quantityUseText.text = $"Qty.\n{_value}";
    }
    public void OnClickCancelButton()
    {
        panelAnimator.Play("Panel_OUT");
    }
    public void OnClickConfirmButton()
    {
        
    }


}
