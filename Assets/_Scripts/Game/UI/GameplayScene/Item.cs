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
    
    private ItemNameCode itemNameCode;
    private int value;
    
    
    public void SetItem(ItemCustom _itemCustom, int _value)
    {
        itemNameCode = _itemCustom.code;
        itemIcon.sprite = _itemCustom.sprite;
        value = _value;
        rarityFrame.sprite = _itemCustom.ratity switch
        {
            ItemRarity.Common => rarityFrameCommon,
            ItemRarity.Uncommon => rarityFrameUnCommon,
            ItemRarity.Rare => rarityFrameRare,
            ItemRarity.Epic => rarityFrameEpic,
            ItemRarity.Legendary => rarityFrameLegendary,
        };
    }

    
    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<Item> ReleaseCallback { get; set; }
}
