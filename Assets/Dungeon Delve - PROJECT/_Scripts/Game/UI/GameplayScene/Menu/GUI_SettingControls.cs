using Cinemachine;
using UnityEngine;


public class GUI_SettingControls : MonoBehaviour, IGUI
{
    [SerializeField] private SliderBar cameraSensitivity;
    
    private CinemachineFreeLook _cinemachineFreeLook;
    
    // Key PlayerPrefs
    private readonly string PP_SensitivityIndex = "SensitivityIndex";
    
    private void OnEnable()
    {
        GUI_Manager.Add(this);
        cameraSensitivity.InitValue(100, 500, PlayerPrefs.GetFloat(PP_SensitivityIndex, 150));
    }
    private void OnDisable() => GUI_Manager.Remove(this);
    
    
    public void GetRef(GameManager _gameManager)
    {
        _cinemachineFreeLook = _gameManager.Player.FreeLookCamera;
        
        var sensitivityValue = PlayerPrefs.GetFloat(PP_SensitivityIndex, 150);
        SetCameraSensitivity(sensitivityValue);
    }
    public void UpdateData() { }
    
    
    public void SetCameraSensitivity(float _value)
    {
        if (!_cinemachineFreeLook) return;
        
        PlayerPrefs.SetFloat(PP_SensitivityIndex, _value);
        _cinemachineFreeLook.m_XAxis.m_MaxSpeed = _value;
    }
    
}
