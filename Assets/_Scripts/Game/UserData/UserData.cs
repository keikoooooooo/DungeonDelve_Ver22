using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Inventory
{
    public ItemTypeEnums itemType;
    public int value;
    public Inventory(ItemTypeEnums _itemType, int _value)
    {
        itemType = _itemType;
        value = _value;
    }
}

[Serializable]
public class UserData
{
    [Tooltip("Tên User")]
    public string username;

    [Tooltip("Nhân vật của người dùng")]
    public CharacterTypeEnums characterType;
    
    [Tooltip("Gems")] 
    public int galacticGems;

    [Tooltip("Dữ liệu Item của User")]
    public List<Inventory> inventories;
    
    public UserData(string _username, int _galacticGems, CharacterTypeEnums _characterType)
    {
        username = _username;
        galacticGems = _galacticGems;
        characterType = _characterType;
        inventories = new List<Inventory>();
    }
    
}
