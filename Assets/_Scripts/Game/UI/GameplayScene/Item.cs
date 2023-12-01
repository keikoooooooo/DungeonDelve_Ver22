using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPooled<Item>
{
    [SerializeField] private Image rarityFrame;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI valueText;
    [Space]
    [SerializeField] private Sprite rarityFrameCommon;
    [SerializeField] private Sprite rarityFrameUnCommon;
    [SerializeField] private Sprite rarityFrameRare;
    [SerializeField] private Sprite rarityFrameEpic;
    [SerializeField] private Sprite rarityFrameLegendary;
    
    
    private int value;
    public Sprite GetSprite => itemIcon.sprite;
    public bool CheckValue => value > 0; // Check số lượng item
    
    
    
    /// <summary>
    /// Set Item ra UI
    /// </summary>
    /// <param name="_itemCustom"> Thông tin Item </param>
    /// <param name="_value"> Số lượng Item </param>
    public void SetItem(ItemCustom _itemCustom, int _value)
    {
        value = _value;
        itemIcon.sprite = _itemCustom.sprite;
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

        SetValueText(_value);
    }
    

    private void SetValueText(int _value) => valueText.text = $"{_value}"; 
        
    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<Item> ReleaseCallback { get; set; }
}
