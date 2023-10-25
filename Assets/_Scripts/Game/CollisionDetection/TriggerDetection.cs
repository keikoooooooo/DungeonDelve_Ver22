using UnityEngine;
using UnityEngine.Events;

public class TriggerDetection : MonoBehaviour
{

    [Tooltip("Layer cần kiểm tra va chạm")]
    public LayerMask layerToCheck;
    
    [Space]
    public UnityEvent<Collider> OnTriggerEnterEvent;


    private void OnTriggerEnter(Collider other)
    {
        if(!layerToCheck.Contains(other.gameObject)) return;
        
        OnTriggerEnterEvent?.Invoke(other);
    }
}
