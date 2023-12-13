using System;

[Serializable]
public class StatusHandle
{
   public StatusHandle() { }
   public StatusHandle(int maxValue)
   {
      MaxValue = maxValue;
      CurrentValue = maxValue;
   } 
   public event Action<int> OnValueChangedEvent;
   public event Action OnInitValueEvent;

   public int MaxValue { get; private set; }
   public int CurrentValue { get; private set; }


   public void InitValue(int _currentValue, int _maxValue)
   {
      MaxValue = _maxValue;
      CurrentValue = _currentValue;
      CallInitValueEvent();
   }
   public void Increases(int _amount)
   {
      CurrentValue += _amount;
      if (CurrentValue >= MaxValue)
         CurrentValue = MaxValue;
      
      CallValueChangeEvent();
   }
   public void Decreases(int _amount)
   {
      CurrentValue -= _amount;
      if (CurrentValue <= 0)
         CurrentValue = 0;

      CallValueChangeEvent();
   }
   public void CallValueChangeEvent() => OnValueChangedEvent?.Invoke(CurrentValue);
   public void CallInitValueEvent() => OnInitValueEvent?.Invoke();
}
