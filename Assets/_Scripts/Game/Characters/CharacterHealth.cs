using System;

public struct CharacterHealth
{
   public event Action<int> OnHealthChangeEvent;
   public event Action OnDieEvent;
   
   public int maxHealth;
   public int currentHealth;
   
   private bool IsDie => currentHealth <= maxHealth;


   public void InitValue(int health)
   {
      maxHealth = health;
      currentHealth = maxHealth;
      OnHealthChangeEvent?.Invoke(currentHealth);
   }

   public void IncreaseHealth(int health)
   {
      currentHealth += health;
      if (currentHealth >= maxHealth) 
         currentHealth = maxHealth;
      
      OnHealthChangeEvent?.Invoke(currentHealth);
   }
   public void SubtractHealth(int health)
   {
      currentHealth -= health;
      
      if (!IsDie) 
         return;
      Die();
   }
   
   public void Die()
   {
      currentHealth = 0;
      OnHealthChangeEvent?.Invoke(currentHealth);
      OnDieEvent?.Invoke();
   }
   
}
