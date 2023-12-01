using Cinemachine;
using UnityEngine;


public class GUI_SettingControls : MonoBehaviour, IGUI
{
    [SerializeField] private SliderBar cameraSensitivity;
    
    private CinemachineFreeLook _cinemachineFreeLook;
    
    // Key PlayerPrefs
    private readonly string PP_SensitivityIndex = "SensitivityIndex";
    

    private void Awake() => GUI_Manager.Add(this);
    private void OnEnable()
    {
        cameraSensitivity.InitValue(100, 500, PlayerPrefs.GetFloat(PP_SensitivityIndex, 150));
    }
    private void OnDestroy() => GUI_Manager.Remove(this);
    
    
    public void GetRef(UserData userData, SO_CharacterUpgradeData characterUpgradeData, SO_GameItemData gameItemData, PlayerController player)
    {
        _cinemachineFreeLook = player.cinemachineFreeLook;

        UpdateData();
    }
    
    public void UpdateData()
    {
        var sensitivityValue = PlayerPrefs.GetFloat(PP_SensitivityIndex, 150);
        SetCameraSensitivity(sensitivityValue);
    }
    
    public void SetCameraSensitivity(float _value)
    {
        PlayerPrefs.SetFloat(PP_SensitivityIndex, _value);
        
        if (!_cinemachineFreeLook) return;
        _cinemachineFreeLook.m_XAxis.m_MaxSpeed = _value;
    }
    
}
