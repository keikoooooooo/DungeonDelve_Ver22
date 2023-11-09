using System;
using NaughtyAttributes;
using UnityEngine;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField, Required] private EnemyController enemy;
    [SerializeField] private ProgressBar healthBar;
    
    
    private void Start()
    {
        enemy.StatusHandle.E_HealthChanged += healthBar.ChangeValue;
        
        healthBar.Init(enemy.EnemyConfig.MaxHealth);
    }
    private void OnDestroy()
    {
        enemy.StatusHandle.E_HealthChanged -= healthBar.ChangeValue;
    }
    
    
    
}
