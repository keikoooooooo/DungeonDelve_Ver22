using System;
using UnityEngine;


/// <summary>
/// Instance quản lí việc tải và lưu mốc thời gian từ PlayerPrefs
/// </summary>
public static class TimeManager
{
    /// <summary>
    /// Lưu xuống PlayerPrefs dạng string của thời gian được truyền vào
    /// </summary>
    /// <param name="_key"> Key: sẽ giữ dữ liệu. </param>
    /// <param name="_dateTime"> Thời gian cần lưu. </param>
    public static void SetTime(string _key, DateTime _dateTime)
    {
        PlayerPrefs.SetString(_key, _dateTime.ToString("O"));
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Lưu xuống PlayerPrefs dạng string của thời gian hiện tại
    /// </summary>
    /// <param name="_key"> Key: sẽ giữ dữ liệu. </param>
    public static void SetTime(string _key)
    {
        PlayerPrefs.SetString(_key, DateTime.Now.ToString("O"));
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Trả về mốc thời gian được lấy từ PlayerPrefs, nếu có trả True và mốc thời gian, ngược lại False và Null
    /// </summary>
    /// <param name="_key"> Key: cần tải dữ liệu. </param>
    /// <returns></returns>
    public static bool GetTime(string _key, out DateTime _dateTime)
    {
        _dateTime = default;
        if (!PlayerPrefs.HasKey(_key)) return false;

        var _strTime = PlayerPrefs.GetString(_key);
        _dateTime = DateTime.Parse(_strTime);
        return true;

    }
}
