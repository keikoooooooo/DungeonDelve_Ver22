using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingDropdownBar : MonoBehaviour
{
    public TMP_Dropdown Dropdown;
    
    
    /// <summary>
    /// Thiết lập các Option ban đầu của Drowdown
    /// </summary>
    /// <param name="_optionTypes"> Các giá trị thiết lập </param>
    /// <param name="_currentValue"> Giá trị ban đầu của Dropdown </param>
    public void InitValue(string[] _optionValue, int _currentValue)
    {
        Dropdown.options = new List<TMP_Dropdown.OptionData>(_optionValue.Length);
        foreach (var _value in _optionValue)
        {
            var optionData = new TMP_Dropdown.OptionData
            {
                text = _value
            };
            Dropdown.options.Add(optionData);
        }
        Dropdown.value = _currentValue;
        Dropdown.RefreshShownValue();
    }
    

    /// <summary>
    /// Thiết lập các Option ban đầu của Drowdown
    /// </summary>
    /// <param name="_optionValue"> Các giá trị thiết lập </param>
    /// <param name="_optionSprites"> Các Sprite của các giá trị </param>
    /// <param name="_currentValue"> Giá trị ban đầu của Dropdown </param>
    public void InitValue(string[] _optionValue, Sprite[] _optionSprites, int _currentValue)
    {
        Dropdown.options = new List<TMP_Dropdown.OptionData>(_optionValue.Length);

        for (var i = 0; i < _optionValue.Length; i++)
        {
            var optionData = new TMP_Dropdown.OptionData
            {
                text = _optionValue[i],
                image = _optionSprites[i]
            };
            Dropdown.options.Add(optionData);
        }
        Dropdown.value = _currentValue;
        Dropdown.RefreshShownValue();
    }
    
    
    /// <summary>
    /// Thiết lập các option ban đầu của Dropdown
    /// </summary>
    /// <param name="_option"> Danh sách Option được định nghĩa trước </param>
    public void InitValue(List<string> _option)
    {
        Dropdown.ClearOptions();
        Dropdown.AddOptions(_option);
        Dropdown.value = 0;
        Dropdown.RefreshShownValue();
    }
    
    
    /// <summary>
    /// Thiết lập các option ban đầu của Dropdown
    /// </summary>
    /// <param name="_option"> Danh sách Option được định nghĩa trước </param>
    /// <param name="_currentValue"> Giá trị ban đầu của Dropdown </param>
    public void InitValue(List<string> _option, int _currentValue)
    {
        Dropdown.ClearOptions();
        Dropdown.AddOptions(_option);
        Dropdown.value = _currentValue;
        Dropdown.RefreshShownValue();
    }
    

}
