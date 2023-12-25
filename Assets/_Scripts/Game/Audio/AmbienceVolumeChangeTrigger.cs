using FMOD.Studio;
using UnityEngine;

public class AmbienceVolumeChangeTrigger : MonoBehaviour
{
    [Tooltip("Layer kiểm tra va chạm"), SerializeField] private LayerMask layerTrigger;
    [Header("Parameter Change")]
    [SerializeField] private string parameterName;
    [SerializeField, Range(0, 1)] private float parameterValue;
    private static EventInstance _eventInstance;

    private void Start()
    {
        _eventInstance = AudioManager.CreateInstance(FMOD_Events.Instance.dungeonAmbience);
        _eventInstance.start();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!layerTrigger.Contains(other.gameObject)) return;
        SetVolume(parameterValue);
    }
    public void SetVolume(float _volume) =>  _eventInstance.setParameterByName(parameterName, _volume);

}
