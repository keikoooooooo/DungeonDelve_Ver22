using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ItemUpgrade
{
    public int Value;
    public ItemType Type;
}

public class PlayerUpgrade 
{
    [Header("Level Upgrade")]
    public int LevelUpgrade;
    [Tooltip("Chi phí nâng cấp")]
    public int UpgradeCost;
    [Tooltip("Kinh nghiệm nâng cấp tối đa của Level tiếp theo")]
    public int MaxExperienceUpgrade;
    [Tooltip("Vật liệu nâng cấp")]
    public List<ItemUpgrade> MaterialsUpgrade;
    
}
