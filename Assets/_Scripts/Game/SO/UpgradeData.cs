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
public class UpgradeData : ScriptableObject
{
    public TextAsset LevelingTextAsset;
    public List<DefaultData> defaultDatas; 
    
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


    /// <summary>
    /// Trả về điểm kinh nghiệm tối đa của level tiếp theo
    /// </summary>
    /// <param name="_level"> Level hiện tại của nhân vật </param>
    /// <returns></returns>
    public int GetNextEXP(int _level)
    {
        var idx = 0;
        for (var i = 0; i < defaultDatas.Count; i++)
        {
            if (defaultDatas[i].Level != _level) continue;
            idx = i;
            break;
        }
        return idx != 89 ? defaultDatas[idx + 1].EXP : 999999999;
    }
}
