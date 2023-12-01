using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DefaultData
{
    public int Level;
    public int EXP;
}

/// <summary>
/// Dự liệu EXP nâng cấp của tất cả Player trong game (Lv 1 -> Lv 90)
/// </summary>
[CreateAssetMenu(fileName = "Upgrade Data", menuName = "Game Configuration/Upgrade Data")]
public class SO_CharacterUpgradeData : ScriptableObject
{
    public TextAsset LevelingTextAsset;
    public List<DefaultData> defaultDatas;

    private readonly Dictionary<int, int> UpgradeData = new();
    private readonly int levelMax = 90;
    
    
    /// <summary>
    /// Hàm này chỉ lấy dữ liệu 1 lần duy nhất trên EDITOR để cập nhật dự liệu vào list Data để tham chiếu và check trong game
    /// </summary>
    public void SetData()
    {
        if(!LevelingTextAsset || defaultDatas.Count != 0) 
            return;
        
        var strContent = LevelingTextAsset.text;
        var files = strContent.Split('\n');

        foreach (var VARIABLE in files)
        {
            var part = VARIABLE.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            DefaultData defaultData = new();
            if (part[0].StartsWith("LV:"))
            {
                var lvStr = part[0].Substring(3);
                if (int.TryParse(lvStr, out var level))
                {
                    defaultData.Level = level;
                }
            }
            if (part[1].StartsWith("EXP:"))
            {
                var expStr= part[1].Substring(4);
                if (int.TryParse(expStr, out var exp))
                {
                   defaultData.EXP = exp;
                }
            }

            defaultDatas.Add(defaultData);
        }
    }
    private void OnEnable()
    {
        UpgradeData.Clear();

        foreach (var data in defaultDatas)
        {
            UpgradeData.Add(data.Level - 1, data.EXP);
            Debug.Log("SO: Load uprage data in Lv: " + data.Level + "/Exp: " + data.EXP);
        }
    }


    /// <summary>
    /// Trả về điểm kinh nghiệm tối đa của level tiếp theo
    /// </summary>
    /// <param name="_level"> Level hiện tại của nhân vật </param>
    /// <returns></returns>
    public int GetNextEXP(int _level)
    {
        if(_level >= levelMax) 
            return 999999999;
        return !UpgradeData.TryGetValue(_level - 1, out var _exp) ? 0 : _exp;
    }


    /// <summary>
    /// Nhận về level sau khi cộng giá trị exp vào
    /// </summary>
    /// <param name="_currenLevel"> Level hiện tại </param>
    /// <param name="_currentExp"> Điểm kinh nghiệm hiện tại </param>
    /// <param name="_expCheck"> Giá trị Exp cần cộng vào </param>
    /// <returns></returns>
    public int GetLevelDemo(int _currenLevel, int _currentExp, int _expCheck)
    {
        var nextExp = GetNextEXP(_currenLevel);
        var lastExp = nextExp - _currentExp;

        var newLevel = 0;
        var remainingExp = 0;
        
        for (var i = _currenLevel; i < levelMax; i++)
        {
            var _nextExp = GetNextEXP(_currenLevel);
            if (_nextExp < _expCheck)
            {
                _expCheck -= nextExp;
                newLevel++;
            }
            
            
        }

        return 1;
    }
}
