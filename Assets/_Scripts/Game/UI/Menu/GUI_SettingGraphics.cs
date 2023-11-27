using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GUI_SettingGraphics : MonoBehaviour
{
    [SerializeField] private DropdownBar displayModeDropdown;
    [SerializeField] private DropdownBar fpsDropdown;
    [SerializeField] private DropdownBar showFPSDropdown;
    
    [SerializeField] private TextMeshProUGUI showFPSPanel;
    
    
    private List<Resolution> _resolutions; // lưu tất cả độ phân giải mặc định game hỗ trợ
    private readonly List<string> fpsType = new() { "24", "30", "45", "60" };
    private readonly List<string> showFPSType = new() { "ON", "OFF" };

    private List<Resolution> _resolutionCustom = new()
    {
        new Resolution { width = 3840, height = 2160},  // 4K
        new Resolution { width = 2560, height = 1440},  // 2K
        new Resolution { width = 1920, height = 1080},  // FHD
        
        new Resolution { width = 1680, height = 1050},  
        new Resolution { width = 1600, height = 1200},  
        new Resolution { width = 1600, height =  900},  
        new Resolution { width = 1440, height = 1080},  
        
        
    };
    // Key PlayerPrefs
    //private string PP_ResolutionWidth = "Resolution";
    private readonly string PP_CurrentResolutionIndex = "ResolutionIndex";
    
    
    
    private void Start()
    {
        Initialized();
        
    }

    
    private void Initialized()
    {
        var currentResolutionIndex = PlayerPrefs.GetInt(PP_CurrentResolutionIndex, 0);  // tìm độ phân giải trước đó đã lưu (nếu có)
        _resolutions = Screen.resolutions.Reverse().ToList();
        
        List<string> _options = new();
        
        _resolutions.ForEach(item => Debug.Log(item));
        // for (var i = 0; i < _resolutions.Count; i++)
        // {
        //     var typeMode = CheckFullscreenResolution(_resolutions[i]) ? "Fullscreen" : "Windowed";
        //     _options.Add($"{_resolutions[i].width} x {_resolutions[i].height} {typeMode}");
        //
        //     // if (_resolutions[i].width == Screen.currentResolution.width && 
        //     //     _resolutions[i].height == Screen.currentResolution.height)
        //     // {
        //     //     currentResolutionIndex = i;
        //     // }
        // }
        PlayerPrefs.DeleteAll();
        
        foreach (var t in _resolutions)
        {
            var typeMode = CheckFullscreenResolution(t) ? "Fullscreen" : "Windowed";
            _options.Add($"{t.width} x {t.height} {typeMode}");
        }
        
        displayModeDropdown.InitValue(_options, currentResolutionIndex);
        fpsDropdown.InitValue(fpsType, 3);
        showFPSDropdown.InitValue(showFPSType, 0);
        
        OnValueDisplayModeChanged(currentResolutionIndex);
    }
    private static bool CheckFullscreenResolution(Resolution _resolution) => _resolution is { width: >= 1920, height: >= 1080 };

    
    
    public void OnValueDisplayModeChanged(int _index)
    {
        PlayerPrefs.SetInt(PP_CurrentResolutionIndex, _index);
        
        var _res = _resolutions[_index];
        Screen.SetResolution(_res.width, _res.height, CheckFullscreenResolution(_res));
    }
    public void OnValueFPSChanged(int _index)
    {
        Application.targetFrameRate = int.Parse(fpsType[_index]);
    }
    public void OnValueShowFPSChanged(int _index)
    {
        showFPSPanel.gameObject.SetActive(_index == 0);
    }

    
}
