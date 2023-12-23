using UnityEngine;

public class AmbienceVolumeChangeTrigger : MonoBehaviour
{
    [Tooltip("Layer kiểm tra va chạm"), SerializeField] private LayerMask layerTrigger;
    [Header("Parameter Change")]
    [SerializeField] private string parameterName;
    [SerializeField, Range(0, 1)] private float parameterValue;
    
    
    private void Change() => AudioManager.Instance.SetAmbienceParameter(parameterName, parameterValue);
    private void OnTriggerEnter(Collider other)
    {
        if (!layerTrigger.Contains(other.gameObject)) return;
        Change();
    }
}
