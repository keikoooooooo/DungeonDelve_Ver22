using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Danh sách NameCode để phân biệt/tìm thông tin của Item
/// </summary>
public enum ItemNameCode 
{
    // Potion (PO)
    POHealth = 0,
    POStamina,
    PODamage,
    PODefense,
    POIceResist,
    
    
    // Experience (EXP)
    EXPSmall = 10,
    EXPMedium,
    EXPBig,
        
    
    // Consume (CO)
    COCoin = 20,
    
    
    // Jade (JA)
    JARed1 = 100,
    JARed2,
    JARed3,
    JARed4,
    
    JABlue1,
    JABlue2,
    JABlue3,
    JABlue4,
     
    JAYellow1,
    JAYellow2,
    JAYellow3,
    JAYellow4,
    
    JASliver1,
    JASliver2,
    JASliver3,

    // Upgrade (UP)
    UPSpearhead1 = 200,
    UPSpearhead2,
    UPSpearhead3,
    UPForgedBow,
    UPForgedSword
}


/// <summary>
/// Độ hiếm của Item
/// </summary>
public enum ItemRarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4
}


/// <summary>
/// Thông tin của từng Item
/// </summary>
[Serializable]
public class ItemCustom
{
    public ItemNameCode code;
    public Sprite sprite;
    public ItemRarity ratity;

    public ItemCustom() { }
    public ItemCustom(ItemNameCode _itemCode, ItemRarity _itemRarity)
    {
        code = _itemCode;
        ratity = _itemRarity;
    }
}


/// <summary>
/// Dữ liệu tất cả các Item trong game: Type và Sprite
/// </summary>
[CreateAssetMenu(fileName = "Item Default Data", menuName = "Game Configuration/Game Item Data")]
public class SO_GameItemData : ScriptableObject
{
    public List<ItemCustom> GameItemDatas = new ();
    private readonly Dictionary<ItemNameCode, ItemCustom> ItemData = new();

    
    /// <summary>
    /// Tìm vả trả về thông tin của Item dựa vào nameCode
    /// </summary>
    /// <param name="_nameCode"> Code của item cần tìm </param>
    /// <returns></returns>
    public bool GetItemCustom(ItemNameCode _nameCode, out ItemCustom itemCustom) => ItemData.TryGetValue(_nameCode, out itemCustom);
    
    private void OnEnable()
    {
        ItemData.Clear();
        foreach (var item in GameItemDatas)
        {
            ItemData.TryAdd(item.code, item);
        }
    }
}
