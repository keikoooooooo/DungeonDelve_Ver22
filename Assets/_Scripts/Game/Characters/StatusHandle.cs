using System;
using UnityEngine;

[Serializable]
public class StatusHandle
{
   public StatusHandle(int maxValue)
   {
      _maxValue = maxValue;
      CurrentValue = maxValue;
   } 
   public event Action<int> E_OnValueChanged;

   private int _maxValue;
   public int CurrentValue { get; private set; }
   


   public void Increases(int _amount)
   {
      CurrentValue += _amount;
      if (CurrentValue >= _maxValue)
         CurrentValue = _maxValue;
      
      E_OnValueChanged?.Invoke(CurrentValue);
   }
   public void Decreases(int _amount)
   {
      CurrentValue -= _amount;
      if (CurrentValue <= 0)
         CurrentValue = 0;
      
      E_OnValueChanged?.Invoke(CurrentValue);
   }
   
}
