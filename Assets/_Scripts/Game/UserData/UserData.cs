using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public string Name;
    public int Level;
    public int CurrentEXP;
    public int MaxEXP;
    
    public UserData()
    {
        Name = "";
        Level = 1;
        CurrentEXP = 0;
        MaxEXP = 200;
    }

    public void IncreaseEXP(int _value)
    {
        
    }
    public void LevelUp()
    {
        
    }
    
}
