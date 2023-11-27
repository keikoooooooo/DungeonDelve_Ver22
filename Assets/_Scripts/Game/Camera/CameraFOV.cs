using Cinemachine;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    
    [SerializeField] private float fovMin = 30;
    [SerializeField] private float fovMax = 50;
    [SerializeField] private float scrollSpeed = 10;
    
    private float _fovCurrent;  // giá trị scroll ban đầu
    private float _scrollInput; 


    private void Start()
    {
        _fovCurrent = cinemachineFreeLook.m_Lens.FieldOfView;
    }

    private void Update()
    {
        _scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (_scrollInput == 0) return;
        
        _fovCurrent -= _scrollInput * scrollSpeed;
        _fovCurrent = Mathf.Clamp(_fovCurrent, fovMin, fovMax);
        cinemachineFreeLook.m_Lens.FieldOfView = _fovCurrent;
    }
    
    
}
