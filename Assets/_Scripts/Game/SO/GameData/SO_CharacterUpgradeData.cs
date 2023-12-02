using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UpgradeCustom
{
    public int Level;
    public int EXP;
    public int TotalExp;
}

/// <summary>
/// Dự liệu EXP nâng cấp của tất cả Player trong game (Lv 1 -> Lv 90)
/// </summary>
[CreateAssetMenu(fileName = "Upgrade Data", menuName = "Game Configuration/Upgrade Data")]
public class SO_CharacterUpgradeData : ScriptableObject
{
    [SerializeField] private TextAsset LevelingTextAsset;
    public List<UpgradeCustom> UpgradeData;

    private readonly Dictionary<int, int> _upgradeDataDictionary = new();
    public const int levelMax = 90;


    /// <summary>
    /// Hàm này chỉ lấy dữ liệu 1 lần duy nhất trên EDITOR để cập nhật dự liệu vào list Data để tham chiếu và check trong game
    /// </summary>
    public void SetData()
    {
        if(!LevelingTextAsset || UpgradeData.Count != 0) 
            return;
        
        var strContent = LevelingTextAsset.text;
        var files = strContent.Split('\n');

        foreach (var VARIABLE in files)
        {
            var part = VARIABLE.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            UpgradeCustom upgradeCustom = new();
            if (part[0].StartsWith("LV:"))
            {
                var lvStr = part[0].Substring(3);
                if (int.TryParse(lvStr, out var level))
                {
                    upgradeCustom.Level = level;
                }
            }
            if (part[1].StartsWith("EXP:"))
            {
                var expStr= part[1].Substring(4);
                if (int.TryParse(expStr, out var exp))
                {
                   upgradeCustom.EXP = exp;
                }
            }

            UpgradeData.Add(upgradeCustom);
        }
    }
    private void OnEnable()
    {
        _upgradeDataDictionary.Clear();

        foreach (var data in UpgradeData)
        {
            _upgradeDataDictionary.Add(data.Level - 1, data.EXP);
            Debug.Log("SO: Load uprage data in Lv: " + data.Level + "/Exp: " + data.EXP);
        }
    }


    /// <summary>
    /// Trả về điểm kinh nghiệm của level tiếp theo
    /// </summary>
    /// <param name="_level"> Level hiện tại của nhân vật </param>
    /// <returns></returns>
    public int GetNextEXP(int _level)
    {
        if(_level >= levelMax) 
            return UpgradeData[^1].EXP;
        return !_upgradeDataDictionary.TryGetValue(_level - 1, out var _exp) ? 0 : _exp;
    }

    
    /// <summary>
    /// Trả về tổng số điểm kinh nghiệm từ Lv1 -> đến (_currentLevel - 1)
    /// Ví dụ _currentLevel = 3, sẽ trả về tổng điểm kinh nghiệm (Lv1 + Lv2)
    /// </summary>
    /// <param name="_currentLevel"> Level hiện tại. </param>
    /// <returns></returns>
    public int GetTotalEXP(int _currentLevel) => _currentLevel <= 1 ? 0 : UpgradeData[_currentLevel - 2].TotalExp;
    
}
