using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField, Required] private PlayerController player;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private ProgressBar staminaBar;
    [SerializeField] private CooldownTime skillCooldownTime;
    [SerializeField] private CooldownTime specialCooldownTime;
    [Space] 
    [SerializeField] private Image chapterIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider expProgress;
    
    private void OnEnable()
    {
        if (player == null) return;

        ChangePlayerConfigValue();
        player.E_SkillCD += skillCooldownTime.StartCd;
        player.E_SpecialCD += specialCooldownTime.StartCd;
        player.Health.E_OnValueChanged += healthBar.ChangedValue;
        player.Stamina.E_OnValueChanged += staminaBar.ChangedValue;
        player.E_OnChangePlayerConfig += ChangePlayerConfigValue;
    }
    private void Start()
    {
        Init();
    }
    private void OnDisable()
    {
        if (player == null) return;
        
        player.E_SkillCD -= skillCooldownTime.StartCd;
        player.E_SpecialCD -= specialCooldownTime.StartCd;
        player.Health.E_OnValueChanged -= healthBar.ChangedValue;
        player.Stamina.E_OnValueChanged -= staminaBar.ChangedValue;
        player.E_OnChangePlayerConfig -= ChangePlayerConfigValue;
    }

    private void ChangePlayerConfigValue()
    {
        healthBar.Init(player.PlayerConfig.GetHP());
        staminaBar.Init(player.PlayerConfig.GetST());
        levelText.text = $"Lv. {player.PlayerConfig.GetLevel()}";
    }
    

    private void Init()
    {
        nameText.text = player.PlayerConfig.GetName();
        chapterIcon.sprite = player.PlayerConfig.ChapterIcon;
    }
    
    
}
