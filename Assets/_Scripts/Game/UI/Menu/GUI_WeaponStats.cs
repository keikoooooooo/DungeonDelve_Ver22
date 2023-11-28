using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_WeaponStats : MonoBehaviour, IPlayerRef
{
    public RawImage rawMesh;
    
    [Header("Infor")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponLevelText;

    [Header("Base")]
    public TextBar critRateText;
    public TextBar critDMGText;
    [Header("Attack")]
    public TextBar hit1_DMGText;
    public TextBar hit2_DMGText;
    public TextBar hit3_DMGText;
    public TextBar hit4_DMGText;
    public TextBar hit5_DMGText;
    public TextBar chargedAttack_STCostText;
    public TextBar chargedAttack_DMGText;
    public TextBar elementalSkill_DMGText;
    public TextBar elementalBurst_DMGText;
    [Header("Details")] 
    public TextBar weaponDetailsText;

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
        weaponNameText.text = $"{_playerConfig.WeaponName}";
        
        var weaLv = _playerConfig.WeaponLevel;
        weaponLevelText.text = $"Lv. {weaLv}";

        critRateText.SetValueText($"{_playerConfig.CRITRate} %");
        critDMGText.SetValueText($"{_playerConfig.CRITDMG} %");
        
        hit1_DMGText.SetValueText($"{_playerConfig.NormalAttackMultiplier[0].Multiplier[weaLv]} %");
        hit2_DMGText.SetValueText($"{_playerConfig.NormalAttackMultiplier[1].Multiplier[weaLv]} %");
        hit3_DMGText.SetValueText($"{_playerConfig.NormalAttackMultiplier[2].Multiplier[weaLv]} %");
        hit4_DMGText.SetValueText($"{_playerConfig.NormalAttackMultiplier[3].Multiplier[weaLv]} %");
        hit5_DMGText.SetValueText($"{_playerConfig.NormalAttackMultiplier[4].Multiplier[weaLv]} %");
        
        chargedAttack_STCostText.SetValueText($"{_playerConfig.ChargedAttackStaminaCost}");
        chargedAttack_DMGText.SetValueText($"{_playerConfig.ChargedAttackMultiplier[0].Multiplier[weaLv]} %");
        elementalSkill_DMGText.SetValueText($"{_playerConfig.SkillMultiplier[0].Multiplier[weaLv]} %");
        elementalBurst_DMGText.SetValueText($"{_playerConfig.SpecialMultiplier[0].Multiplier[weaLv]} %");
        
        weaponDetailsText.SetValueText($"{_playerConfig.WeaponInfo}");
    }
    
    
    public void OpenWeaponRenderTexture()
    {
        if (!MenuController.Instance.Player) return;
        var _playerRenderTexture = MenuController.Instance.Player.PlayerData.PlayerRenderTexture;
        _playerRenderTexture.OpenRenderUI(PlayerRenderTexture.RenderType.Weapon);
        rawMesh.texture = _playerRenderTexture.renderTexture;
    }


}
