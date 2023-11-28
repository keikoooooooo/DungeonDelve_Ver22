using System;
using UnityEngine;

public class GUI_SettingControls : MonoBehaviour, IPlayerRef
{
    [SerializeField] private SliderBar cameraSensitivity;

    
    private PlayerController _player;
    
    // Key PlayerPrefs
    private readonly string PP_SensitivityIndex = "SensitivityIndex";
    

    private void Awake() => PlayerRefGUIManager.Add(this);
    private void OnDestroy() => PlayerRefGUIManager.Remove(this);
    
    
    
    private void Start()
    {
        cameraSensitivity.InitValue(100, 500, PP_SensitivityIndex.GetFloatPP(150));
    }

    
    public void GetRef(PlayerController player)
    { 
        var sensitivityValue = PP_SensitivityIndex.GetFloatPP(150);
        _player = player;
        _player.cinemachineFreeLook.m_XAxis.m_MaxSpeed = sensitivityValue;
    }
    
    
    public void SetCameraSensitivity(float _value)
    {
        PP_SensitivityIndex.SetFloatPP(_value);

        if (!_player) return;
        _player.cinemachineFreeLook.m_XAxis.m_MaxSpeed = _value;
    }


    
    
}
