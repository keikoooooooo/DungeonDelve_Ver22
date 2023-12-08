using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Item : MonoBehaviour, IPooled<GUI_Item>
{
    [SerializeField] private Image rarityFrame;
    [SerializeField] private Image iconItem;
    [SerializeField] private TextMeshProUGUI valueText;
    [Space]
    [SerializeField] private Sprite rarityFrameCommon;
    [SerializeField] private Sprite rarityFrameUnCommon;
    [SerializeField] private Sprite rarityFrameRare;
    [SerializeField] private Sprite rarityFrameEpic;
    [SerializeField] private Sprite rarityFrameLegendary;
    
    public Sprite GetSprite => iconItem.sprite;
    
    
    
    /// <summary>
    /// Set Item ra UI
    /// </summary>
    /// <param name="_itemCustom"> Thông tin Item </param>
    /// <param name="_value"> Số lượng Item </param>
    public void SetItem(ItemCustom _itemCustom)
    {
        iconItem.sprite = _itemCustom.sprite;
        switch (_itemCustom.ratity)
        {
            case ItemRarity.Common:
                rarityFrame.sprite = rarityFrameCommon;
                break;
            case ItemRarity.Uncommon:
                rarityFrame.sprite = rarityFrameUnCommon;
                break;
            case ItemRarity.Rare:
                rarityFrame.sprite = rarityFrameRare;
                break;
            case ItemRarity.Epic:
                rarityFrame.sprite = rarityFrameEpic;
                break;
            case ItemRarity.Legendary:
                rarityFrame.sprite = rarityFrameLegendary;
                break;
        }
    }
    public void SetValueText(string _textValue) => valueText.text = _textValue;
    
        
    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<GUI_Item> ReleaseCallback { get; set; }
}
