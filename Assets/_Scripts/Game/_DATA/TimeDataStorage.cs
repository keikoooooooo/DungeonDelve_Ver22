using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class TimeData
{
    [SerializeField, JsonProperty] private string key;
    [SerializeField, JsonProperty] private string value;

    public string GetKey() => key;
    public string GetValue() => value;
    public void SetKey(string _value) => key = _value;
    public void SetValue(string _value) => value = _value;
}


public static class TimeDataStorage
{
    private static readonly Dictionary<string, TimeData> _dataStorage = new();

    
    /// <summary>
    /// Cập nhật giá trị mới
    /// </summary>
    /// <param name="_key"> KEY? </param>
    /// <param name="_value"> VALUE? </param>
    public static void Set(string _key, TimeData _value) => _dataStorage.TryAdd(_key, _value);
    
    
    /// <summary>
    /// Trả về giá trị theo KEY(nếu có), còn không trả về Null
    /// </summary>
    /// <param name="_key"> KEY? </param>
    /// <returns></returns>
    public static TimeData Get(string _key) => _dataStorage.TryGetValue(_key, out var _value) ? _value : null;
    
  
    public static string GetJson() => JsonConvert.SerializeObject(_dataStorage);

    public static void ConvertToObject(string _json) => JsonConvert.DeserializeObject<TimeData>(_json);

}
