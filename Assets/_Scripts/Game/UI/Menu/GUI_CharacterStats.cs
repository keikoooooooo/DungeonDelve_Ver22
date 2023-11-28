using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_CharacterStats : MonoBehaviour, IPlayerRef
{
    public RawImage rawMesh;
    
    [Header("Infor")] 
    public TextMeshProUGUI charNameText;
    public TextMeshProUGUI charLevelText;
    public TextMeshProUGUI charCurrentEXPText;
    public Image charChapterIcon;
    
    [Header("BaseStats")]
    public TextBar maxHPText;
    public TextBar maxSTText;
    public TextBar runSpeedText;
    public TextBar elementalSkillText;
    public TextBar elementalBurstText;
    
    [Header("Attack")] 
    public TextBar atkText;
    
    [Header("Defense")] 
    public TextBar defText;

    private PlayerController _player;
    
    
    private void Awake() => PlayerRefGUIManager.Add(this);
    private void OnDestroy() => PlayerRefGUIManager.Remove(this);
    
    
    public void GetRef(PlayerController player)
    {
        _player = player;
        UpdateStatsText(_player.PlayerData.PlayerConfig);
    }

    private void UpdateStatsText(PlayerConfiguration _playerConfig)
    {
        charNameText.text = $"{_playerConfig.Name}";
        charLevelText.text = $"Lv. {_playerConfig.Level}";
        charCurrentEXPText.text = $"{_playerConfig.CurrentEXP}";

        maxHPText.SetValueText($"{_playerConfig.MaxHP}");
        maxSTText.SetValueText($"{_playerConfig.MaxST}");
        runSpeedText.SetValueText($"{_playerConfig.RunSpeed}");
        elementalSkillText.SetValueText($"{_playerConfig.ElementalSkillCD}");
        elementalBurstText.SetValueText($"{_playerConfig.ElementalBurstlCD}");
        
        atkText.SetValueText($"{_playerConfig.ATK}");
        defText.SetValueText($"{_playerConfig.DEF}");
    }
    
    public void OpenCharacterRenderTexture()
    {
        if (!MenuController.Instance.Player) return;
        var _playerRenderTexture = MenuController.Instance.Player.PlayerData.PlayerRenderTexture;
        _playerRenderTexture.OpenRenderUI(PlayerRenderTexture.RenderType.Character);
        rawMesh.texture = _playerRenderTexture.renderTexture;
    }


    
}
