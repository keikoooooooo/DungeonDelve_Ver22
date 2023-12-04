using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_WeaponStats : MonoBehaviour, IGUI
{
    
    [SerializeField] private RawImage rawMesh;
    
    [Header("Infor")]
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI weaponLevelText;
    [Header("Base")]
    [SerializeField] private TextBar critRateText;
    [SerializeField] private TextBar critDMGText;
    [Header("Attack")]
    [SerializeField] private TextBar hit1_DMGText;
    [SerializeField] private TextBar hit2_DMGText;
    [SerializeField] private TextBar hit3_DMGText;
    [SerializeField] private TextBar hit4_DMGText;
    [SerializeField] private TextBar hit5_DMGText;
    [SerializeField] private TextBar chargedAttack_STCostText;
    [SerializeField] private TextBar chargedAttack_DMGText;
    [SerializeField] private TextBar elementalSkill_DMGText;
    [SerializeField] private TextBar elementalBurst_DMGText;
    [Header("Details")] 
    [SerializeField] private TextBar weaponDetailsText;

    
    // Variables
    private SO_PlayerConfiguration _playerConfig;
    private PlayerRenderTexture _playerRender;
    
    
    private void Awake() => GUI_Manager.Add(this);
    private void OnDestroy() => GUI_Manager.Remove(this);
    
    

    public void GetRef(GameManager _gameManager)
    {
        _playerConfig = _gameManager.Player.PlayerConfig;
        _playerRender = _gameManager.Player.PlayerData.PlayerRenderTexture;
        rawMesh.texture = _playerRender.renderTexture;
        
        UpdateData();
    }
    public void UpdateData()
    {
        UpdateStatsText();
    }
    
    private void UpdateStatsText()
    {
        if(!_playerConfig) return;
        
        var weaLv = _playerConfig.GetWeaponLevel();
        weaponLevelText.text = $"Lv. {weaLv}";
        weaponNameText.text = $"{_playerConfig.GetWeaponName()}";

        critRateText.SetValueText($"{_playerConfig.GetCRITRate()} %");
        critDMGText.SetValueText($"{_playerConfig.GetCRITDMG()} %");
        hit1_DMGText.SetValueText($"{_playerConfig.GetNormalAttackMultiplier()[0].Multiplier[weaLv]} %");
        hit2_DMGText.SetValueText($"{_playerConfig.GetNormalAttackMultiplier()[1].Multiplier[weaLv]} %");
        hit3_DMGText.SetValueText($"{_playerConfig.GetNormalAttackMultiplier()[2].Multiplier[weaLv]} %");
        hit4_DMGText.SetValueText($"{_playerConfig.GetNormalAttackMultiplier()[3].Multiplier[weaLv]} %");
        hit5_DMGText.SetValueText($"{_playerConfig.GetNormalAttackMultiplier()[4].Multiplier[weaLv]} %");
        chargedAttack_STCostText.SetValueText($"{_playerConfig.GetChargedAttackSTCost()}");
        chargedAttack_DMGText.SetValueText($"{_playerConfig.ChargedAttackMultiplier[0].Multiplier[weaLv]} %");
        elementalSkill_DMGText.SetValueText($"{_playerConfig.SkillMultiplier[0].Multiplier[weaLv]} %");
        elementalBurst_DMGText.SetValueText($"{_playerConfig.SpecialMultiplier[0].Multiplier[weaLv]} %");
        weaponDetailsText.SetValueText($"{_playerConfig.GetWeaponInfo()}");
    }
    public void OpenRenderTexture()
    {
        if (!_playerRender) return;
        
        _playerRender.OpenRenderUI(PlayerRenderTexture.RenderType.Weapon);
    }



}
