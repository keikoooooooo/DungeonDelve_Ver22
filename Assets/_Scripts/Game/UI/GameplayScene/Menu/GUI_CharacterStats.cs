using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_CharacterStats : MonoBehaviour, IGUI
{
    
    [SerializeField] private RawImage rawMainMesh;
    [SerializeField] private RawImage rawShadowMesh;
    
    [Header("Infor")] 
    [SerializeField] private Image charChapterIcon;
    [SerializeField] private TextMeshProUGUI charNameText;
    [SerializeField] private TextMeshProUGUI charLevelText;
    [SerializeField] private TextMeshProUGUI charCurrentEXPText;
    [Header("BaseStats")]
    [SerializeField] private TextBar maxHPText;
    [SerializeField] private TextBar maxSTText;
    [SerializeField] private TextBar runSpeedText;
    [SerializeField] private TextBar elementalSkillText;
    [SerializeField] private TextBar elementalBurstText;
    [Header("Attack")] 
    [SerializeField] private  TextBar atkText;
    [Header("Defense")] 
    [SerializeField] private  TextBar defText;

    // Variables
    private SO_CharacterUpgradeData _upgradeData;
    private SO_PlayerConfiguration _playerConfig;
    private PlayerRenderTexture _playerRender;

    
    private void Awake() => GUI_Manager.Add(this);
    private void OnDestroy() => GUI_Manager.Remove(this);
    
    
    public void GetRef(UserData userData, SO_CharacterUpgradeData characterUpgradeData, SO_GameItemData gameItemData, PlayerController player)
    {
        _playerConfig = player.PlayerConfig;
        _playerRender = player.PlayerData.PlayerRenderTexture;
        _upgradeData = characterUpgradeData;
        
        UpdateData();
    }
    
    public void UpdateData()
    {
        UpdateStatsText();
        OpenRenderTexture();
    }
    
    private void UpdateStatsText()
    {
        if(!_playerConfig) return;
        
        charNameText.text = $"{_playerConfig.Name}";
        charLevelText.text = $"Lv. {_playerConfig.Level}";
        charCurrentEXPText.text = $"{_playerConfig.CurrentEXP} / {_upgradeData.GetNextEXP(_playerConfig.Level)}";
        charChapterIcon.sprite = _playerConfig.ChapterIcon;

        maxHPText.SetValueText($"{_playerConfig.MaxHP}");
        maxSTText.SetValueText($"{_playerConfig.MaxST}");
        runSpeedText.SetValueText($"{_playerConfig.RunSpeed}");
        elementalSkillText.SetValueText($"{_playerConfig.ElementalSkillCD}");
        elementalBurstText.SetValueText($"{_playerConfig.ElementalBurstlCD}");
        
        atkText.SetValueText($"{_playerConfig.ATK}");
        defText.SetValueText($"{_playerConfig.DEF}");
    }
    private void OpenRenderTexture()
    {
        if (!_playerRender) return;
        
        _playerRender.OpenRenderUI(PlayerRenderTexture.RenderType.Character);
        rawMainMesh.texture = _playerRender.renderTexture;
        rawShadowMesh.texture = rawMainMesh.texture;
    }


}
