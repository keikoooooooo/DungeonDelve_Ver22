using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemNameCode 
{
    // Potion (PO)
    POHealth,
    POStamina,
    PODamage,
    PODefense,
    POIceResist,
    
    
    // Jade (JA)
    JARed1,
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
    
    
    // Experience (EXP)
    EXPSmall,
    EXPMedium,
    EXPBig,
    
    
    // Others (OR)
    ORCommonSword,
    ORCommonBow,
    
    
    // Upgrade (UP)
    UPSpearhead1,
    UPSpearhead2,
    UPSpearhead3,
    UPForgedBow,
    UPForgedSword
    
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[Serializable]
public class ItemCustom
{
    public ItemNameCode code;
    public Sprite sprite;
    public ItemRarity ratity;
}

/// <summary>
/// Dữ liệu tất cả các Item trong game: Type và Sprite
/// </summary>
[CreateAssetMenu(fileName = "Item Default Data", menuName = "Game Configuration/Game Item Data")]
public class GameItemData : ScriptableObject
{
    public List<ItemCustom> GameItemDatas = new ();

    public ItemCustom GetItemCustom(ItemNameCode _nameCode) => GameItemDatas.Find(item => item.code == _nameCode);
}
