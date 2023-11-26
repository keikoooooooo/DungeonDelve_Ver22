using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SettingGraphicsPanel : MonoBehaviour
{
    [SerializeField] private SettingDropdownBar displayModeDropdown;
    [SerializeField] private SettingDropdownBar fpsDropdown;
    [SerializeField] private SettingDropdownBar showFPSDropdown;
    
    [SerializeField] private TextMeshProUGUI showFPSPanel;
    
    
    private List<Resolution> _resolutions; // lưu tất cả độ phân giải mặc định game hỗ trợ
    private readonly List<string> fpsType = new() { "24", "30", "45", "60" };
    private readonly List<string> showFPSType = new() { "ON", "OFF" };
    
    private int currentResolutionIndex; // độ phân giải hiện tại
    
    
    private void Start()
    {
        Initialized();
    }

    
    private void Initialized()
    {
        _resolutions = Screen.resolutions.ToList();
        _resolutions.Reverse();

        List<string> _options = new();
        for (var i = 0; i < _resolutions.Count; i++)
        {
            var typeMode = CheckFullscreenResolution(_resolutions[i]) ? "Fullscreen" : "Windowed";
            _options.Add($"{_resolutions[i].width} x {_resolutions[i].height} {typeMode}");

            if (_resolutions[i].width == Screen.currentResolution.width && 
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        displayModeDropdown.InitValue(_options, currentResolutionIndex);
        fpsDropdown.InitValue(fpsType, 3);
        showFPSDropdown.InitValue(showFPSType, 0);
    }
    private static bool CheckFullscreenResolution(Resolution _resolution) => _resolution is { width: >= 1920, height: >= 1080 };

    
    
    public void OnValueDisplayModeChanged(int _index)
    {
        var _res = _resolutions[_index];
        Screen.SetResolution(_res.width, _res.height, true);
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
