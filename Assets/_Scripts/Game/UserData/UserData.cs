using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Inventory
{
    public ItemNameCode itemCode;
    public int value;
    public Inventory(ItemNameCode _itemCode, int _value)
    {
        itemCode = _itemCode;
        value = _value;
    }
}

[Serializable]
public class UserData
{
    [Tooltip("Tên User")]
    public string username;
    
    [Tooltip("Gems")] 
    public int galacticGems;

    [Tooltip("Dữ liệu Item của User")]
    public List<Inventory> inventories;
    
    
    /// <summary>
    /// Tạo một data mới cho user
    /// </summary>
    /// <param name="_username"> Tên của User, có thể lấy username lúc tạo tài khoản. </param>
    /// <param name="_galacticGems"> Đá quý ban đầu cho User </param>
    public UserData(string _username, int _galacticGems)
    {
        username = _username;
        galacticGems = _galacticGems;
        
        inventories = new List<Inventory>
        {
            new(ItemNameCode.POHealth, 5),
            new(ItemNameCode.POStamina, 5),
        };
    }
    
}
