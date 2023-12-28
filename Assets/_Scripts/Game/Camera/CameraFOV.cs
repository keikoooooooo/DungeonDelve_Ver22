using Cinemachine;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    [field: SerializeField] public CinemachineFreeLook cinemachineFreeLook { get; private set; }
    [field: SerializeField] public float fovMin  { get; private set; } = 18;
    [field: SerializeField] public float fovMax  { get; private set; } = 70;
    
    [SerializeField] private float scrollSpeed = 13;
    private float _fovCurrent;  // giá trị scroll ban đầu
    private float _scrollInput; 


    private void Start()
    {
        _fovCurrent = cinemachineFreeLook.m_Lens.FieldOfView;
    }

    private void Update()
    {
        _scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (_scrollInput == 0 || Time.timeScale <= 0) return;
        
        _fovCurrent -= _scrollInput * scrollSpeed;
        _fovCurrent = Mathf.Clamp(_fovCurrent, fovMin, fovMax);
        cinemachineFreeLook.m_Lens.FieldOfView = _fovCurrent;
    }
    
    
}
