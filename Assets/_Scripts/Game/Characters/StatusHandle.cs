using System;

[Serializable]
public class StatusHandle
{
   public StatusHandle(int _maxHealth)
   {
      this._maxHealth = _maxHealth;
      CurrentHealth = _maxHealth;
   } 
   public StatusHandle(int _maxHealth, int _maxStamina)
   {
      this._maxHealth = _maxHealth;
      this._maxStamina = _maxStamina;
       
      CurrentHealth = _maxHealth;
      CurrentStamina = _maxStamina;
   }

   
   
   public enum StatusType
   {
      Health,
      Stamina
   }
   
   public event Action<int> E_HealthChanged;
   public event Action<int> E_StaminaChaged; 
   public event Action E_Die;

   
   private int _maxHealth;
   public int CurrentHealth { get; private set; }
   
   private int _maxStamina;
   public int CurrentStamina { get; private set; }
   
   

   public void Increase(int _amount, StatusType _statusType)
   {
      switch (_statusType)
      {
         case StatusType.Health:
            CurrentHealth += _amount;
            if (CurrentHealth >= _maxHealth) CurrentHealth = _maxHealth;
            E_HealthChanged?.Invoke(CurrentHealth);
            break;
         
         case StatusType.Stamina:
            CurrentStamina += _amount;
            if (CurrentStamina >= _maxStamina) CurrentStamina = _maxStamina;
            E_StaminaChaged?.Invoke(CurrentStamina);
            break;
      }
   }
   public void Subtract(int _amount, StatusType _statusType)
   {
      switch (_statusType)
      {
         case StatusType.Health:
            CurrentHealth -= _amount;
            if (CurrentHealth <= 0)
            {
               CurrentHealth = 0;
               E_HealthChanged?.Invoke(CurrentHealth);
               E_Die?.Invoke();
               return;
            }
            E_HealthChanged?.Invoke(CurrentHealth);
            break;
         
         case StatusType.Stamina:
            CurrentStamina -= _amount;
            if (CurrentStamina <= 0) CurrentStamina = 0;
            E_StaminaChaged?.Invoke(CurrentStamina);
            break;
      }
   }
   
}
