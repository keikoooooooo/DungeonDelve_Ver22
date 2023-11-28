using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UserData
{
    [Tooltip("Tên User")]
    public string Username;
    
    [Tooltip("Gems")] 
    public int GalacticGems;

    
    [Serializable]
    public class InventoryData
    {
        public ItemType ItemType;
        public int Value;
    }
    
    private List<InventoryData> UserInventory;
    
    public UserData()
    {
        Username = "";
        GalacticGems = 1000;
    }
    
}
