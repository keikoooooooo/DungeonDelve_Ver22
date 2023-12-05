using System;

[Serializable]
public class StatusHandle
{
   public StatusHandle() { }
   public StatusHandle(int maxValue)
   {
      _maxValue = maxValue;
      CurrentValue = maxValue;
   } 
   public event Action<int> E_OnValueChanged;

   private int _maxValue;
   public int CurrentValue { get; private set; }


   public void InitValue(int maxValue)
   {
      _maxValue = maxValue;
      CurrentValue = maxValue;
      CallValueChangeEvent();
   }
   public void Increases(int _amount)
   {
      CurrentValue += _amount;
      if (CurrentValue >= _maxValue)
         CurrentValue = _maxValue;
      
      CallValueChangeEvent();
   }
   public void Decreases(int _amount)
   {
      CurrentValue -= _amount;
      if (CurrentValue <= 0)
         CurrentValue = 0;

      CallValueChangeEvent();
   }
   public void CallValueChangeEvent() => E_OnValueChanged?.Invoke(CurrentValue);
   
}
