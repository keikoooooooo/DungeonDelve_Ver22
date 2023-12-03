using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class UpgradeCustom
{
    public int Level { get; private set; }
    public int EXP { get; private set; }
    public int TotalExp;

    public UpgradeCustom(int _level, int _exp)
    {
        Level = _level;
        EXP = _exp;
    }
}

/// <summary>
/// Dự liệu EXP nâng cấp của tất cả Player trong game (Lv 1 -> Lv 90)
/// </summary>
[CreateAssetMenu(fileName = "Upgrade Data", menuName = "Game Configuration/Upgrade Data")]
public class SO_CharacterUpgradeData : ScriptableObject
{
    [SerializeField] private TextAsset LevelingTextAsset;
    public List<UpgradeCustom> DataList;

    private readonly Dictionary<int, int> _upgradeDataDictionary = new();
    public readonly int levelMax  = 90;



    /// <summary>
    /// Hàm này chỉ lấy dữ liệu 1 lần duy nhất trên EDITOR để cập nhật dự liệu vào list Data để tham chiếu và check trong game
    /// </summary>
    public void SetData()
    {
        if(!LevelingTextAsset || DataList.Count != 0) 
            return;
        
        var strContent = LevelingTextAsset.text;
        var files = strContent.Split('\n');
        foreach (var VARIABLE in files)
        {
            var part = VARIABLE.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            var _level = 0;
            if (part[0].StartsWith("LV:"))
            {
                var lvStr = part[0].Substring(3);
                if (int.TryParse(lvStr, out var level))
                {
                    _level = level;
                }
            }

            var _exp = 0;
            if (part[1].StartsWith("EXP:"))
            {
                var expStr= part[1].Substring(4);
                if (int.TryParse(expStr, out var exp))
                {
                   _exp = exp;
                }
            }
            DataList.Add(new UpgradeCustom(_level, _exp));
        }
    }
    private void OnEnable()
    {
        _upgradeDataDictionary.Clear();
        foreach (var data in DataList)
        {
            _upgradeDataDictionary.TryAdd(data.Level - 1, data.EXP);
        }
    }
    public void RenewValue()
    {
        DataList.Clear();
        _upgradeDataDictionary.Clear();
    }

    
    /// <summary>
    /// Trả về điểm kinh nghiệm của level tiếp theo
    /// </summary>
    /// <param name="_level"> Level hiện tại của nhân vật </param>
    /// <returns></returns>
    public int GetNextEXP(int _level)
    {
        if(_level >= levelMax) 
            return DataList[^1].EXP;
        return !_upgradeDataDictionary.TryGetValue(_level - 1, out var _exp) ? 0 : _exp;
    }

    
    /// <summary>
    /// Trả về tổng số điểm kinh nghiệm từ Lv1 -> đến (_currentLevel - 1)
    /// Ví dụ _currentLevel = 3, sẽ trả về tổng điểm kinh nghiệm (Lv1 + Lv2)
    /// </summary>
    /// <param name="_currentLevel"> Level hiện tại. </param>
    /// <returns></returns>
    public int GetTotalEXP(int _currentLevel) => _currentLevel <= 1 ? 0 : DataList[_currentLevel - 2].TotalExp;
    
}
