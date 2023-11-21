using System;
using NaughtyAttributes;
using UnityEngine;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField, Required] private EnemyController enemy;
    [SerializeField] private NameAndLevelText nameAndLevelText;
    [SerializeField] private ProgressBar healthBar;
    
    
    private void Start()
    {
        nameAndLevelText.ChangeNameText(enemy.EnemyConfig.Name);
        nameAndLevelText.ChangeLevelText(enemy.EnemyConfig.Level);
        
        enemy.StatusHandle.E_HealthChanged += healthBar.ChangeValue;
        healthBar.Init(enemy.EnemyConfig.MaxHealth);
    }
    private void OnDestroy()
    {
        enemy.StatusHandle.E_HealthChanged -= healthBar.ChangeValue;
    }
    
    
    
}
