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
        cameraSensitivity.InitValue(100, 500, PlayerPrefs.GetFloat(PP_SensitivityIndex, 150));
    }

    
    public void GetRef(PlayerController player)
    { 
        var sensitivityValue = PlayerPrefs.GetFloat(PP_SensitivityIndex, 150);
        _player = player;
        _player.cinemachineFreeLook.m_XAxis.m_MaxSpeed = sensitivityValue;
    }
    
    
    public void SetCameraSensitivity(float _value)
    {
        PlayerPrefs.SetFloat(PP_SensitivityIndex, _value);
        if (!_player) return;
        _player.cinemachineFreeLook.m_XAxis.m_MaxSpeed = _value;
    }


    
    
}
