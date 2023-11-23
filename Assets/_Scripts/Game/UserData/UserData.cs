using System;
using UnityEngine;

[Serializable]
public class UserData
{
    [Tooltip("Tên User")]
    public string Name;
    
    [Tooltip("Cấp độ User")] 
    public int Level;

    [Tooltip("Gems")] 
    public int GalacticGems;
    
    [Tooltip("Kinh nghiệm hiện tại của User")] 
    public int CurrentEXP;
    
    [Tooltip("Kinh nghiệm tối đa của User cho Level tiếp theo")] 
    public int MaxEXP;
    
    public UserData()
    {
        Name = "";
        Level = 1;
        GalacticGems = 1000;
        CurrentEXP = 0;
        MaxEXP = 200;
    }
    
}
