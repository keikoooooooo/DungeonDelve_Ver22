using UnityEngine;

public class GUI_SettingControls : MonoBehaviour
{
    [SerializeField] private SliderBar cameraSensitivity;

    
    // Key PlayerPrefs
    private readonly string PP_SensitivityIndex = "SensitivityIndex";
    
    
    private void Start()
    {
        cameraSensitivity.InitValue(100, 500, 120);

        SetCameraSensitivity( PP_SensitivityIndex.GetFloatPP());
    }


    public void SetCameraSensitivity(float _value)
    {
        PP_SensitivityIndex.SetFloatPP(_value);
        MenuController.Instance.Player.cinemachineFreeLook.m_XAxis.m_MaxSpeed = _value;
    }
    
    
}
