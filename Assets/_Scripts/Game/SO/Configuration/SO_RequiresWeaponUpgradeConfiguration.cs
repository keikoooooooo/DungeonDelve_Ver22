using UnityEngine;
using System;
using System.Collections.Generic;


/// <summary>
/// Cấu hình về các yêu cầu khi nâng cấp vũ khí của nhân vật: gồm số lượng/type của item, và chi phí cần khi nâng cấp
/// </summary>
[Serializable, CreateAssetMenu(fileName = "Weapon Upgrade Config", menuName = "Characters Configuration/Player/Weapon Config")]
public class SO_RequiresWeaponUpgradeConfiguration : ScriptableObject
{
    [Serializable]
    public class UpgradeItem
    {
        public ItemNameCode code;
        public int value;
    }
    
    [Serializable]
    public class RequiresData
    {
        public int levelUpgrade;
        public int coinCost;
        public List<UpgradeItem> requiresItem = new();
    }
    
    [Tooltip("Danh sách các Item cần trên từng mốc khi nâng cấp vũ khí")]
    public List<RequiresData> RequiresDatas = new();
}
