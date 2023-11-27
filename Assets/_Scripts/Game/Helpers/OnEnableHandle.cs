using UnityEngine;
using UnityEngine.Events;

public class OnEnableHandle : MonoBehaviour
{
    public UnityEvent OnEnableEvent;

    private void OnEnable() => OnEnableEvent ?.Invoke();
    
}
