using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour, IGUI
{
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private ProgressBar staminaBar;
    [SerializeField] private CooldownTime skillCooldownTime;
    [SerializeField] private CooldownTime specialCooldownTime;
    [Space] 
    [SerializeField] private Image chapterIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider expProgress;
    
    private PlayerController player;
    private SO_CharacterUpgradeData _characterUpgradeData;
    private bool _isEventRegistered;
    
    
    private void OnEnable()
    {
        GUI_Manager.Add(this);
    }
    private void OnDisable()
    {
        GUI_Manager.Remove(this);
        
        if(!_isEventRegistered) 
            return;
        _isEventRegistered = false;
         
        player.E_SkillCD -= skillCooldownTime.StartCd;
        player.E_SpecialCD -= specialCooldownTime.StartCd;
        player.Health.E_OnValueChanged -= healthBar.ChangedValue;
        player.Stamina.E_OnValueChanged -= staminaBar.ChangedValue;
    }

    
    public void GetRef(GameManager _gameManager)
    {
        player = _gameManager.Player;
        _characterUpgradeData = _gameManager.CharacterUpgradeData;

        if (!_isEventRegistered)
        {
            _isEventRegistered = true;
            
            player.E_SkillCD += skillCooldownTime.StartCd;
            player.E_SpecialCD += specialCooldownTime.StartCd;
            player.Health.E_OnValueChanged += healthBar.ChangedValue;
            player.Stamina.E_OnValueChanged += staminaBar.ChangedValue;
        }
        
        Init();
        UpdateData();
    }
    public void UpdateData()
    {
        ChangedConfigValue();
    }
    
    
    private void Init()
    {
        nameText.text = player.PlayerConfig.GetName();
        chapterIcon.sprite = player.PlayerConfig.ChapterIcon;
        expProgress.minValue = 0;
    }
    private void ChangedConfigValue()
    {
        healthBar.Init(player.PlayerConfig.GetHP());
        staminaBar.Init(player.PlayerConfig.GetST());
        
        var _currentLevel = player.PlayerConfig.GetLevel();
        levelText.text = $"Lv. {_currentLevel}";

        expProgress.maxValue = _characterUpgradeData.GetNextEXP(_currentLevel);
        expProgress.value = player.PlayerConfig.GetCurrentEXP();
    }
    
  
}
