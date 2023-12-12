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
        enemy.Health.OnInitValueEvent += OnConfigChanged;
        enemy.Health.OnValueChangedEvent += healthBar.ChangedValue;
    }
    private void OnDestroy()
    {
        enemy.Health.OnInitValueEvent -= OnConfigChanged;
        enemy.Health.OnValueChangedEvent -= healthBar.ChangedValue;
    }

    private void OnConfigChanged()
    {
        healthBar.Init(enemy.EnemyConfig.GetHP());
        if(nameText) 
            nameText.text = enemy.EnemyConfig.GetName();
        if(levelText) 
            levelText.text = $"Lv. {enemy.EnemyConfig.GetLevel()}";
    }
    
}
