using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField, Required] private EnemyController enemy;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    
    private void Start()
    {
        if(nameText) nameText.text = enemy.EnemyConfig.GetName();
        if(levelText) levelText.text = $"Lv. {enemy.EnemyConfig.GetLevel()}";
        
        enemy.Health.E_OnValueChanged += healthBar.ChangedValue;
        healthBar.Init(enemy.EnemyConfig.GetHP());
    }
    private void OnDestroy()
    {
        enemy.Health.E_OnValueChanged -= healthBar.ChangedValue;
    }
    
    
    
}
