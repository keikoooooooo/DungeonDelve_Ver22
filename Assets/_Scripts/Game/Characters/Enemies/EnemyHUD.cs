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
        
        enemy.Health.OnInitValueEvent += healthBar.Init;
        enemy.Health.OnInitValueEvent += (i1, i2) => levelText.text = $"Lv. {enemy.EnemyConfig.GetLevel()}";
        enemy.Health.OnCurrentValueChangeEvent += healthBar.OnCurrentValueChange;
    }
    private void OnDestroy()
    {
        enemy.Health.OnInitValueEvent -= healthBar.Init;
        enemy.Health.OnInitValueEvent -= (i1, i2) => levelText.text = $"Lv. {enemy.EnemyConfig.GetLevel()}";
        enemy.Health.OnCurrentValueChangeEvent -= healthBar.OnCurrentValueChange;
    }

    
}
