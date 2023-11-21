using UnityEngine;
using UnityEngine.Events;

public class PlayerCallbackEvent : MonoBehaviour
{
    public UnityEvent OnDashEvent;
    public UnityEvent OnDieEvent;
    public UnityEvent OnTakeDMGEvent;
    
    public void CallDashEvent() => OnDashEvent?.Invoke();
    public void CallDeadEvent() => OnDieEvent?.Invoke();
    public void CallTakeDMGEvent() => OnTakeDMGEvent?.Invoke();

    
    public void CallDashEvent(float _delay) => Invoke(nameof(CallDashEvent), _delay);
    public void CallDeadEvent(float _delay) => Invoke(nameof(CallDeadEvent), _delay);
    public void CallTakeDMGEvent(float _delay) => Invoke(nameof(CallTakeDMGEvent), _delay);
}
